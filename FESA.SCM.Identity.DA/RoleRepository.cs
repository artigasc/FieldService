using System;
using System.Collections.Generic;
using FESA.SCM.Common;
using FESA.SCM.Identity.BE.RoleBE;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.Identity.DA
{
    public class RoleRepository : IRoleRepository
    {
        public IList<Role> GetAll()
        {
            IList<Role> roles;
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("GET_ALL_ROLES_SP"))
            {
                using (var reader = database.ExecuteReader(cmd))
                {
                    roles = new List<Role>();
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = DataConvert.ToString(reader["ID"]),
                            Name = DataConvert.ToString(reader["NAME"])
                        });
                    }
                }
            }
            return roles;
        }

        public IList<Role> GetPaginated(Role filters, int pageIndex, int pageSize, out int totalRows)
        {
            throw new NotImplementedException();
        }

        public Role GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Add(Role entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("INSERT_ROLE_SP", entity.Id, entity.Name, entity.CreatedBy, entity.CreationDate);
        }

        public void Update(Role entity)
        {
            DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("UPDATE_ROLE_SP", entity.Id, entity.Name, entity.ModifiedBy, entity.LastModification);
        }

        public void Delete(string id, string modifiedBy, DateTime lastModification)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_ROLE_SP", id, modifiedBy, lastModification);
        }
    }
}