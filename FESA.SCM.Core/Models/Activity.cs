using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("ACTIVITY")]
    public class Activity
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string AssignmentId { get; set; }
        public Enumerations.ActivityType ActivityType { get; set; }
        public Enumerations.ActivityState State { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long TotalTicks { get; private set; }

        [Ignore]
        public TimeSpan Duration
        {
            get { return TimeSpan.FromTicks(TotalTicks); }
            set { TotalTicks = value.Ticks; }
        }

        public bool IsRecording { get; set; }
        [Ignore]
        public List<Trace> Traces { get; set; }
    }
}