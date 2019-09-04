namespace FESA.SCM.ServiceGateway.DTO {
	public class Contact {
		public string Id { get; set; }
		public string AssignmentId { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Charge { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsFerreyrosContact { get; set; }
		public string ContactId { get; set; }
		public string CustomerId { get; set; }
		public int AssignmentStatus { get; set; }

	}
}