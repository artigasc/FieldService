using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class Order
    {
        public string ORDERID { get; set; }
        public string NUMBERORDER { get; set; }
        public string NAME { get; set; }
        public string SERIALNUMBER { get; set; }
        public string BRAND { get; set; }
        public string MODEL { get; set; }
        public int ASSIGNMENTID { get; set; }
        public string ASSIGNMENTSTATUS { get; set; }
        public string ESTIMATEDSTARTDATE { get; set; }
        public string ESTIMATEDENDDATE { get; set; }
        public string PERSONNELID { get; set; }
        public string NAMEPERSONNEL { get; set; }
        public int USERTYPE { get; set; }

   }
}