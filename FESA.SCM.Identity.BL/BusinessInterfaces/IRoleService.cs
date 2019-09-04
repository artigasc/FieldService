using System;
using System.Collections.Generic;
using FESA.SCM.Identity.BE.RoleBE;

namespace FESA.SCM.Identity.BL.BusinessInterfaces
{
    public interface IRoleService
    {
        IList<Role> GetAll();
        IList<Role> GetPaginated(Role role, int pageIndex, int pageSize, out int totalRows);
        Role GetRoleById(string id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(string id, string modifiedBy, DateTime lastModification);
    }
}