using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;

namespace DataFusion.PluginHosting
{
    public class PluginLoader : MarshalByRefObject, IPluginLoader
    {
        private Dispatcher _dispatcher;
        private IPluginHost _host;
        private string _name;

        public void Run(string name)
        {
            _name = name;
            _dispatcher = Dispatcher.CurrentDispatcher;
            new AssemblyResolver().Setup();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Console.WriteLine("注册IPC:" + name);
            IpcServices.RegisterChannel(name);
            RegisterObject();
            SignalReady();

            Dispatcher.Run();
        }
        private void RegisterObject()
        {
            Console.WriteLine("Client Register EntryLoader...");
            RemotingServices.Marshal(this, "PluginLoader", typeof(IPluginLoader));
        }
        private void SignalReady()
        {
            var eventName = _name + ".Ready";
            var readyEvent = EventWaitHandle.OpenExisting(eventName);
            readyEvent.Set();
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (e.ExceptionObject as Exception) ??
              new Exception("Unknown error. Exception object is null");
            ReportFatalError(exception);
        }

        private void ReportFatalError(Exception exception)
        {
            if (_host != null)
            {
                _host.ReportFatalError(ExceptionUtil.GetUserMessage(exception), exception.ToString());
            }
            else
            {

            }
            Console.WriteLine("意外退出.");
            Environment.Exit(2);
        }
        public void Dispose()
        {
            Console.WriteLine("Shutdown requested");

            if (_dispatcher != null)
            {
                Console.WriteLine("Performing dispatcher shutdown");
                _dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
            }
            else
            {
                Console.WriteLine("No dispatcher, exiting the process");
                Environment.Exit(1);
            }
        }

        public void Hello()
        {

        }

        public IRemotePlugin LoadPlugin(IPluginHost host, PluginStartupInfo startupInfo)
        {
            _host = host;
            new ProcessMonitor(Dispose).Start(_host.HostProcessId);
            Func<PluginStartupInfo, object> createOnUiThread = LoadPluginOnUiThread;
            var result = _dispatcher.Invoke(createOnUiThread, startupInfo);
            if (result is Exception)
            {
                throw new TargetInvocationException((Exception)result);
            }
            //return (IRemotePlugin)((Task<object>)result).Result;
            return (IRemotePlugin)result;

        }
        private  object LoadPluginOnUiThread(PluginStartupInfo startupInfo)
        {
            var ipluginType = typeof(IPlugin);
            try
            {
                var obj = PluginCreator.CreatePlugin(startupInfo.AssemblyName, ipluginType, _host);
                var localPlugin = obj as IPlugin;
                if (localPlugin == null)
                {
                    var message = string.Format("Object of type {0} cannot be loaded as plugin " + "because it does not implement IPlugin interface", ipluginType.Name);
                    throw new InvalidOperationException(message);
                }

                localPlugin.Load();
                //await Task.Factory.StartNew(() =>
                //{
                //    localPlugin.Load();
                //});
                var remotePlugin = new RemotePlugin(localPlugin);
                return remotePlugin;
            }
            catch (Exception ex)
            {
                var message = String.Format("Error loading type '{0}' from assembly '{1}'. {2}",
                    ipluginType.Name, startupInfo.AssemblyName, ex.Message);
                return new ApplicationException(message, ex);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}
