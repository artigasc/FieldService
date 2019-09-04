using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using FESA.SCM.Common;
using FESA.SCM.Identity.BE.UserBE;
using FESA.SCM.Identity.BL.BusinessInterfaces;
using Microsoft.CSharp;
using System.Linq;
using FESA.SCM.Identity.BE;

namespace FESA.SCM.Identity.BL.BusinessIServices {
	public class UserService : IUserService {
		#region Members
		private readonly IUserRepository _userRepository;
		#endregion
		#region Constructor
		public UserService(IUserRepository userRepository) {
			if (userRepository == null)
				throw new ArgumentNullException(nameof(userRepository));

			_userRepository = userRepository;
		}
		#endregion
		#region Methods
		public IList<User> GetAll() {
			return _userRepository.GetAll();
		}

		public IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter) {
			return _userRepository.GetUserByStatus(userstatus, supervisorid, idsOffice, idsCostCenter);
		}

		public IList<User> GetPaginated(User user, int pageIndex, int pageSize, out int totalRows) {
			throw new System.NotImplementedException();
		}

		public IList<User> GetUsersByIdAssignment(string id) {
			var personnel = _userRepository.GetByIdAssignment(id);
			return personnel;
		}

		public User GetUserById(string id) {
			return _userRepository.GetById(id);
		}

		public User GetUserByFesaUserId(string fesaUserId) {
			return _userRepository.GetByFesaUserId(fesaUserId);
		}

		public void AddUser(User user) {
			user.Id = Guid.NewGuid().ToString();
			user.Password = EncryptPassword(user.Password);
			_userRepository.Add(user);
		}

		public void UpdateUser(User user) {
			_userRepository.Update(user);
		}

		public void DeleteUser(string id, string modifiedBy, DateTime lastModification) {
			_userRepository.Delete(id, modifiedBy, lastModification);
		}

		public User LoginUser(string userName, string userPassword) {
			userPassword = EncryptPassword(userPassword);
			var user = _userRepository.LoginUser(userName, userPassword);
			if (!user.SessionActive) {
				_userRepository.ChangeSessionStatus(user.Id, true);
			}
			return user;
		}

		public void ChangePassword(string userId, string userPassword) {
			userPassword = EncryptPassword(userPassword);
			_userRepository.ChangePassword(userId, userPassword);
			_userRepository.ChangeSessionStatus(userId, status: true);
		}

		public User ResetPassword(string userName) {
			var userId = _userRepository.GetIdByUserName(userName);
			if (string.IsNullOrEmpty(userId))
				return null;
			var tempPassword = GeneratePassword();
			var _password = tempPassword;
			tempPassword = EncryptPassword(tempPassword);
			_userRepository.ResetPassword(userId, tempPassword);
			return new User() { UserName = userName, Password = _password };
		}

		public void SyncUsers(IList<User> users) {
			foreach (var user in users) {
				user.Id = Guid.NewGuid().ToString();
				//user.Password = EncryptPassword(user.Password);
				user.OfficeId = _userRepository.InsertOffice(user.Office);
				user.CostCenterId = _userRepository.InsertCostCenter(user.CostCenter);
			}
			_userRepository.SyncUsers(users);
		}

		public void SetUserPns(string userId, string pns) {
			_userRepository.SetPns(userId, pns);
		}

		public int LogOff(string userId) {
			return _userRepository.ChangeSessionStatus(userId, status: false);
		}


		public IList<Office> GetAllOffice() {
			return _userRepository.GetAllOffice();
		}

		public IList<CostCenter> GetAllCostCenter() {
			return _userRepository.GetAllCostCenter();
		}

		//NEW 
		public ResponseFilter GetAllFilters() {
			var result = new ResponseFilter();

			var offices = _userRepository.GetAllOffice();
			var brands = _userRepository.GetAllMachineryBrand();
			var costsCenter = _userRepository.GetAllCostCenter();

			if (offices != null) {
				result.Offices = new List<Office>(offices);
			}
			if (brands != null) {
				result.Brands = new List<Brand>(brands);
			}
			if (costsCenter != null) {
				result.CostsCenter = new List<CostCenter>(costsCenter);
			}

			return result;
		}

		#endregion
		#region Private Methods
		private static string EncryptPassword(string password) {
			var clearBytes = Encoding.Unicode.GetBytes(password);
			using (var encryptor = Aes.Create()) {
				var pdb = new Rfc2898DeriveBytes(Constants.EncryptionKey,
					new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				if (encryptor == null)
					return password;
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (var ms = new MemoryStream()) {
					using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					password = Convert.ToBase64String(ms.ToArray());
				}
			}
			return password;
		}

		private static string GeneratePassword() {
			var rdIndex = new Random();
			const int passwordLength = 8;
			const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789@$";
			var chars = new char[passwordLength];
			for (var i = 0; i < passwordLength; i++) {
				chars[i] = allowedChars[rdIndex.Next(0, allowedChars.Length)];
			}
			return new string(chars);
		}

		public void InsertDetails(string userId, int userStatus, DateTime date) {
			_userRepository.InsertDetails(userId, userStatus, date);
		}


		#endregion
	}
}