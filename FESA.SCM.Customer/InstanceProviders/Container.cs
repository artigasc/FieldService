using FESA.SCM.Customer.BE.ContactBE;
using FESA.SCM.Customer.BE.CustomerCompanyBE;
using FESA.SCM.Customer.BL.BusinessInterfaces;
using FESA.SCM.Customer.BL.BusinessServices;
using FESA.SCM.Customer.DA;
using Microsoft.Practices.Unity;

namespace FESA.SCM.Customer.InstanceProviders
{
    public class Container
    {
        #region Properties

        public static readonly IUnityContainer Current;

        #endregion Properties

        #region Constructor

        static Container()
        {
            Current = new UnityContainer();
            ConfigureContainer();
        }

        #endregion Constructor

        #region Methods

        private static void ConfigureContainer()
        {

            #region Repositories

            Current.RegisterType<ICustomerCompanyRepository, CustomerCompanyRepository>();
            Current.RegisterType<IContactRepository, ContactRepository>();

            #endregion Repositories

            #region Business

            Current.RegisterType<ICustomerService, CustomerService>();

            #endregion Business

            #region Services
            Current.RegisterType<ICustomerApi, CustomerApi>();

            #endregion Services
        }
        #endregion Methods
    }
}