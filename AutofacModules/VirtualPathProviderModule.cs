using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Lombiq.IsolatedDevelopmentToolkit.Services;
using Orchard.FileSystems.VirtualPath;

namespace Lombiq.IsolatedDevelopmentToolkit.AutofacModules
{
    public class VirtualPathProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VirtualPathProvider>().As<ICustomVirtualPathProvider>().As<IVirtualPathDispatcher>().SingleInstance();
        }
    }
}
