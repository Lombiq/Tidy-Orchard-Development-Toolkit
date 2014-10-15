using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Lombiq.TidyOrchardDevelopmentToolkit.Services;
using Orchard.FileSystems.VirtualPath;

namespace Lombiq.TidyOrchardDevelopmentToolkit.AutofacModules
{
    public class SingletonsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VirtualPathProvider>().As<ICustomVirtualPathProvider>().SingleInstance();
            builder.RegisterType<VirtualPathDispatcher>().As<IVirtualPathDispatcher>().SingleInstance();
        }
    }
}
