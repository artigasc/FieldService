using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class Contact
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public string Charge { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Mail { get; set; }
        public bool IsFerreyrosContact { get; set; }
    }
}