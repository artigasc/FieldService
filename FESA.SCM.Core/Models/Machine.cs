using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("MACHINE")]
    public class Machine
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string AssignmentId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int TotalHoursFunction { get; set; }
    }
}