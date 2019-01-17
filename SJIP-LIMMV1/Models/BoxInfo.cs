using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SJIP_LIMMV1.Models
{
    public class BoxInfo
    {
        [Key]
        [Display(Name = "Id Key")]
        public int id { get; set; }
        [Display(Name = "Additional Notes")]
        public string additionalnote { get; set; }
        [Display(Name = "Town Council")]
        public string towncouncil { get; set; }

        public string searchstring { get; set; }
        public string addressstring { get; set; }
        public string simcardstring { get; set; }
        public string lmpdstring { get; set; }

        [Display(Name = "PreBox ID")]
        public int preboxID { get; set; }
        [Display(Name = "LMPD #")]
        public string preboxlmpdnum { get; set; }
        [Display(Name = "Simcard #")]
        public string preboxsimnum { get; set; }
        [Display(Name = "Postal Code")]
        public string preboxpostalcode { get; set; }
        [Display(Name = "Block")]
        public string preboxblock { get; set; }
        [Display(Name = "Road Name")]
        public string preboxroad { get; set; }
        [Display(Name = "Telco (Simcard)")]
        public string preboxtelco { get; set; }
        [Display(Name = "JSON File number")]
        public string preboxjsonid { get; set; }
        [Display(Name = "Lift")]
        public string preboxlift{ get; set; }
        [Display(Name = "Checker Name")]
        public string preboxcheckername { get; set; }
        [Display(Name = "Technician Name")]
        public string preboxtechname { get; set; }
        [Display(Name = "Date Checked")]
        public DateTime? preboxcheckdate { get; set; }
        [Display(Name = "Date Installed")]
        public DateTime? preboxinstalldate { get; set; }
        [Display(Name = "Status")]
        public string preboxstatus { get; set; }
        [Display(Name = "History")]
        public string preboxhistory { get; set; }
        [Display(Name = "Deployment Flag")]
        public bool preboxisdeployed { get; set; }
        [Display(Name = "Matched Flag")]
        public bool preboxismatched { get; set; }

        [Display(Name = "ComBox ID")]
        public int comboxID { get; set; }
        [Display(Name = "LMPD #")]
        public string comboxlmpdnum { get; set; }
        [Display(Name = "Postal Code")]
        public string comboxrptpostalcode { get; set; }
        [Display(Name = "Block")]
        public string comboxblock { get; set; }
        [Display(Name = "Road Name")]
        public string comboxroad { get; set; }
        [Display(Name = "Lift")]
        public string comboxrptlift { get; set; }
        [Display(Name = "Date Commissioned")]
        public DateTime? comboxrptcomdate { get; set; }
        [Display(Name = "Comments")]
        public string comboxrptcomment { get; set; }
        [Display(Name = "Team Name")]
        public string comboxteamname { get; set; }
        [Display(Name = "Technician Name")]
        public string comboxtechname { get; set; }
        [Display(Name = "Status")]
        public string comboxstatus { get; set; }
        [Display(Name = "History")]
        public string comboxhistory { get; set; }
        [Display(Name = "Matched Flag")]
        public bool comboxismatched { get; set; }

        [Display(Name = "Commission Record")]
        public int comrecID { get; set; }
        [Display(Name = "Commission Report Date")]
        public DateTime comrecorddate { get; set; }
        [Display(Name = "Comments")]
        public string comreccomment { get; set; }
        [Display(Name = "History")]
        public string comrechistory { get; set; }
        [Display(Name = "Status")]
        public string comrecstatus { get; set; }
        [Display(Name = "Supervisor Name")]
        public string comrecsupname { get; set; }
    }

    public class BoxInfoRecord : IEnumerable
    {
        // IEnumerable implementtion
        private BoxInfo[] _boxinfos;
        public BoxInfoRecord (BoxInfo[] boxes)
        {
            _boxinfos = new BoxInfo[boxes.Length];
            for (int i = 0; i < boxes.Length; i++)
            {
                _boxinfos[i] = boxes[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public BoxEnumerator GetEnumerator()
        {
            return new BoxEnumerator(_boxinfos);
        }

    }

    public class BoxEnumerator : IEnumerator
    {
        public BoxInfo[] _boxInfos;
        int position = -1;

        public BoxEnumerator(BoxInfo[] list)
        {
            _boxInfos = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _boxInfos.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public BoxInfo Current
        {
            get
            {
                try
                {
                    return _boxInfos[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}