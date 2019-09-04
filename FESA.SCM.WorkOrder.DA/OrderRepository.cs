using System;
using System.Collections.Generic;
using FESA.SCM.Common;
using FESA.SCM.WorkOrder.BE.MachineryBE;
using FESA.SCM.WorkOrder.BE.OrderBE;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace FESA.SCM.WorkOrder.DA {
	public class OrderRepository : IOrderRepository {
		public IList<Order> GetAll() {
			IList<Order> orders;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ALL_ORDERS_SP")) {
				using (var reader = database.ExecuteReader(cmd)) {
					orders = new List<Order>();
					while (reader.Read()) {
						orders.Add(new Order {
							Id = DataConvert.ToString(reader["ID"]),
							Code = DataConvert.ToString(reader["CODE"]),
							CustomerId = DataConvert.ToString(reader["CUSTOMERID"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"])
						});
					}
				}
			}
			return orders;
		}


		public IList<Order> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter) {
			IList<Order> orders;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ALL_ORDERS_BY_CUSTOMERID_SP", customerid, supervisorid, dateIni, dateFin, idsOffice, idsCostCenter)) {
				using (var reader = database.ExecuteReader(cmd)) {
					orders = new List<Order>();
					while (reader.Read()) {
						orders.Add(new Order {
							Id = DataConvert.ToString(reader["ID"]),
							Code = DataConvert.ToString(reader["CODE"]),
							CustomerId = "",
							Description = "",
							Machinery = new Machinery(),
							TechnicalContacts = new List<TechnicalContact>(),
							CostCenter = "",
							Office = ""
						});
					}
				}
			}
			return orders;
		}


		//public IList<Order> GetOrderByCustomer(string customerid) {
		//    IList<Order> orders;
		//    var database = DatabaseFactory.CreateDatabase();
		//    using (var cmd = database.GetStoredProcCommand("GET_ALL_ORDERS_BY_CUSTOMERID_SP", customerid)) {
		//        using (var reader = database.ExecuteReader(cmd)) {
		//            orders = new List<Order>();
		//            while (reader.Read()) {
		//                orders.Add(new Order {
		//                    Id = DataConvert.ToString(reader["ID"]),
		//                    Code = DataConvert.ToString(reader["CODE"])
		//                });
		//            }
		//        }
		//    }
		//    return orders;
		//}

		public IList<Order> GetPaginated(Order filters, int pageIndex, int pageSize, out int totalRows) {
			totalRows = 0;
			return new List<Order>();
		}

		public Order GetById(string id) {
			Order order = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ORDER_BYID_SP", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						order = new Order {
							Id = DataConvert.ToString(reader["ID"]),
							Code = DataConvert.ToString(reader["CODE"]),
							CustomerId = DataConvert.ToString(reader["CUSTOMERID"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							//TechnicalContacts = new TechnicalContact
							//{
							//    Id = DataConvert.ToString(reader["TECHNICALCONTACTID"]),
							//    Name = DataConvert.ToString(reader["NAME"]),
							//    Email = DataConvert.ToString(reader["EMAIL"]),
							//    Phone = DataConvert.ToString(reader["PHONE"])
							//},
							Machinery = new Machinery {
								Id = DataConvert.ToString(reader["MACHINEID"]),
								Brand = DataConvert.ToString(reader["BRAND"]),
								SerialNumber = DataConvert.ToString(reader["SERIALNUMBER"]),
								LifeHours = DataConvert.ToDecimal(reader["LIFEHOURS"]),
								Model = DataConvert.ToString(reader["MODEL"])
							}
						};
					}
				}
			}
			return order;
		}


		public void Add(Order entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("INSERT_ORDER_SP_", entity.Id, entity.Code, entity.Machinery.Id, entity.CustomerId, entity.Description,
					//entity.TechnicalContacts.Id, entity.CreatedBy, entity.CreationDate, entity.Office, entity.CostCenter);
					"", entity.CreatedBy, entity.CreationDate, entity.Office, entity.CostCenter);
		}

		public void Update(Order entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("UPDATE_ORDER_SP", entity.Id, entity.Code, entity.CustomerId, entity.ModifiedBy, entity.LastModification);
		}

		public void Delete(string id, string modifiedBy, DateTime lastModification) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_ORDER_SP", id, modifiedBy, lastModification);
		}

		public string GetIdByCode(string code) {
			Order order = new Order();
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ORDER_BYCODE_SP", code)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						order = new Order {
							Id = DataConvert.ToString(reader["ID"])

						};
					}
				}
			}
			return order?.Id;
		}


	}
}
