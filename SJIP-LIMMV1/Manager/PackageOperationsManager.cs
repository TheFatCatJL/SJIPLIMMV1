using SJIP_LIMMV1.Manager.Fluent;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Models.Interfaces;
using SJIP_LIMMV1.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SJIP_LIMMV1.Manager
{
    public class PackageOperationsManager : IDisposable
    {
        // Declare logger - Note standard declaration
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        IViewModel baseModel;
        IEnumerable<IViewModel> baseModels;
        HttpContextBase context;
        public HttpContextBase thisContext { get => context; set => context = value; }
        public static IDictionary<string, object> PackingDetails { get; set; }

        public PackageOperationsManager(HttpContextBase context, Type vmType)
        {
            this.context = context;
            BaseModelIniatiate(vmType);
            BaseModelsInitiate(vmType);
        }

        #region Makes BaseViewModel/ViewModels by dynamic invoking 
        private void BaseModelIniatiate(Type vmtype)
        {
            dynamic newOrderType = Activator.CreateInstance(vmtype , true);
            this.baseModel = newOrderType;
        }

        private void BaseModelsInitiate(Type vmtype)
        {
            dynamic newOrderType = Activator.CreateInstance(vmtype, true);
            IEnumerable<IViewModel> tempmodels = new List<IViewModel>() { newOrderType };
            this.baseModels = tempmodels;
        }

        #endregion

        #region Exposed Methods
        public IViewModel PopulateNewBox()
        {
            IViewModel viewModel = PopulateFields[this.baseModel.GetType()].Invoke("New",baseModel);
            return viewModel;
        }

        public IViewModel PopulateSearchBox(IViewModel vm)
        {
            IViewModel viewModel = PopulateFields[this.baseModel.GetType()].Invoke("Search", vm);
            return viewModel;
        }

        public IViewModel PopulatePostBox(IViewModel vm)
        {
            IViewModel viewModel = PopulateFields[this.baseModel.GetType()].Invoke("Post", vm);
            return viewModel;
        }

        public void PopulatePostBoxNoReturn(IViewModel vm)
        {
            IViewModel viewModel = PopulateFields[this.baseModel.GetType()].Invoke("PostNoReturn", vm);
        }

        public void SetDetails(IDictionary<string,object> dictionary)
        {
            PackingDetails = dictionary;
        }
        #endregion

        #region Field populating operations
        // Makes ViewModel
        private static IDictionary<Type, Func<string, IViewModel, IViewModel>> PopulateFields = new Dictionary<Type, Func<string, IViewModel, IViewModel>>
        {
            {
                typeof(PreBoxInfoViewModel), (instruction, model) =>
                {
                    PreBoxInfoViewModel pbox = (PreBoxInfoViewModel) model;
                    if(instruction == "New")
                    {
                        pbox.status= "Pre-Commission";
                        return pbox;
                    }
                    else if(instruction == "Search")
                    {
                        if(pbox.checkdate == null)
                            pbox.checkdate = default(DateTime);
                        if(pbox.checkername == null)
                            pbox.checkername = "";
                        if(pbox.history == null)
                            pbox.history = "";
                        if(pbox.installdate == null)
                            pbox.installdate = default(DateTime);
                        if(pbox.jsonid == null)
                            pbox.jsonid = "";
                        if(pbox.lift == null)
                            pbox.lift = "";
                        if(pbox.lmpdnum == "")
                            pbox.lmpdnum = "";
                        if(pbox.postalcode == null)
                            pbox.postalcode = "";
                        if(pbox.simnum == null)
                            pbox.simnum = "";
                        if(pbox.status == null)
                            pbox.status = "";
                        if(pbox.techname == null)
                            pbox.techname = "";
                        if(pbox.telco == null)
                            pbox.telco = "";
                        return pbox;
                    }
                    else if (instruction=="Post")
                    {
                        pbox.checkername = (string) PackingDetails["checkername"];
                        pbox.checkdate = DateTime.Now;
                        pbox.history = string.Format("Pre-Commission done by {0} on {1}.", pbox.checkername, pbox.checkdate);
                        return pbox;
                    }
                    else
                    {
                        return null;
                    }
                }
            },
            {
                typeof(ComBoxInfoViewModel), (instruction, model) =>
                {
                    ComBoxInfoViewModel cbox = (ComBoxInfoViewModel) model;
                    if(instruction == "New")
                    {
                        cbox.status= "Pre-Submission";
                        return cbox;
                    }
                    else if(instruction == "Search")
                    {
                        if (cbox.history == null)
                            cbox.history = "";
                        if (cbox.lmpdnum == null)
                            cbox.lmpdnum = "";
                        if (cbox.rptcomdate == null)
                            cbox.rptcomdate = default(DateTime);
                        if (cbox.rptcomment == null)
                            cbox.rptcomment = "";
                        if (cbox.rptlift == null)
                            cbox.rptlift = "";
                        if (cbox.rptpostalcode == null)
                            cbox.rptpostalcode = "";
                        if (cbox.status == null)
                            cbox.status = "";
                        if (cbox.teamname == null)
                            cbox.teamname = "";
                        if (cbox.techname == null)
                            cbox.techname = "";
                        return cbox;
                    }
                    else if (instruction=="Post")
                    {
                        string supname = (string) PackingDetails["supname"];
                        cbox.status = "Commissioned";
                        cbox.rptcomdate = DateTime.Now;
                        cbox.history = string.Format("Submission done by {0} on {1}.", supname, cbox.rptcomdate);
                        return cbox;
                    }
                    else
                    {
                        return null;
                    }
                }
            },
            {
                typeof(ContactFormViewModel), (instruction, model) =>
                {
                    ContactFormViewModel cForm = (ContactFormViewModel) model;
                    if(instruction == "New")
                    {
                        string username = (string) PackingDetails["username"];
                        string email = (string) PackingDetails["email"];
                        cForm.Name = username;
                        cForm.Email = email;
                        return cForm;
                    }
                    if(instruction == "Search")
                    {
                        // Future implementations
                        return cForm;
                    }
                    else if (instruction=="Post")
                    {
                        // Future implementations
                        return cForm;
                    }
                    else
                    {
                        return null;
                    }
                }
            },
            {
                typeof(CommissionRecordVM), (instruction, model) =>
                {
                    if(instruction == "New")
                    {
                        CommissionRecordVM record = model == null ? new CommissionRecordVM() : (CommissionRecordVM)model;
                        record.status = "Report Pending";
                        record.supname = PackingDetails["supname"].ToString();
                        record.comrecorddate = DateTime.Now;
                        return record;
                    }
                    else if(instruction == "Search")
                    {
                        CommissionRecordVM record = (CommissionRecordVM)model;
                        if(record.comment == null)
                            record.comment = "";
                        if(record.comrecorddate == null)
                            record.comrecorddate = default(DateTime);
                        if(record.history == null)
                            record.history = "";
                        if(record.status == null)
                            record.status = "";
                        if(record.supname == null)
                            record.supname = "";
                        return record;
                    }
                    else if (instruction=="Post")
                    {
                        CommissionRecordVM record = model == null ? new CommissionRecordVM() : (CommissionRecordVM)model;
                        record.status = "Reported";
                        record.comrecorddate = DateTime.Now;
                        record.history = string.Format("Commission Report submitted by {0} on {1}.", record.supname, record.comrecorddate);
                        return record;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        };

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        #endregion


    }
}