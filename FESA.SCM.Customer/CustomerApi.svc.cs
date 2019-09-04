using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using FESA.SCM.Common;
using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.BE.CustomerCompanyBE;
using FESA.SCM.Customer.BL.BusinessInterfaces;
using FESA.SCM.Customer.InstanceProviders;

namespace FESA.SCM.Customer
{
    [UnityInstanceProviderBehaviour]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CustomerApi : ICustomerApi
    {
        #region Members
        private readonly ICustomerService _customerService;
        #endregion
        #region Constructor
        public CustomerApi(ICustomerService customerService)
        {
            if (customerService == null)
                throw new ArgumentNullException(nameof(customerService));

            _customerService = customerService;
        }
        #endregion
        #region Methods

        public IList<CustomerCompany> GetAllCustomerCompanies()
        {
            try
            {
                return _customerService.GetAllCustomerCompanies();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public IList<CustomerCompany> GetCustomersPaginatedList(CustomerCompany customer, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _customerService.GetCustomersPaginatedList(customer, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public CustomerCompany GetByCustomerId(string id)
        {
            try
            {
                return _customerService.GetByCustomerId(id);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public CustomerCompany GetCustomerByRuc(string ruc)
        {
            try
            {
                return _customerService.GetByRuc(ruc);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public string AddCustomer(CustomerCompany customer)
        {
            try
            {
                return _customerService.AddCustomer(customer);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public void UpdateCustomer(CustomerCompany customer)
        {
            try
            {
                _customerService.UpdateCustomer(customer);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public void DeleteCustomer(string id, string modifiedBy, DateTime lastModification)
        {
            try
            {
                _customerService.DeleteCustomer(id, modifiedBy, lastModification);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                throw;
            }
        }

        public string AddContact(Contact contact) {
            try {
                return _customerService.AddContact(contact);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        #endregion
    }
}
