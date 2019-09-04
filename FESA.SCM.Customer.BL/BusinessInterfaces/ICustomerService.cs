using System;
using System.Collections.Generic;
using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.BE.CustomerCompanyBE;

namespace FESA.SCM.Customer.BL.BusinessInterfaces
{
    public interface ICustomerService
    {
        IList<CustomerCompany> GetAllCustomerCompanies(); 
        IList<CustomerCompany> GetCustomersPaginatedList(CustomerCompany customer, int pageIndex, int pageSize,
            out int totalRows);
        CustomerCompany GetByCustomerId(string id);
        CustomerCompany GetByRuc(string ruc);
        string AddCustomer(CustomerCompany customer);
        void UpdateCustomer(CustomerCompany customer);
        void DeleteCustomer(string id, string modifiedBy, DateTime lastModification);
        string AddContact(Contact contact);

    }
}