using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Orchard.Environment;

namespace Lombiq.IsolatedDevelopmentToolkit.Services
{
    /// <summary>
    /// This HTTP module rewrites requests to Orchard's built-in static files (that are now under the Orchard folder)
    /// so they can be reached under their original path.
    /// </summary>
    /// <remarks>
    /// The module should be registered from the Web.config and also the handlers section should be modified.
    /// </remarks>
    /// <example>
    /// Registration added to the Web.config:
    ///   <system.webServer>
    ///     <modules>
    ///         <remove name="IsolatedDevelopmentModule" />
    ///         <add name="IsolatedDevelopmentModule" type="Lombiq.IsolatedDevelopmentToolkit.Services.IsolatedDevelopmentModule, Lombiq.IsolatedDevelopmentToolkit" />
    ///     </modules>
    ///     </system.webServer>
    ///     
    /// Also modify the accessPolicy of <handlers> to "Script, Read" (should include Read).
    /// </example>
    public class IsolatedDevelopmentModule : IHttpModule, IShim
    {
        public IOrchardHostContainer HostContainer { get; set; }


        public void Init(HttpApplication application)
        {
            OrchardHostContainerRegistry.RegisterShim(this);

            application.BeginRequest += (sender, e) =>
                {
                    var httpApplication = (HttpApplication)sender;
                    var httpContext = httpApplication.Context;
                    var request = httpContext.Request;

                    // Only dealing with (seemingly) static file requests and only for files with an extension. This
                    // won't handle extension-less files but this should be extremely reare.
                    if (!string.IsNullOrEmpty(request.CurrentExecutionFilePathExtension))
                    {
                        var rewrittenPath = HostContainer.Resolve<IVirtualPathDispatcher>().ReMapPathIfOrchard(request.AppRelativeCurrentExecutionFilePath);
                        if (rewrittenPath != request.AppRelativeCurrentExecutionFilePath)
                        {
                            httpContext.RewritePath(rewrittenPath);
                        }
                    }
                };
        }

        public void Dispose() { }
    }
}
