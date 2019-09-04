using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FESA.SCM.Common;
using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.DA.TableTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.Customer.DA {
	public class ContactRepository : IContactRepository {
		public IList<Contact> GetAll() {
			throw new System.NotImplementedException();
		}

		public IList<Contact> GetPaginated(Contact filters, int pageIndex, int pageSize, out int totalRows) {
			throw new System.NotImplementedException();
		}

		public Contact GetById(string id) {
			throw new System.NotImplementedException();
		}

		public void Add(Contact entity) {
			throw new System.NotImplementedException();
		}

		public void Update(Contact entity) {
			throw new System.NotImplementedException();
		}

		public void Delete(string id, string modifiedBy, DateTime lastModification) {
			throw new System.NotImplementedException();
		}

		public IList<Contact> GetByCustomerId(string customerId) {
			IList<Contact> contacts;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_CONTACTS_BY_CUSTOMERID_SP", customerId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					contacts = new List<Contact>();
					while (reader.Read()) {
						contacts.Add(new Contact {
							Id = DataConvert.ToString(reader["ID"]),
							ContactId = DataConvert.ToString(reader["CONTACTID"]),
							CustomerId = customerId,
							Name = DataConvert.ToString(reader["NAME"]),
							LastName = DataConvert.ToString(reader["LASTNAME"]),
							Email = DataConvert.ToString(reader["EMAIL"]),
							Charge = DataConvert.ToString(reader["CHARGE"]),
							Phone = DataConvert.ToString(reader["PHONE"])
						});
					}
				}
			}
			return contacts;
		}

		public void InsertContacts(IList<Contact> contacts) {
			var contactsType = new ContactTableType();
			contactsType.AddRange(contacts);
			if (contactsType.Count == 0) return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_CONTACTS_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@CONTACTS",
				Value = contactsType,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "CONTACT_TYPE"
			});
			database.ExecuteNonQuery(cmd);
		}

		public void InsertContact(Contact contact) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_CONTACT_INSERT_SP", contact.Id, contact.Name, contact.Phone, contact.Email,
				contact.Active, contact.LastName, contact.Charge, contact.CustomerId, contact.IsSC);
		}
	}
}