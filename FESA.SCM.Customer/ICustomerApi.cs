using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.BE.CustomerCompanyBE;

namespace FESA.SCM.Customer
{
    [ServiceContract]
    public interface ICustomerApi
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "Customers")]
        IList<CustomerCompany> GetAllCustomerCompanies();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "Customers")]
        IList<CustomerCompany> GetCustomersPaginatedList(CustomerCompany customer, int pageIndex, int pageSize,
            out int totalRows);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "Customer")]
        CustomerCompany GetByCustomerId(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "Customer")]
        CustomerCompany GetCustomerByRuc(string ruc);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AddCustomer(CustomerCompany customer);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void UpdateCustomer(CustomerCompany customer);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void DeleteCustomer(string id, string modifiedBy, DateTime lastModification);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AddContact(Contact contact);
    }
}
