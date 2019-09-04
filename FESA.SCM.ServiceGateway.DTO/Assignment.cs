using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.ServiceGateway.DTO {
	public class Assignment : Entity {
		public string Id { get; set; }
		//public string AssignmentId { get; set; }
		public string UserId { get; set; }
		public AssignmentStatus Status { get; set; }
		public AssignmentType AssignmentType { get; set; }
		public string WorkOrderNumber { get; set; }
		public string WorkOrderId { get; set; }

		public string Description { get; set; }
		public string CompanyName { get; set; }
		public int Priority { get; set; }
		public List<Contact> TechnicalContacts { get; set; }
		public List<Contact> FerreyrosContacts { get; set; }
		public DateTime RequestDate { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime EstimatedEndDate { get; set; }
		public DateTime EstimatedStartDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<Activity> Activities { get; set; }
		public List<Document> Documents { get; set; }
		public Location Location { get; set; }
		public Machine Machine { get; set; }

		public string Office { get; set; }
		public string CostCenter { get; set; }
		public bool Active {
			get; set;
		}
		public string Rating { get; set; }
		public int TypeConsult { get; set; }


		public Report Report { get; set; }
		public string RUC { get; set; }
		public string NumberPlate { get; set; }

		public List<Report> Reports { get; set; }

	}
}
