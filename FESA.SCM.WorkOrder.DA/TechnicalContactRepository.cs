using System;
using System.Collections.Generic;
using FESA.SCM.Common;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.WorkOrder.DA {
    public class TechnicalContactRepository : ITechnicalContactRepository {
        public IList<TechnicalContact> GetAll() {
            return new List<TechnicalContact>();
        }

        public IList<TechnicalContact> GetPaginated(TechnicalContact filters, int pageIndex, int pageSize, out int totalRows) {
            totalRows = 10;
            return new List<TechnicalContact>();
        }

        public TechnicalContact GetById(string id) {
            TechnicalContact contact = null;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_CONTACT_BYID_SP", id)) {
                using (var reader = database.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        contact = new TechnicalContact {
                            Id = DataConvert.ToString(reader["ID"]),
                            Name = DataConvert.ToString(reader["NAME"]),
                            Email = DataConvert.ToString(reader["EMAIL"]),
                            Phone = DataConvert.ToString(reader["PHONE"])
                        };
                    }
                }
            }
            return contact;
        }

        public void Add(TechnicalContact entity) {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("INSERT_CONTACT_SP", entity.Id, entity.ContactId, entity.Name, entity.Email, entity.Phone,
                    entity.LastName, entity.Charge,
                    entity.CreatedBy, entity.CreationDate, entity.CustomerId);
        }

        public void Update(TechnicalContact entity) {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("UPDATE_CONTACT_SP", entity.Id, entity.Name, entity.LastName, entity.Email, entity.Phone,
                    entity.Charge, entity?.ModifiedBy);
        }

        public void Delete(string id, string modifiedBy, DateTime lastModification) {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_CONTACT_SP", id, modifiedBy, lastModification);
        }
   
        public IList<TechnicalContact> GetContactByIdOrder(string orderId) {
            IList<TechnicalContact> contacts;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_CONTACTS_BY_ORDERID_SP", orderId)) {
                using (var reader = database.ExecuteReader(cmd)) {
                    contacts = new List<TechnicalContact>();
                    while (reader.Read()) {
                        contacts.Add(new TechnicalContact {
                            Id = DataConvert.ToString(reader["ID"]),
                            ContactId = DataConvert.ToString(reader["CONTACTID"]),
                            CustomerId = orderId,
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

        public void DeleteByOrderAndContactId(string orderId, string contactId) {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_CONTACTBYORDERCONTACTID_SP", orderId, contactId);
        }


    }
}