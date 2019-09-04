using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using FESA.SCM.Identity.BE;
using FESA.SCM.Identity.BE.RoleBE;
using FESA.SCM.Identity.BE.UserBE;

namespace FESA.SCM.Identity {
	[ServiceContract]
	public interface IIdentityApi {

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Users")]
		IList<User> GetAllUsers();

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Users")]
		IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Users")]
		IList<User> GetUsersPaginated(User user, int pageIndex, int pageSize, out int totalRows);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[return: MessageParameter(Name = "Users")]
		IList<User> GetUsersByIdAssignment(string id);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "User")]
		User GetUserById(string id);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "User")]
		User GetByFesaUserId(string fesaUserId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void AddUser(User user);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void UpdateUser(User user);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			 BodyStyle = WebMessageBodyStyle.Wrapped)]
		void SetUserPns(string userId, string pns);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			 BodyStyle = WebMessageBodyStyle.Wrapped)]
		int LogOff(string userId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			 BodyStyle = WebMessageBodyStyle.Wrapped)]
		void InsertDetails(string userId, int userStatus, string date);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void DeleteUser(string id, string modifiedBy, DateTime lastModification);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		User LoginUser(string userName, string userPassword);
		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void ChangeUserPassword(string userId, string userPassword);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		User ResetPassword(string userName);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void SyncUsers(IList<User> users);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Roles")]
		IList<Role> GetAllRoles();

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Roles")]
		IList<Role> GetRolesPaginated(Role role, int pageIndex, int pageSize, out int totalRows);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Role")]
		Role GetRoleById(string id);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void AddRole(Role role);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void UpdateRole(Role role);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void DeleteRole(string id, string modifiedBy, DateTime lastModification);


		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
		   BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[return: MessageParameter(Name = "Roles")]
		IList<Office> GetAllOffice();

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
		   BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[return: MessageParameter(Name = "Roles")]
		IList<CostCenter> GetAllCostCenter();


		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
		   BodyStyle = WebMessageBodyStyle.Wrapped)]
		ResponseFilter GetAllFilters();

	}
}
