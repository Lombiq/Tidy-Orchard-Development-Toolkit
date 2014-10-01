using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Lombiq.IsolatedOrchardDevelopmentToolkit.Services;
using Orchard.Environment;
using Orchard.FileSystems.VirtualPath;
using Orchard.Localization;

namespace Lombiq.IsolatedOrchardDevelopmentToolkit.AutofacModules
{
    /// <summary>
    /// Main module for the Isolated Development Toolkit.
    /// </summary>
    public class IsolatedDevelopmentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // These two should be in different modules as decorators should be active for shells too, while VirtualPathProvider there
            // should be only one.
            builder.RegisterModule<DecoratorsModule>();
            builder.RegisterModule<SingletonsModule>();
        }
    }
}