using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace SJIP_LIMMV1.Models
{
    public class SearchViewModel
    {
        [RegularExpression("^\\s*[a-zA-Z]{0,50}\\s*$", ErrorMessage = "Please only enter letters")]
        public String TownCouncil { get; set; }

        [RegularExpression("^\\s*[0-9]{0,14}(-){0,1}[0,9]{0,4}\\s*$", ErrorMessage = "Please enter SIM Card number start with 14 numbers followed with a \"-\" and followed with 4 numbers")]
        public String SIMCard { get; set; }

        [Range(1, 999, ErrorMessage = "Block number must be number and between 1 to 999")]
        public int? Block { get; set; }

        [RegularExpression("^\\s*[a-zA-Z]{0,2}[0-9]{0,10}\\s*$", ErrorMessage = "Please enter LMPD number start with 2 letters followed with 10 numbers")]
        public String LMPD { get; set; }

        public int defaultPageNumber { get; set; }

        public int defaultPageSize { get; set; }

        public List<SensorBoxInfo> SensorBoxInfoRecordList { get; set; }
        public PagedList<SensorBoxInfo> PagedSensorBoxInfo { get; set; }


        public SearchViewModel()
        {
            defaultPageNumber = 1;
            defaultPageSize = 4;
            TownCouncil = null;
            SIMCard = null;
            Block = null;
            LMPD = null;
            int pageNumber;
            pageNumber = 1;
            SensorBoxInfoRecordList = new List<SensorBoxInfo>();
            PagedSensorBoxInfo = new PagedList<SensorBoxInfo>(SensorBoxInfoRecordList, pageNumber, 2);
        }
      
    }
}