using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using FESA.SCM.Common;
using FESA.SCM.Identity.BE;
using FESA.SCM.Identity.BE.RoleBE;
using FESA.SCM.Identity.BE.UserBE;
using FESA.SCM.Identity.BL.BusinessInterfaces;
using FESA.SCM.Identity.InstanceProviders;

namespace FESA.SCM.Identity {
	[UnityInstanceProviderBehaviour]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class IdentityApi : IIdentityApi {
		#region Members
		private readonly IUserService _userService;
		private readonly IRoleService _roleService;
		#endregion
		#region Constructor
		public IdentityApi(IUserService userService, IRoleService roleService) {
			if (userService == null)
				throw new ArgumentNullException(nameof(userService));

			_userService = userService;

			if (roleService == null)
				throw new ArgumentNullException(nameof(roleService));

			_roleService = roleService;
		}
		#endregion
		#region Methods
		public IList<User> GetAllUsers() {
			try {
				return _userService.GetAll();
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter) {
			try {
				return _userService.GetUserByStatus(userstatus, supervisorid, idsOffice, idsCostCenter);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<User> GetUsersPaginated(User user, int pageIndex, int pageSize, out int totalRows) {
			try {
				return _userService.GetPaginated(user, pageIndex, pageSize, out totalRows);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<User> GetUsersByIdAssignment(string id) {
			try {

				return _userService.GetUsersByIdAssignment(id);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public User GetUserById(string id) {
			try {
				return _userService.GetUserById(id);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public User GetByFesaUserId(string fesaUserId) {
			try {
				return _userService.GetUserByFesaUserId(fesaUserId);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void AddUser(User user) {
			try {
				_userService.AddUser(user);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void UpdateUser(User user) {
			try {
				_userService.UpdateUser(user);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void SetUserPns(string userId, string pns) {
			try {
				_userService.SetUserPns(userId, pns);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public int LogOff(string userId) {
			try {
				return _userService.LogOff(userId);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void InsertDetails(string userId, int userStatus, string date) {
			try {
				DateTime dateTime = DateTime.Parse(date);
				_userService.InsertDetails(userId, userStatus, dateTime);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void DeleteUser(string id, string modifiedBy, DateTime lastModification) {
			try {
				_userService.DeleteUser(id, modifiedBy, lastModification);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public User LoginUser(string userName, string userPassword) {
			try {
				return _userService.LoginUser(userName, userPassword);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void ChangeUserPassword(string userId, string userPassword) {
			try {
				_userService.ChangePassword(userId, userPassword);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public User ResetPassword(string userName) {
			try {
				return _userService.ResetPassword(userName);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void SyncUsers(IList<User> users) {
			try {
				_userService.SyncUsers(users);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<Role> GetAllRoles() {
			try {
				return _roleService.GetAll();
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<Role> GetRolesPaginated(Role role, int pageIndex, int pageSize, out int totalRows) {
			try {
				return _roleService.GetPaginated(role, pageIndex, pageSize, out totalRows);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public Role GetRoleById(string id) {
			try {
				return _roleService.GetRoleById(id);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}
		public void AddRole(Role role) {
			try {
				_roleService.AddRole(role);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}
		public void UpdateRole(Role role) {
			try {
				_roleService.UpdateRole(role);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void DeleteRole(string id, string modifiedBy, DateTime lastModification) {
			try {
				_roleService.DeleteRole(id, modifiedBy, lastModification);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<Office> GetAllOffice() {
			try {
				return _userService.GetAllOffice();
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<CostCenter> GetAllCostCenter() {
			try {
				return _userService.GetAllCostCenter();
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		// NEW 
		public ResponseFilter GetAllFilters() {
			try {
				return _userService.GetAllFilters();
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}



		#endregion
	}
}
