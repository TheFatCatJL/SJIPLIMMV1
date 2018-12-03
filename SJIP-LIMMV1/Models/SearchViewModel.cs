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
        //white space at both end of the string will be ignored; only accept max 30 letter input;allow space between words
        const string regExrTownCouncil = "^\\s*[a-zA-Z ]{0,30}\\s*$";

        //white space at both end of the string will be ignored;first 14 digits must be numbers; 15th digit must be "-" and remaining 4 digits must be number
        const string regExrSIMCard = "^\\s*[0-9]{0,14}(-){0,1}[0,9]{0,4}\\s*$";

        //white space at both end of the string will be ignored;first two digit must be letters and remainning 10 digits need to be numbers
        const string regExrLMPD = "(^\\s*[a-zA-Z]{0,1}\\s*$)|(^\\s*[a-zA-Z]{2}[0-9]{0,10}\\s*$)";

        //white space at both end of the string will be ignored; first three digits must be number ranging from 1 to 999 and fourth digit must be letter if available,
        //string start with "0" will not be accepted
        const string regExrBlock = "(^\\s*(?!0)[0-9]{0,2}$)|(^\\s*(?!0)[0-9]{3}[a-zA-Z]{0,1}\\s*$)";


        [RegularExpression( regExrTownCouncil,ErrorMessage = "Please only enter letters")]
        public String TownCouncil { get; set; }

        [RegularExpression(regExrSIMCard, ErrorMessage = "Please enter SIM Card number start with 14 numbers followed with a \"-\" and followed with 4 numbers")]
        public String SIMCard { get; set; }

        [RegularExpression(regExrBlock, ErrorMessage = "Please enter Block number between 1 to 999 and may followed with a letter")]
        public string Block { get; set; }

        [RegularExpression(regExrLMPD, ErrorMessage = "Please enter LMPD number start with 2 letters followed with 10 numbers")]
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