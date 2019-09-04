using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace FESA.SCM.Customer.InstanceProviders
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        #region Members

        private readonly Type _serviceType;
        private readonly IUnityContainer _container;

        #endregion Members

        #region Constructor

        /// <summary>
        /// Create a new instance of unity instance provider
        /// </summary>
        /// <param name="serviceType">The service where we apply the instance provider</param>
        public UnityInstanceProvider(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            _serviceType = serviceType;
            _container = Container.Current;
        }

        #endregion Constructor

        #region IInstance Provider Members

        /// <summary>
        /// <see cref="IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="IInstanceProvider"/></param>
        /// <param name="message"><see cref="IInstanceProvider"/></param>
        /// <returns><see cref="IInstanceProvider"/></returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            //This is the only call to UNITY container in the whole solution
            return _container.Resolve(_serviceType);
        }

        /// <summary>
        /// <see cref="IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="IInstanceProvider"/></param>
        /// <returns><see cref="IInstanceProvider"/></returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, Message.CreateMessage(MessageVersion.Default, string.Empty));
        }

        /// <summary>
        /// <see cref="IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="IInstanceProvider"/></param>
        /// <param name="instance"><see cref="IInstanceProvider"/></param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            (instance as IDisposable)?.Dispose();
        }

        #endregion IInstance Provider Members
    }
}