using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.TidyOrchardDevelopmentToolkit.Services
{
    public interface IVirtualPathDispatcher
    {
        /// <summary>
        /// Re-maps the given path if it originally points to a resource that is built into Orchard.
        /// </summary>
        /// <example>
        /// The path "~/Modules/Orchard.Autoroute/Styles/orchard-autoroute-settings.css" will be mapped to
        /// "~/Orchard/src/Orchard.Web/Modules/Orchard.Autoroute/Styles/orchard-autoroute-settings.css".
        /// This doesn't happen for paths that point to resources not existing under the Orchard folder.
        /// </example>
        /// <param name="virtualPath">The virtual path to re-map.</param>
        /// <returns>The re-mapped path if the path should be re-mapped or the original path.</returns>
        string ReMapPathIfOrchard(string virtualPath);

        /// <summary>
        /// Converts the path back to its original version if it points to a resource that is built into Orchard.
        /// </summary>
        /// <example>
        /// The path "~/Orchard/src/Orchard.Web/Modules/Orchard.Autoroute/Styles/orchard-autoroute-settings.css"
        /// will be converted back to "~/Modules/Orchard.Autoroute/Styles/orchard-autoroute-settings.css".
        /// </example>
        /// <param name="virtualPath">The virtual path to convert back.</param>
        /// <returns>The converted virtual path if it should be converted or the original path.</returns>
        string ConvertBackIfOrchard(string virtualPath);
    }


    public static class VirtualPathDispatcherExtensions
    {
        public static void AlterPathIfOrchard(this IVirtualPathDispatcher virtualPathDispatcher, ref string virtualPath)
        {
            virtualPath = virtualPathDispatcher.ReMapPathIfOrchard(virtualPath);
        }
    }
}
