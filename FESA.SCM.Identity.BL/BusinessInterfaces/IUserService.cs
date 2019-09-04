using System;
using System.Collections.Generic;
using FESA.SCM.Identity.BE;
using FESA.SCM.Identity.BE.UserBE;

namespace FESA.SCM.Identity.BL.BusinessInterfaces {
	public interface IUserService {
		IList<User> GetAll();
		IList<User> GetPaginated(User user, int pageIndex, int pageSize, out int totalRows);
		User GetUserById(string id);
		User GetUserByFesaUserId(string fesaUserId);
		void AddUser(User user);
		void UpdateUser(User user);
		void DeleteUser(string id, string modifiedBy, DateTime lastModification);
		User LoginUser(string userName, string userPassword);
		void ChangePassword(string userId, string userPassword);
		User ResetPassword(string userName);
		void SyncUsers(IList<User> users);
		void SetUserPns(string userId, string pns);
		int LogOff(string userId);
		void InsertDetails(string userId, int userStatus, DateTime date);
		IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter);
		IList<User> GetUsersByIdAssignment(string id);

		IList<Office> GetAllOffice();
		IList<CostCenter> GetAllCostCenter();

		//NEW 
		ResponseFilter GetAllFilters();

	}
}