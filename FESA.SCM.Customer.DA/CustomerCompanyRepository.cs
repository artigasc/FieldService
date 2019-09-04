using System;
using System.Collections.Generic;
using FESA.SCM.Common;
using FESA.SCM.Customer.BE.CustomerCompanyBE;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.Customer.DA
{
    public class CustomerCompanyRepository : ICustomerCompanyRepository
    {
        public IList<CustomerCompany> GetAll()
        {
            IList<CustomerCompany> customers;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_ALL_CUSTOMERS_SP"))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    customers = new List<CustomerCompany>();
                    while (reader.Read())
                    {
                        customers.Add(new CustomerCompany
                        {
                            Id = DataConvert.ToString(reader["ID"]),
                            Name = DataConvert.ToString(reader["NAME"]),
                            BusinessName = DataConvert.ToString(reader["BUSINESSNAME"]),
                            Ruc = DataConvert.ToString(reader["RUC"])
                        });
                    }
                }
            }
            return customers;
        }

        public IList<CustomerCompany> GetPaginated(CustomerCompany filters, int pageIndex, int pageSize, out int totalRows)
        {
            throw new NotImplementedException();
        }

        public CustomerCompany GetById(string id)
        {
            CustomerCompany customer = null;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_CUSTOMER_BYID_SP", id))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        customer = new CustomerCompany
                        {
                            Id = DataConvert.ToString(reader["ID"]),
                            Name = DataConvert.ToString(reader["NAME"]),
                            BusinessName = DataConvert.ToString(reader["BUSINESSNAME"]),
                            Ruc = DataConvert.ToString(reader["RUC"])
                        };
                    }
                }
            }
            return customer;
        }

        public void Add(CustomerCompany entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("INSERT_CUSTOMER_SP", entity.Id, entity.Name, entity.BusinessName, entity.Ruc,
                    entity.CreatedBy, entity.CreationDate);
        }

        public void Update(CustomerCompany entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("UPDATE_CUSTOMER_SP", entity.Id, entity.Name, entity.BusinessName, entity.Ruc,
                    entity.ModifiedBy, entity.LastModification);
        }

        public void Delete(string id, string modifiedBy, DateTime lastModification)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_CUSTOMER_SP", id, modifiedBy, lastModification);
        }

        public CustomerCompany GetByRuc(string ruc)
        {
            CustomerCompany customer = null;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_CUSTOMER_BY_RUC_SP", ruc))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        customer = new CustomerCompany
                        {
                            Id = DataConvert.ToString(reader["ID"]),
                            Name = DataConvert.ToString(reader["NAME"]),
                            BusinessName = DataConvert.ToString(reader["BUSINESSNAME"]),
                            Ruc = DataConvert.ToString(reader["RUC"])
                        };
                    }
                }
            }
            return customer;
        }
    }
}
