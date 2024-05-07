using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class MovieVM
    {

        public string name { set; get; }
        public HttpPostedFileBase imageMovie { set; get; }

        public string introduce{set;get;}

        public string trailer { set; get; }
        
        public int idCountry { set; get; }
        public DateTime showDate { set; get; }
        public int time { set; get; }
        public string yearManufacture { set; get; }
        public string company { set; get; }



    }
}