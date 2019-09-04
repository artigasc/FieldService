using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FESA.SCM.Web.Models.Enumerations;

namespace FESA.SCM.Web.Models
{
    public class Activity
    {
        public string Id { get; set; }
        public string AssignemntId { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityState ActivityState { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
    }
}