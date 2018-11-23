using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SJIP_LIMMV1.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Please enter the name of town council")]
        public String TownCouncil { get; set; }
        public String SIMCard { get; set; }

        [Range(0, 999, ErrorMessage = "Block number must be between 1 to 999")]
        public int? Block { get; set; }
        public String LMPD { get; set; }

        public List<SensorBoxInfo> SensorBoxInfoResults { get; set; }

        public SearchViewModel()
        {
            TownCouncil = null;
            SIMCard = null;
            Block = null;
            LMPD = null;
            SensorBoxInfoResults = new List<SensorBoxInfo>();
        }


    }
}