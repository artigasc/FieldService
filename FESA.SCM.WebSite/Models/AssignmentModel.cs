using FESA.SCM.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class AssignmentModel {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RequestId { get; set; }
        public string CorpId { get; set; }
        public string CiaId { get; set; }
        public AssignmentStatus Status { get; set; }
        public AssignmentType AssignmentType { get; set; }
        public string WorkOrderNumber { get; set; }
        public string WorkOrderId { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public int Priority { get; set; }
        public List<ContactModel> TechnicalContacts { get; set; }
        public List<ContactModel> FerreyrosContacts { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Personnel> AssignedPersonnel { get; set; }
        public List<ActivityModel> Activities { get; set; }
        public List<DocumentModel> Documents { get; set; }
        public Location Location { get; set; }
        public Machine Machine { get; set; }
        public string CostCenter { get; set; }
        public string Office { get; set; }
        public int? Rating { get; set; }

        public int TypeConsult { get; set; }
        public string Supervisor { get; set; }
        public string Brand { get; set; }
        public string Ruc { get; set; }
        public ReportModel Report { get; set; }
    }
}

