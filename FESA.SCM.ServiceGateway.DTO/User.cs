namespace FESA.SCM.ServiceGateway.DTO
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public bool ChangedPassword { get; set; }
        public bool SessionActive { get; set; }
        public UserType UserType { get; set; }
        public UserStatus UserStatus { get; set; } 
        public int? Office { get; set; }
        public int? CostCenter { get; set; }
    }
}