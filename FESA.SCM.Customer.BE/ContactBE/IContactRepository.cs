using System.Collections.Generic;
using FESA.SCM.Common.Base;

namespace FESA.SCM.Customer.BE.ContactBE
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        IList<Contact> GetByCustomerId(string customerId);
        void InsertContacts(IList<Contact> contacts);
        void InsertContact(Contact contact);
    }
}