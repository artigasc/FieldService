using SQLite.Net.Attributes;

namespace FESA.SCM.Core.Models
{
    [Table("DOCUMENT")]
    public class Document
    {
        [PrimaryKey]
        public string Id { get; set; }
        [Indexed]
        public string AssignmentId { get; set; }

        public Enumerations.DocumentType DocumentType { get; set; }
        public string Name { get; set; }
    }
}