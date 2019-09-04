using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.AssignmentBE
{
    public class Contact
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Charge { get; set; }
        public string Phone { get; set; }                
        public string Email { get; set; }
        public bool IsFerreyrosContact { get; set; }
    }
}
