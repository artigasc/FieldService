using FESA.SCM.Common.Base;
using System;

namespace FESA.SCM.ServiceGateway.DTO {
	public class Document : Entity {
		public string Id { get; set; }
		public string UserId { get; set; }
		public string AssignmentId { get; set; }
		public string Name { get; set; }
		public string ActivityId { get; set; }
		public int ActivityValue { get; set; }
		public string Text { get; set; }
		public int Position { get; set; }
		public bool Check { get; set; }

		public string DocumentId { get; set; }

		public DateTime Date { get; set; }

	}
}