using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.IsolatedDevelopmentToolkit.Services
{
    public interface IVirtualPathDispatcher
    {
        string AlterPathIfOrchard(string virtualPath);
    }


    public static class VirtualPathDispatcher
    {
        public static void AlterPathIfOrchard(this IVirtualPathDispatcher virtualPathDispatcher, ref string virtualPath)
        {
            virtualPath = virtualPathDispatcher.AlterPathIfOrchard(virtualPath);
        }
    }
}
