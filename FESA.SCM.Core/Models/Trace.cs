using System;
using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("TRACE")]
    public class Trace
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string ActivityId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Enumerations.ActivityState ActivityState { get; set; }
        public DateTime TraceDate { get; set; }
    }
}