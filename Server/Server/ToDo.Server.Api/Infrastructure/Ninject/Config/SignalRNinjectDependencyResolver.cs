namespace ToDo.Api.Infrastructure.Ninject.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Ninject;
    using Microsoft.AspNet.SignalR;

    public class SignalRNinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel kernel;

        public SignalRNinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}