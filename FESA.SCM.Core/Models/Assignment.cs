using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("ASSIGNMENT")]
    public class Assignment
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Enumerations.AssignmentStatus Status { get; set; }
        public Enumerations.AssignmentType AssignmentType { get; set; }
        public string WorkOrderNumber { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public int Priority { get; set; }
        [Ignore]
        public Contact TechnicalContact { get; set; }
        [Ignore]
        public Contact FerreyrosContact { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Ignore]
        public List<Activity> Activities { get; set; }
        [Ignore]
        public List<Document> Documents { get; set; }
        [Ignore]
        public Location Location { get; set; }
        [Ignore]
        public Machine Machine { get; set; }
        public bool IsHistory { get; set; }
    }
}