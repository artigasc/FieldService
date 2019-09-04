using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FESA.SCM.Common;
using FESA.SCM.Identity.BE.RoleBE;
using FESA.SCM.Identity.BE.UserBE;
using FESA.SCM.Identity.DA.TableTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.Identity.DA {
	public class UserRepository : IUserRepository {

		public IList<User> GetAll() {
			IList<User> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ALL_USERS_SP")) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<User>();
					while (reader.Read()) {
						users.Add(new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Phone = DataConvert.ToString(reader["PHONE"]),
							Photo = DataConvert.ToString(reader["PHOTO"]),
							UserType = (UserType)DataConvert.ToInt32(reader["USERTYPE"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]),
							Role = new Role {
								Id = DataConvert.ToString(reader["ROLEID"]),
								Name = DataConvert.ToString(reader["ROLENAME"])
							}
						});
					}
				}
			}
			return users;
		}

		public IList<User> GetUserByStatus(UserStatus userstatus, string supervisorid, string idsOffice, string idsCostCenter) {
			IList<User> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_USER_BY_STATUS_SP", userstatus, supervisorid, idsOffice, idsCostCenter)) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<User>();
					while (reader.Read()) {
						users.Add(new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"])
						});
					}
				}
			}
			return users;
		}

		public IList<User> GetPaginated(User filters, int pageIndex, int pageSize, out int totalRows) {
			throw new NotImplementedException();
		}

		public IList<User> GetByIdAssignment(string id) {
			IList<User> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_USERS_BY_ASSIGNMENT_SP", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<User>();
					while (reader.Read()) {
						users.Add(new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Phone = DataConvert.ToString(reader["PHONE"]),
							Photo = DataConvert.ToString(reader["PHOTO"]),
							UserType = (UserType)DataConvert.ToInt32(reader["USERTYPE"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]),
							Role = new Role {
								Id = DataConvert.ToString(reader["ROLEID"]),
								Name = DataConvert.ToString(reader["ROLENAME"])
							},
							Celullar = DataConvert.ToString(reader["CELULLAR"]),
							Rpm = DataConvert.ToString(reader["RPM"])
						});
					}
				}
			}
			return users;
		}

		public User GetById(string id) {
			User user = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_USER_BYID_SP", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						user = new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Phone = DataConvert.ToString(reader["PHONE"]),
							Photo = DataConvert.ToString(reader["PHOTO"]),
							UserType = (UserType)DataConvert.ToInt32(reader["USERTYPE"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]),
							Role = new Role {
								Id = DataConvert.ToString(reader["ROLEID"]),
								Name = DataConvert.ToString(reader["NAME"])
							}
						};
					}
				}
			}
			return user;
		}

		public void Add(User entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("INSERT_USER_SP", entity.Id, entity.FesaUserId, entity.Name, entity.Email, entity.Phone,
					entity.UserName, entity.Password, entity.Photo, (int)entity.UserType, (int)entity.UserStatus,
					entity.Role.Id, entity.CreatedBy, entity.CreationDate);
		}

		public void Update(User entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("UPDATE_USER_SP", entity.Id, entity.Phone, entity.Name, entity.Email,
					entity.Photo, (int)entity.UserType, (int)entity.UserStatus);
		}

		public void Delete(string id, string modifiedBy, DateTime lastModification) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_USER_SP", id, modifiedBy, lastModification);
		}

		public void InsertDetails(string userId, int userStatus, DateTime date) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("INSERT_USERDETAILS_SP", userId, userStatus, date);
		}

		public User GetByFesaUserId(string fesaUserId) {
			User user = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_USER_BY_FESAUSERID_SP", fesaUserId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						user = new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Phone = DataConvert.ToString(reader["PHONE"]),
							Photo = DataConvert.ToString(reader["PHOTO"]),
							Pns = DataConvert.ToString(reader["PNS"]),
							UserType = (UserType)DataConvert.ToInt32(reader["USERTYPE"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]),
							Role = new Role {
								Id = DataConvert.ToString(reader["ROLEID"]),
								Name = DataConvert.ToString(reader["NAME"])
							}
						};
					}
				}
			}
			return user;
		}

		public User LoginUser(string userName, string userPassword) {
			User user = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_LOGIN_USER_SP", userName, userPassword)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						user = new User {
							Id = DataConvert.ToString(reader["ID"]),
							Name = DataConvert.ToString(reader["NAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Photo = DataConvert.ToString(reader["PHOTO"]),
							UserType = (UserType)DataConvert.ToInt32(reader["USERTYPE"]),
							UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]),
							Role = new Role {
								Id = DataConvert.ToString(reader["ROLEID"]),
								Name = DataConvert.ToString(reader["ROLENAME"])
							},
							ChangedPassword = DataConvert.ToBool(reader["HASCHANGEDPASSWORD"]),
							SessionActive = DataConvert.ToBool(reader["SESSIONSTARTED"])
						};
					}
				}
			}
			return user;
		}

		public void ChangePassword(string userId, string userPassword) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("UPDATE_PASSWORD_SP", userId, userPassword);
		}

		public void ResetPassword(string userId, string tempPassword) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("RESET_PASSWORD_SP", userId, tempPassword);
		}

		public string GetIdByUserName(string userName) {
			return DataConvert.ToString(DatabaseFactory.CreateDatabase().ExecuteScalar("GET_USERID_BY_USERNAME_SP", userName));
		}


		public void SyncUsers(IList<User> users) {
			var usuarios = new UserTableType();
			usuarios.AddRange(users);
			if (usuarios.Count == 0)
				return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_USERS_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@USERS",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "USER_TYPE"
			});
			database.ExecuteNonQuery(cmd);
		}

		public void SetPns(string userId, string pns) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("SET_USER_PNS_SP", userId, pns);
		}

		public int ChangeSessionStatus(string userId, bool status) {
			return DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("UPDATE_LOGIN_USER_STATE_SP", userId, status);
		}

		public int InsertOffice(string office) {
			int officeid = 0;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("INSERT_OFFICE_SP", office)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						officeid = int.Parse(reader["ID"].ToString());
					}

				}
			}
			return officeid;
		}

		public int InsertCostCenter(string costCenter) {
			int costcenterid = 0;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("INSERT_COSTCENTER_SP", costCenter)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						costcenterid = int.Parse(reader["ID"].ToString());
					}

				}
			}
			return costcenterid;
		}

		public IList<Office> GetAllOffice() {
			IList<Office> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ALL_OFFICE")) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<Office>();
					while (reader.Read()) {
						users.Add(new Office {
							Id = DataConvert.ToInt(reader["ID"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							City = DataConvert.ToString(reader["STRCITY"]),
							StrOffice = DataConvert.ToString(reader["STROFFICE"]),
						});
					}
				}
			}
			return users;
		}

		public IList<CostCenter> GetAllCostCenter() {
			IList<CostCenter> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ALL_COSTCENTER")) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<CostCenter>();
					while (reader.Read()) {
						users.Add(new CostCenter {
							Id = DataConvert.ToInt(reader["ID"]),
							Description = DataConvert.ToString(reader["DESCRIPTIONC"]),
						});
					}
				}
			}
			return users;
		}

		public IList<Brand> GetAllMachineryBrand() {
			IList<Brand> items;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_MACHINERYBRAND_GETALL")) {
				using (var reader = database.ExecuteReader(cmd)) {
					items = new List<Brand>();
					while (reader.Read()) {
						items.Add(new Brand {
							Id = DataConvert.ToString(reader["STRID"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							Text = DataConvert.ToString(reader["STRTEXT"]),
						});
					}
				}
			}
			return items;
		}

	}
}
