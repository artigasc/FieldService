using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FESA.SCM.FieldService.InstanceProviders
{
    public class UnityInstanceProviderBehaviourAttribute : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                dispatcher.Endpoints.ToList().ForEach(endpoint =>
                {
                    endpoint.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(serviceDescription.ServiceType);
                });
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        #endregion IServiceBehavior Members
    }
}