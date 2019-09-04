using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.ServiceGateway.DTO
{
    public class CustomerCompany
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string Ruc { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
