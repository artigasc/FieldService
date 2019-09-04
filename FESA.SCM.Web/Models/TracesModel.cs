using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FESA.SCM.Web.Models.Enumerations;

namespace FESA.SCM.Web.Models {
    public class TracesModel {
        public string Id { get; set; }
        public string ActivityId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ActivityState ActivityState { get; set; }
        public DateTime TraceDate { get; set; }
    }
}