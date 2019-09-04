using System;

namespace FESA.SCM.ServiceGateway.DTO
{
    public class Trace
    {
        public string Id { get; set; }
        public string ActivityId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ActivityState ActivityState { get; set; }
        public DateTime TraceDate { get; set; }
    }
}