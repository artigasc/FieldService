using FESA.SCM.Common.Base;

namespace FESA.SCM.Customer.BE.CustomerCompanyBE
{
    public interface ICustomerCompanyRepository : IBaseRepository<CustomerCompany>
    {
        CustomerCompany GetByRuc(string ruc);
    }
}