using AutoMapper;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Repository
{
    public class BoxInfoRepo : IDisposable
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoxModelEntities db = new BoxModelEntities();

        #region Get Operations
        // Initialise
        public async Task<SearchViewModel> GetEmptyBox(SearchViewModel spvm)
        {
            try
            {
                List<BoxInfo> boxInfos = new List<BoxInfo>();
                await Task.Run(async () =>
                {
                    // Prep address search strings
                    var addresses = db.Addresses.ToList();
                    if (addresses.Any())
                    {
                        foreach (var address in addresses)
                        {
                            if (address.PreBoxInfo.Any())
                            {
                                IList<BoxInfo> pboxes = Mapper.Map<IList<PreBoxInfo>, IList<BoxInfo>>(address.PreBoxInfo.ToList());
                                foreach (var box in pboxes)
                                {
                                    PreBoxInfo prebox = db.PreBoxInfoes.Where(x => x.preboxId == box.preboxID).FirstOrDefault();
                                    if (prebox != default(PreBoxInfo))
                                    {
                                        box.addressstring = string.Format("Block-PB : {0} , Road-PB : {1} , PostalCode-PB : {2}" +
                                                            "Block-CB : NIL , Road-CB : NIL , PostalCode-CB : NIL"
                                                            , box.preboxblock, box.preboxroad, box.preboxpostalcode);
                                        box.searchstring = "";
                                        boxInfos.Add(box);
                                    }
                                    BoxInfo inflatedbox = await getComBoxComRec(box);
                                    if (inflatedbox.comboxID != 0)
                                    {
                                        inflatedbox.addressstring = string.Format("Block-PB : {0} , Road-PB : {1} , PostalCode-PB : {2} , " +
                                                            "Block-CB : {3} , Road-CB : {4} , PostalCode-CB : {5}"
                                                            , inflatedbox.preboxblock, inflatedbox.preboxroad, inflatedbox.preboxpostalcode, inflatedbox.comboxblock
                                                            , inflatedbox.comboxroad, inflatedbox.comboxrptpostalcode);
                                        inflatedbox.searchstring = "";
                                        boxInfos.Add(inflatedbox);
                                    }
                                }
                            }
                            else if (address.ComBoxInfo.Any())
                            {
                                IList<BoxInfo> cboxes = Mapper.Map<IList<ComBoxInfo>, IList<BoxInfo>>(address.ComBoxInfo.ToList());
                                foreach (var box in cboxes)
                                {
                                    BoxInfo inflatedbox = await getPreBoxComRec(box);
                                    if (inflatedbox.comboxID != 0)
                                    {
                                        inflatedbox.addressstring = string.Format("Block-PB : N/A , Road-PB : N/A , PostalCode-PB : N/A , " +
                                                            "Block-CB : {0} , Road-CB : {1} , PostalCode-CB : {2}"
                                                            , inflatedbox.comboxblock, inflatedbox.comboxroad, inflatedbox.comboxrptpostalcode);
                                        inflatedbox.searchstring = "";
                                        boxInfos.Add(inflatedbox);
                                    }
                                }
                            }
                        }                        
                    }

                    // Prep simcard search strings
                    var preboxsimcards = db.PreBoxInfoes.ToList();
                    if (preboxsimcards.Any())
                    {
                        foreach (var preboxsimcard in preboxsimcards)
                        {
                            BoxInfo box = Mapper.Map<PreBoxInfo, BoxInfo>(preboxsimcard);
                            box.simcardstring = string.Format("Simcard : {0} , Telco : {1}"
                                                , box.preboxsimnum, box.preboxtelco);
                            BoxInfo inflatedbox = await getComBoxComRec(box);
                            inflatedbox.searchstring = "";
                            boxInfos.Add(inflatedbox);
                        }
                    }

                    // Prep lmpd search strings
                    var preboxlmpds = db.PreBoxInfoes.ToList();
                    if (preboxlmpds.Any())
                    {
                        foreach (var preboxlmpd in preboxlmpds)
                        {
                            BoxInfo box = Mapper.Map<PreBoxInfo, BoxInfo>(preboxlmpd);
                            box.lmpdstring = string.Format("LMPD-PB : {0} , JSONID-PB : {1} , CheckerName-PB : {2} , Lift-PB : {3}" +
                                                     "LMPD-CB : NIL , LIFT-CB : NIL"
                                                , box.preboxlmpdnum, box.preboxjsonid, box.preboxcheckername, box.preboxlift);
                            BoxInfo inflatedbox = await getComBoxComRec(box);
                            if (inflatedbox.comboxID != 0)
                            {
                                inflatedbox.lmpdstring = string.Format("LMPD-PB : {0} , JSONID-PB : {1} , CheckerName-PB : {2} , Lift-PB : {3}" +
                                                     "LMPD-CB : {4} , LIFT-CB : {5}"
                                                     , inflatedbox.preboxlmpdnum, inflatedbox.preboxjsonid, inflatedbox.preboxcheckername
                                                     , inflatedbox.preboxlift, inflatedbox.comboxlmpdnum, inflatedbox.comboxrptlift);
                                inflatedbox.searchstring = "";
                                boxInfos.Add(inflatedbox);
                            }
                            else
                            {
                                box.searchstring = "";
                                boxInfos.Add(box);
                            }                            
                        }
                    }

                    return boxInfos;
                }).ContinueWith((SearchResultVM) =>
                {
                    if (SearchResultVM.Result.Any())
                    {
                        spvm.records = new BoxInfoRecord(SearchResultVM.Result.ToArray<BoxInfo>());
                        logger.Info(String.Format("Search result(s) : ({0}) obtained.", SearchResultVM.Result.Count));
                    }
                    logger.Info("No Search Results");
                });
                return spvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return spvm;
            }
        }

        // Search
        public async Task<SearchViewModel> GetBoxInfos (SearchViewModel spvm)
        {
            try
            {
                string mySearch = string.IsNullOrWhiteSpace(spvm.searchstring) ? "" : spvm.searchstring.ToLower();
                if(mySearch =="")
                {
                    throw new Exception("User Send Empty String"); // can be handled in validation
                }
                List<BoxInfo> boxInfos = new List<BoxInfo>();
                await Task.Run(async () =>
                {
                    // Prep address search strings
                    var addresses = db.Addresses.ToList();
                    if (addresses.Any())
                    {
                        foreach (var address in addresses)
                        {
                            if (address.PreBoxInfo.Any())
                            {
                                IList<BoxInfo> pboxes = Mapper.Map<IList<PreBoxInfo>, IList<BoxInfo>>(address.PreBoxInfo.ToList());
                                foreach (var box in pboxes)
                                {
                                    box.searchstring = spvm.searchstring;
                                    PreBoxInfo prebox = db.PreBoxInfoes.Where(x => x.preboxId == box.preboxID).FirstOrDefault();
                                    if (prebox != default(PreBoxInfo))
                                    {
                                        box.addressstring = string.Format("Block-PB : {0} , Road-PB : {1} , PostalCode-PB : {2}" +
                                                            "Block-CB : NIL , Road-CB : NIL , PostalCode-CB : NIL"
                                                            , box.preboxblock, box.preboxroad, box.preboxpostalcode);
                                        box.addressstring = box.addressstring.ToLower().Contains(mySearch) ? box.addressstring : "";
                                    }
                                    BoxInfo inflatedbox = await getComBoxComRec(box);
                                    if (inflatedbox.comboxID != 0)
                                    {
                                        inflatedbox.addressstring = string.Format("Block-PB : {0} , Road-PB : {1} , PostalCode-PB : {2} , " +
                                                            "Block-CB : {3} , Road-CB : {4} , PostalCode-CB : {5}"
                                                            , inflatedbox.preboxblock, inflatedbox.preboxroad, inflatedbox.preboxpostalcode, inflatedbox.comboxblock
                                                            , inflatedbox.comboxroad, inflatedbox.comboxrptpostalcode);
                                        inflatedbox.addressstring = inflatedbox.addressstring.ToLower().Contains(mySearch) ? inflatedbox.addressstring : "";
                                        if(inflatedbox.addressstring != "")
                                            boxInfos.Add(inflatedbox);
                                    }
                                    else
                                    {
                                        if (box.addressstring != "")
                                            boxInfos.Add(box);
                                    }
                                }
                            }
                            else if (address.ComBoxInfo.Any())
                            {
                                IList<BoxInfo> cboxes = Mapper.Map<IList<ComBoxInfo>, IList<BoxInfo>>(address.ComBoxInfo.ToList());
                                foreach (var box in cboxes)
                                {
                                    box.searchstring = spvm.searchstring;
                                    BoxInfo inflatedbox = await getPreBoxComRec(box);
                                    if (inflatedbox.comboxID != 0)
                                    {
                                        inflatedbox.addressstring = string.Format("Block-PB : N/A , Road-PB : N/A , PostalCode-PB : N/A , " +
                                                            "Block-CB : {0} , Road-CB : {1} , PostalCode-CB : {2}"
                                                            , inflatedbox.comboxblock, inflatedbox.comboxroad, inflatedbox.comboxrptpostalcode);
                                        inflatedbox.addressstring = inflatedbox.addressstring.ToLower().Contains(mySearch) ? inflatedbox.addressstring : "";
                                        if (inflatedbox.addressstring != "")
                                            boxInfos.Add(inflatedbox);
                                    }
                                    else
                                    {
                                        if (box.addressstring != "")
                                            boxInfos.Add(box);
                                    }
                                }
                            }
                        }
                    }

                    // Prep simcard search strings
                    var preboxsimcards = db.PreBoxInfoes.ToList();
                    if (preboxsimcards.Any())
                    {
                        foreach (var preboxsimcard in preboxsimcards)
                        {
                            BoxInfo box = Mapper.Map<PreBoxInfo, BoxInfo>(preboxsimcard);
                            box.searchstring = spvm.searchstring;
                            box.simcardstring = string.Format("Simcard : {0} , Telco : {1}"
                                                , box.preboxsimnum, box.preboxtelco);
                            box.simcardstring = box.simcardstring.ToLower().Contains(mySearch) ? box.simcardstring : "";
                            if (box.simcardstring != "")
                            {
                                BoxInfo inflatedbox = await getComBoxComRec(box);
                                boxInfos.Add(inflatedbox);
                            }
                        }
                    }

                    // Prep lmpd search strings
                    var preboxlmpds = db.PreBoxInfoes.ToList();
                    if (preboxlmpds.Any())
                    {
                        foreach (var preboxlmpd in preboxlmpds)
                        {
                            BoxInfo box = Mapper.Map<PreBoxInfo, BoxInfo>(preboxlmpd);
                            box.searchstring = spvm.searchstring;
                            box.lmpdstring = string.Format("LMPD-PB : {0} , JSONID-PB : {1} , CheckerName-PB : {2} , Lift-PB : {3}" +
                                                     "LMPD-CB : NIL , LIFT-CB : NIL"
                                                , box.preboxlmpdnum, box.preboxjsonid, box.preboxcheckername, box.preboxlift);
                            box.lmpdstring = box.lmpdstring.ToLower().Contains(mySearch) ? box.lmpdstring : "";
                            BoxInfo inflatedbox = await getComBoxComRec(box);
                            if (inflatedbox.comboxID != 0)
                            {
                                inflatedbox.lmpdstring = string.Format("LMPD-PB : {0} , JSONID-PB : {1} , CheckerName-PB : {2} , Lift-PB : {3}" +
                                                     "LMPD-CB : {4} , LIFT-CB : {5}"
                                                     , inflatedbox.preboxlmpdnum, inflatedbox.preboxjsonid, inflatedbox.preboxcheckername
                                                     , inflatedbox.preboxlift, inflatedbox.comboxlmpdnum, inflatedbox.comboxrptlift);
                                inflatedbox.lmpdstring = inflatedbox.lmpdstring.ToLower().Contains(mySearch) ? inflatedbox.lmpdstring : "";
                                if (inflatedbox.lmpdstring != "")
                                    boxInfos.Add(inflatedbox);
                            }
                            else
                            {
                                if (box.lmpdstring != "")
                                    boxInfos.Add(box);
                            }
                        }
                    }

                    return boxInfos;
                }).ContinueWith((SearchResultVM) =>
                {
                    if (SearchResultVM.Result.Any())
                    {
                        spvm.records = new BoxInfoRecord(SearchResultVM.Result.ToArray<BoxInfo>());
                        logger.Info(String.Format("Search result(s) : ({0}) obtained.", SearchResultVM.Result.Count));
                    }
                    logger.Info("No Search Results");
                });
                return spvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return spvm;
            }
        }

        //Partial PBox
        public async Task<PreBoxInfoViewModel> GetPBoxInfo(SearchViewModel model)
        {
            try
            {
                string searchparam = model.searchstring;
                PreBoxInfoViewModel pbvm = new PreBoxInfoViewModel();
                await Task.Run(() =>
                {
                    List<BoxInfo> boxes = model.records.Cast<BoxInfo>().ToList();
                    BoxInfo box = model.records.Cast<BoxInfo>().ToList()
                                    .Where(x => x.addressstring == searchparam || x.lmpdstring == searchparam || x.simcardstring == searchparam)
                                    .FirstOrDefault();
                    PreBoxInfo pbox = box != null ? db.PreBoxInfoes.Where(x => x.preboxId == box.preboxID).FirstOrDefault() : null;
                    pbvm = pbox != null ? Mapper.Map<PreBoxInfo, PreBoxInfoViewModel>(pbox) : null;
                });
                return pbvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return default(PreBoxInfoViewModel);
            }
        }

        public async Task<ComBoxInfoViewModel> GetCBoxInfo(SearchViewModel model)
        {
            try
            {
                string searchparam = model.searchstring;
                ComBoxInfoViewModel cbvm = new ComBoxInfoViewModel();
                await Task.Run(() =>
                {
                    List<BoxInfo> boxes = model.records.Cast<BoxInfo>().ToList();
                    BoxInfo box = model.records.Cast<BoxInfo>().ToList()
                                    .Where(x => x.addressstring == searchparam || x.lmpdstring == searchparam || x.simcardstring == searchparam)
                                    .FirstOrDefault();
                    ComBoxInfo cbox = box != null ? db.ComBoxInfoes.Where(x => x.comboxId == box.comboxID).FirstOrDefault() : null;
                    cbvm = cbox != null ? Mapper.Map<ComBoxInfo, ComBoxInfoViewModel>(cbox) : null;
                });
                return cbvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return default(ComBoxInfoViewModel);
            }
        }

        public async Task<CommissionRecordVM> GetComRec(SearchViewModel model)
        {
            try
            {
                string searchparam = model.searchstring;
                CommissionRecordVM comrecvm = new CommissionRecordVM();
                await Task.Run(() =>
                {
                    List<BoxInfo> boxes = model.records.Cast<BoxInfo>().ToList();
                    BoxInfo box = model.records.Cast<BoxInfo>().ToList()
                                    .Where(x => x.addressstring == searchparam || x.lmpdstring == searchparam || x.simcardstring == searchparam)
                                    .FirstOrDefault();
                    CommissionRecord comrec = box != null ? db.CommissionRecords.Where(x => x.comrecId == box.comrecID).FirstOrDefault() : null;
                    comrecvm = comrec != null ? Mapper.Map<CommissionRecord, CommissionRecordVM>(comrec) : null;
                });
                return comrecvm;
            }
            catch (Exception exception)
            {
                logger.Error("An attempted to access the DB failed. Error Trace :" + exception);
                return default(CommissionRecordVM);
            }
        }

        #endregion

        #region Helpers

        public async Task<BoxInfo> getComBoxComRec(BoxInfo mybox)
        {
            await Task.Run(() =>
            {
                ComBoxInfo combox = db.ComBoxInfoes.Where(x => x.lmpdnum == mybox.preboxlmpdnum).FirstOrDefault();
                if (combox != default(ComBoxInfo))
                {
                    BoxInfo boxinfo = Mapper.Map<ComBoxInfo, BoxInfo>(combox);
                    mybox.comboxblock = boxinfo.comboxblock;
                    mybox.comboxhistory = boxinfo.comboxhistory;
                    mybox.comboxID = boxinfo.comboxID;
                    mybox.comboxismatched = boxinfo.comboxismatched;
                    mybox.comboxlmpdnum = boxinfo.comboxlmpdnum;
                    mybox.comboxroad = boxinfo.comboxroad;
                    mybox.comboxrptcomdate = boxinfo.comboxrptcomdate;
                    mybox.comboxrptcomment = boxinfo.comboxrptcomment;
                    mybox.comboxrptlift = boxinfo.comboxrptlift;
                    mybox.comboxrptpostalcode = boxinfo.comboxrptpostalcode;
                    mybox.comboxstatus = boxinfo.comboxstatus;
                    mybox.comboxteamname = boxinfo.comboxteamname;
                    mybox.comboxtechname = boxinfo.comboxtechname;
                    mybox.comrecID = boxinfo.comrecID;
                    CommissionRecord comrec = db.CommissionRecords.Where(x => x.comrecId == mybox.comrecID).FirstOrDefault();
                    if (comrec != default(CommissionRecord))
                    {
                        BoxInfo boxinfo2 = Mapper.Map<CommissionRecord, BoxInfo>(comrec);
                        mybox.comreccomment = boxinfo2.comreccomment;
                        mybox.comrechistory = boxinfo2.comrechistory;
                        mybox.comrecorddate = boxinfo2.comrecorddate;
                        mybox.comrecstatus = boxinfo2.comrecstatus;
                        mybox.comrecsupname = boxinfo2.comrecsupname;
                    }
                }                
            });
            return mybox;
        }
        
        public async Task<BoxInfo> getPreBoxComRec(BoxInfo mybox)
        {
            await Task.Run(() =>
            {
                PreBoxInfo prebox = db.PreBoxInfoes.Where(x => x.lmpdnum == mybox.comboxlmpdnum).FirstOrDefault();
                if (prebox != default(PreBoxInfo))
                {
                    BoxInfo boxinfo = Mapper.Map<PreBoxInfo, BoxInfo>(prebox);
                    mybox.preboxblock = boxinfo.preboxblock;
                    mybox.preboxcheckdate = boxinfo.preboxcheckdate;
                    mybox.preboxcheckername = boxinfo.preboxcheckername;
                    mybox.preboxhistory = boxinfo.preboxhistory;
                    mybox.preboxID = boxinfo.preboxID;
                    mybox.preboxinstalldate = boxinfo.preboxinstalldate;
                    mybox.preboxisdeployed = boxinfo.preboxisdeployed;
                    mybox.preboxismatched = boxinfo.preboxismatched;
                    mybox.preboxjsonid = boxinfo.preboxjsonid;
                    mybox.preboxlift = boxinfo.preboxlift;
                    mybox.preboxlmpdnum = boxinfo.preboxlmpdnum;
                    mybox.preboxpostalcode = boxinfo.preboxpostalcode;
                    mybox.preboxroad = boxinfo.preboxroad;
                    mybox.preboxsimnum = boxinfo.preboxsimnum;
                    mybox.preboxstatus = boxinfo.preboxstatus;
                    mybox.preboxtechname = boxinfo.preboxtechname;
                    mybox.preboxtelco = boxinfo.preboxtelco;
                    CommissionRecord comrec = db.CommissionRecords.Where(x => x.comrecId == mybox.comrecID).FirstOrDefault();
                    if (comrec != default(CommissionRecord))
                    {
                        BoxInfo boxinfo2 = Mapper.Map<CommissionRecord, BoxInfo>(comrec);
                        mybox.comreccomment = boxinfo2.comreccomment;
                        mybox.comrechistory = boxinfo2.comrechistory;
                        mybox.comrecorddate = boxinfo2.comrecorddate;
                        mybox.comrecstatus = boxinfo2.comrecstatus;
                        mybox.comrecsupname = boxinfo2.comrecsupname;
                    }
                }
            });
            return mybox;
        }
        #endregion


        #region  IDisposable implementation

        internal bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            ((IDisposable)db).Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            disposed = false;
            if (!disposed)
            {
                if (disposing)
                {
                    // Clear all property values if any
                }
                // Indicate that the instance has been disposed.
                disposed = true;
            }
        }
        #endregion
    }
}