using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("CONTACT")]
    public class Contact
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public string Charge { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Mail { get; set; }
        public bool IsFerreyrosContact { get; set; }
    }
}