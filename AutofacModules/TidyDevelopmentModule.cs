using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Lombiq.TidyOrchardDevelopmentToolkit.Services;
using Orchard.Environment;
using Orchard.FileSystems.VirtualPath;
using Orchard.Localization;

namespace Lombiq.TidyOrchardDevelopmentToolkit.AutofacModules
{
    /// <summary>
    /// Main module for the Tidy Development Toolkit.
    /// </summary>
    public class TidyDevelopmentModule : Module
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