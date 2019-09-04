using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class TechnicalContact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CreationDate { get; set; }
        public string LastModification { get; set; }

    }
}