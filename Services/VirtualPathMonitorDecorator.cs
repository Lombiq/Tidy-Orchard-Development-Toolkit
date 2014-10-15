using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard.Caching;
using Orchard.FileSystems.VirtualPath;

namespace Lombiq.TidyOrchardDevelopmentToolkit.Services
{
    public class VirtualPathMonitorDecorator : IVirtualPathMonitor
    {
        private readonly IVirtualPathMonitor _virtualPathMonitor;
        private readonly IVirtualPathDispatcher _virtualPathDispatcher;


        public VirtualPathMonitorDecorator(IVirtualPathMonitor virtualPathMonitor, IVirtualPathDispatcher virtualPathDispatcher)
        {
            _virtualPathMonitor = virtualPathMonitor;
            _virtualPathDispatcher = virtualPathDispatcher;
        }


        public IVolatileToken WhenPathChanges(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathMonitor.WhenPathChanges(virtualPath);
        }
    }
}
