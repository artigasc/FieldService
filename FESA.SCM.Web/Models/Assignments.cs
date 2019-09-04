using System;
using System.Collections.Generic;


namespace FESA.SCM.Web.Models
{
    public class Assignments
    {
        public string Id { get; set; }
        public AssignmentStatus Status { get; set; }
        public string StatusDescription { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public string WorkOrderNumber { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public int Priority { get; set; }
        public Contact TechnicalContact { get; set; }
        public Contact FerreyrosContact { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Documents> Documents { get; set; }
        public Location Location { get; set; }
        public Machine Machine { get; set; }
    }
}