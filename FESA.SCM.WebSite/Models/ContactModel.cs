using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class ContactModel {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Charge { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsFerreyrosContact { get; set; }
        public string IdCustomer { get; set; }
        public string CustomerId { get; set; }
        public ContactStatus assignmentStatus { get; set; }

    }
}
