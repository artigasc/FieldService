using System;

namespace FESA.SCM.FieldService.BE.ActivityBE
{
    public class Trace
    {
        public string Id { get; set; }
        public string ActivityId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ActivityState ActivityState { get; set; }
        public DateTime TraceDate { get; set; }

    }
}