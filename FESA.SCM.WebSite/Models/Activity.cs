using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class ActivityModel {


        public string Group { get; set; }
        public string Name { get; set; }

        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string UserId { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityState ActivityState { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        //public TimeSpan Duration { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan DurationEnd {
            get {
                return new TimeSpan(0);
            }  //TimeSpan.Parse(Duration); }
        }

        public List<TraceModel> Traces { get; set; }


        public bool Day { get; set; }
        public bool Online { get; set; }
        //new
        public string InitialLocation { get; set; }
        public string FinalLocation { get; set; }
    }
}
