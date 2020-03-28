using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DataFusion.ViewModel
{
    internal class PluginViewOfHost : MarshalByRefObject, IPluginHost
    {

        private readonly IUnityContainer _container;

        public PluginViewOfHost(IUnityContainer container)
        {
            _container = container;
        }

        public int HostProcessId => Process.GetCurrentProcess().Id;


        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public void Hello()
        {
            Console.WriteLine("Plugin Host say hello");
        }

        public event Action<Exception> FatalError;

        public Exception LastError { get; private set; }

        public void ReportFatalError(string userMessage, string fullExceptionText)
        {
            LastError = new PluginException(userMessage, fullExceptionText);
            FatalError?.Invoke(LastError);
        }
        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
   public  class PluginException : Exception
    {
        private readonly string _fullExceptionText;

        public PluginException(string userMessage, string fullExceptionText)
            :
            base(userMessage)
        {
            _fullExceptionText = fullExceptionText;
        }

        public override string ToString()
        {
            return "Plugin exception: " + _fullExceptionText;
        }
    }
}
