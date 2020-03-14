using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces
{
    public interface IPluginHost : IServiceProvider
    {
        void Hello();

        int HostProcessId { get; }
        /// <summary>
        /// Reports fatal plugin error to the host; the plugin will be closed
        /// </summary>
        /// <param name="userMessage">Message explaining the nature of the error</param>
        /// <param name="fullExceptionText">Exception call stack as string</param>
        void ReportFatalError(string userMessage, string fullExceptionText);
    }
}
