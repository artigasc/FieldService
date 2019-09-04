using System.Collections.Generic;
using FESA.SCM.Common.Base;
using System;

namespace FESA.SCM.Identity.BE.UserBE {
	public interface IUserRepository : IBaseRepository<User> {
		User GetByFesaUserId(string fesaUserId);
		User LoginUser(string userName, string userPassword);
		void ChangePassword(string userId, string userPassword);
		void ResetPassword(string userId, string tempPassword);
		string GetIdByUserName(string userName);
		void SyncUsers(IList<User> users);
		void InsertDetails(string userId, int userStatus, DateTime date);
		void SetPns(string userId, string pns);
		int ChangeSessionStatus(string userId, bool status);
		IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter);
		IList<User> GetByIdAssignment(string id);
		int InsertOffice(string office);
		int InsertCostCenter(string costCenter);
		//IList<User> GetListById(string id);

		IList<Office> GetAllOffice();
		IList<CostCenter> GetAllCostCenter();

		//NEW WEB
		IList<Brand> GetAllMachineryBrand();

	}
}