using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class Personnel
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public PersonnelType PersonnelType { get; set; }
        //New
        public string Status { get; set; }
    }
}
