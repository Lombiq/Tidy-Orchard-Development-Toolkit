using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Lombiq.IsolatedDevelopmentToolkit.Services;
using Orchard.Environment;
using Orchard.FileSystems.VirtualPath;
using Orchard.Localization;

namespace Lombiq.IsolatedDevelopmentToolkit
{
    public class IsolatedDevelopmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VirtualPathProvider>().As<ICustomVirtualPathProvider>().As<IVirtualPathDispatcher>().SingleInstance();

            // Without shell registrations everything hooked up in this module would be only valid for the application scope, not
            // for shell scopes.
            var shellRegistrations = new ShellContainerRegistrations
            {
                Registrations = shellBuilder =>
                {
                    shellBuilder.RegisterModule(this);
                }
            };
            builder.RegisterInstance(shellRegistrations).As<IShellContainerRegistrations>();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            if (registration.Activator.LimitType.IsAssignableTo<IVirtualPathProvider>())
            {
                registration.Activating += (sender, e) =>
                {
                    var decorator = new VirtualPathProviderDecorator(
                        (IVirtualPathProvider)e.Instance,
                        e.Context.Resolve<IVirtualPathDispatcher>());
                    e.Instance = decorator;
                };
            }

            if (registration.Activator.LimitType.IsAssignableTo<IVirtualPathMonitor>())
            {
                registration.Activating += (sender, e) =>
                {
                    var decorator = new VirtualPathMonitorDecorator(
                        (IVirtualPathMonitor)e.Instance,
                        e.Context.Resolve<IVirtualPathDispatcher>());
                    e.Instance = decorator;
                };
            }
        }


        private class ShellContainerRegistrations : IShellContainerRegistrations
        {
            public Action<ContainerBuilder> Registrations { get; set; }
        }
    }
}