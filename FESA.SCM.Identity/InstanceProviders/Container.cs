using FESA.SCM.Identity.BE.RoleBE;
using FESA.SCM.Identity.BE.UserBE;
using FESA.SCM.Identity.BL.BusinessInterfaces;
using FESA.SCM.Identity.BL.BusinessIServices;
using FESA.SCM.Identity.DA;
using Microsoft.Practices.Unity;

namespace FESA.SCM.Identity.InstanceProviders
{
    public class Container
    {
        #region Properties

        public static readonly IUnityContainer Current;

        #endregion 

        #region Constructor

        static Container()
        {
            Current = new UnityContainer();
            ConfigureContainer();
        }

        #endregion 

        #region Methods

        private static void ConfigureContainer()
        {
            #region Repositories
            Current.RegisterType<IUserRepository, UserRepository>();
            Current.RegisterType<IRoleRepository, RoleRepository>();
            #endregion

            #region Business
            Current.RegisterType<IUserService, UserService>();
            Current.RegisterType<IRoleService, RoleService>();
            #endregion

            #region Services
            Current.RegisterType<IIdentityApi, IdentityApi>();
            #endregion
        }
        #endregion 
    }
}