using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BE.ReportBE;
using FESA.SCM.FieldService.BL.BusinessInterfaces;
using FESA.SCM.FieldService.BL.BusinessServices;
using FESA.SCM.FieldService.DA;
using Microsoft.Practices.Unity;

namespace FESA.SCM.FieldService.InstanceProviders
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
            Current.RegisterType<IAssignmentRepository, AssignmentRepository>();
            Current.RegisterType<IActivityRepository, ActivityRepository>();
            Current.RegisterType<IDocumentRepository, DocumentRepository>();
            Current.RegisterType<IOcupabilityRepository, OcupabilityRepository>();
            Current.RegisterType<IOcupabilityThroughTimeRepository, OcupabilityThroughTimeRepository>();
            #endregion 

            #region Business
            Current.RegisterType<IAssignmentService, AssignmentService>();
            Current.RegisterType<IOcupabilityService, OcupabilityService>();
            Current.RegisterType<IOcupabilityThroughTimeService, OcupabilityThroughTimeService>();
            #endregion 

            #region Services
            Current.RegisterType<IFieldServiceApi, FieldServiceApi>();
            #endregion
        }
        #endregion
    }
}