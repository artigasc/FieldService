using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("USER")]
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public bool ChangedPassword { get; set; }
        public bool SessionActive { get; set; }
        public Enumerations.UserType UserType { get; set; }
        public Enumerations.UserStatus UserStatus { get; set; }
    }
}