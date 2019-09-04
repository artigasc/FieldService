using System;
using System.Collections.Generic;
using System.Linq;
using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.BE.CustomerCompanyBE;
using FESA.SCM.Customer.BL.BusinessInterfaces;

namespace FESA.SCM.Customer.BL.BusinessServices {
    public class CustomerService : ICustomerService {
        #region Members
        private readonly ICustomerCompanyRepository _customerCompanyRepository;
        private readonly IContactRepository _contactRepository;
        #endregion
        #region Constructor
        public CustomerService(ICustomerCompanyRepository customerCompanyRepository, IContactRepository contactRepository) {
            if (customerCompanyRepository == null)
                throw new ArgumentNullException(nameof(customerCompanyRepository));

            _customerCompanyRepository = customerCompanyRepository;

            if (contactRepository == null)
                throw new ArgumentNullException(nameof(contactRepository));

            _contactRepository = contactRepository;
        }

        #endregion
        #region Public Methods

        public IList<CustomerCompany> GetAllCustomerCompanies() {
            return _customerCompanyRepository.GetAll();
        }

        public IList<CustomerCompany> GetCustomersPaginatedList(CustomerCompany customer, int pageIndex, int pageSize, out int totalRows) {
            throw new NotImplementedException();
        }

        public CustomerCompany GetByCustomerId(string id) {
            var customer = _customerCompanyRepository.GetById(id);
            if (customer == null) return null;
            // ANTONIO
            //customer.Contacts = _contactRepository.GetByCustomerId(id).ToList();
            return customer;
        }

        public CustomerCompany GetByRuc(string ruc) {
            var customer = _customerCompanyRepository.GetByRuc(ruc);
            if (customer == null) return null;
            //customer.Contacts = _contactRepository.GetByCustomerId(customer.Id).ToList();
            return customer;
        }

        public string AddCustomer(CustomerCompany customer) {
            customer.Id = Guid.NewGuid().ToString();
            customer.CreationDate = DateTime.Now;
            _customerCompanyRepository.Add(customer);
            customer.Contacts.ForEach(c => {
                c.CustomerId = customer.Id;
                //c.ContactId = customer.ContactId;
                c.Id = Guid.NewGuid().ToString();
                c.CreationDate = DateTime.Now;
            });
            //_contactRepository.InsertContacts(customer.Contacts);
            return customer.Id;
        }

        public void UpdateCustomer(CustomerCompany customer) {
            _customerCompanyRepository.Update(customer);
        }

        public void DeleteCustomer(string id, string modifiedBy, DateTime lastModification) {
            _customerCompanyRepository.Delete(id, modifiedBy, lastModification);
        }

        public string AddContact(Contact contact) {
            contact.Id = Guid.NewGuid().ToString();
            contact.IsSC = true;
            contact.Active = true;
            _contactRepository.InsertContact(contact);
            return contact.Id;
        }

        #endregion
    }
}