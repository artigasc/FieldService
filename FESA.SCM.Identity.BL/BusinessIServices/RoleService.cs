using System;
using System.Collections.Generic;
using FESA.SCM.Identity.BE.RoleBE;
using FESA.SCM.Identity.BL.BusinessInterfaces;

namespace FESA.SCM.Identity.BL.BusinessIServices
{
    public class RoleService : IRoleService
    {
        #region Members
        private readonly IRoleRepository _roleRepository;
        #endregion
        #region Constructor
        public RoleService(IRoleRepository roleRepository)
        {
            if(roleRepository == null)
                throw new ArgumentNullException(nameof(roleRepository));

            _roleRepository = roleRepository;
        }
        #endregion
        #region Methods
        public IList<Role> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public IList<Role> GetPaginated(Role role, int pageIndex, int pageSize, out int totalRows)
        {
            return _roleRepository.GetPaginated(role, pageIndex, pageSize, out totalRows);
        }

        public Role GetRoleById(string id)
        {
            return _roleRepository.GetById(id);
        }

        public void AddRole(Role role)
        {
            role.Id = Guid.NewGuid().ToString();
            _roleRepository.Add(role);
        }

        public void UpdateRole(Role role)
        {
            _roleRepository.Update(role);
        }

        public void DeleteRole(string id, string modifiedBy, DateTime lastModification)
        {
            _roleRepository.Delete(id, modifiedBy, lastModification);
        }
        #endregion
    }
}