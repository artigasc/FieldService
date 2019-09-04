using System.Collections.Generic;
using FESA.SCM.Common.Base;
using FESA.SCM.Customer.BE.ContactBE;

namespace FESA.SCM.Customer.BE.CustomerCompanyBE
{
    public class CustomerCompany : Entity
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string Ruc { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}