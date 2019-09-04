using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("LOCATION")]
    public class Location
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string AssignmentId { get; set; }
        public string Province { get; set; }
        public string Department { get; set; }
        public string District { get; set; }
    }
}