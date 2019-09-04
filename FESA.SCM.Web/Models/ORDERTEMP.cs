using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class ORDERTEMP
    {
        public string ORDERID { get; set; }
        public string NUMBERORDER { get; set; }
        public string NAME { get; set; }  
        public string SERIALNUMBER { get; set; }
        public string BRAND { get; set; }
        public string MODEL { get; set; }
        public string ASSIGNMENTID { get; set; }
        public int? ASSIGNMENTSTATUS { get; set; }
        public DateTime? ESTIMATEDSTARTDATE { get; set; }
        public DateTime? ESTIMATEDENDDATE { get; set; }
        public int ROW { get; set; }
        public DateTime? CREATIONDATE { get; set; }
        }
}