using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class TraceModel
    {
        public string Id { get; set; }
        public string ActivityId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ActivityState ActivityState { get; set; }
        public DateTime TraceDate { get; set; }
    }
}
