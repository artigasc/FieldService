using System;

namespace FESA.SCM.Common.Base
{
    public class Entity
    {
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime LastModification { get; set; }
        public bool Active { get; set; }
    }
}
