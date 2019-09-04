using FESA.SCM.WorkOrder.BE.MachineryBE;
using FESA.SCM.WorkOrder.BE.OrderBE;
using FESA.SCM.WorkOrder.BE.OrderPerClient;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using FESA.SCM.WorkOrder.BL.BusinessInterfaces;
using FESA.SCM.WorkOrder.BL.BusinessServices;
using FESA.SCM.WorkOrder.DA;
using Microsoft.Practices.Unity;

namespace FESA.SCM.WorkOrder.InstanceProviders
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

            Current.RegisterType<IOrderRepository, OrderRepository>();
            Current.RegisterType<IMachineryRepository, MachineryRepository>();
            Current.RegisterType<ITechnicalContactRepository, TechnicalContactRepository>();
            Current.RegisterType<IOrderPerClientRepository, OrderPerClientRepository>();

            #endregion Repositories

            #region Business

            Current.RegisterType<IOrderService, OrderService>();
            Current.RegisterType<IOrderPerClientService, OrderPerClientService>();

            #endregion Business

            #region Services
            Current.RegisterType<IWorkOrderApi, WorkOrderApi>();

            #endregion Services
        }
        #endregion Methods
    }
}