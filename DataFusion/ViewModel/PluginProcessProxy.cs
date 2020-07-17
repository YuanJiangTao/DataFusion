using DataFusion.Interfaces;
using DataFusion.PluginHosting;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFusion.ViewModel
{
    public class PluginProcessProxy : IDisposable
    {
        private readonly IPluginHost _host;
        private readonly PluginStartupInfo _startupInfo;
        private EventWaitHandle _readyEvent;
        private Process _process;
        private string _name;
        private IPluginLoader _pluginLoader;
        public Process Process { get { return _process; } }

        public PluginProcessProxy(PluginStartupInfo startupInfo, IPluginHost host)
        {
            _startupInfo = startupInfo;
            _host = host;
        }
        public void Start()
        {
            if (Process != null) throw new InvalidOperationException("Plugin process already started, cannot load more than one plugin per process");
            StartPluginProcess(_startupInfo.AssemblyPath);
        }
        public void LoadPlugin()
        {
            if (Process == null) throw new InvalidOperationException("Plugin process not started");
            if (Process.HasExited) throw new InvalidOperationException("Plugin process has terminated unexpectedly");

            _pluginLoader = GetPluginLoader();
            RemotePlugin = _pluginLoader.LoadPlugin(_host, _startupInfo);
        }

        private IPluginLoader GetPluginLoader()
        {
            if(Process.HasExited)
            {
                throw new InvalidOperationException("Plugin process has terminated unexpectedly");
            }
            var timeoutMs = 10000;
            if (!_readyEvent.WaitOne(timeoutMs))
            {
                throw new InvalidOperationException("Plugin process did not respond within timeout period");
            }

            var hostChannelName = "DataFusion." + Process.GetCurrentProcess().Id;
            IpcServices.RegisterChannel(hostChannelName);

            var url = "ipc://" + _name + "/PluginLoader";
            var pluginLoader = (IPluginLoader)Activator.GetObject(typeof(IPluginLoader), url);
            return pluginLoader;
        }
        private void StartPluginProcess(string assemblyPath)
        {
            _name = "PluginProcess." + Guid.NewGuid();
            var eventName = _name + ".Ready";
            _readyEvent = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);
            var directory = Path.GetDirectoryName(GetType().Assembly.Location);
            var exeFile = _startupInfo.Bits == 64 ? "PluginProcess64.exe" : "PluginProcess.exe";
            var processName = Path.Combine(directory, exeFile);
            if (!File.Exists(processName)) throw new InvalidOperationException("Could not find file '" + processName + "'");
            const string quote = "\"";
            const string doubleQuote = "\"\"";

            var quotedAssemblyPath = quote + assemblyPath.Replace(quote, doubleQuote) + quote;
            var createNoWindow = !_startupInfo.IsDebug;

            var info = new ProcessStartInfo
            {
                Arguments = _name + " " + quotedAssemblyPath,
                CreateNoWindow = createNoWindow,
                UseShellExecute = false,
                FileName = processName,
            };
            Trace.WriteLine(info.Arguments);
            _process = Process.Start(info);
       }

        public IRemotePlugin RemotePlugin { get; private set; }
        public void Dispose()
        {
            if (RemotePlugin != null)
            {
                try
                {
                    RemotePlugin.Dispose();
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<ToastErrorMsg>(new ToastErrorMsg("Remote plugin dispose Error", ex));
                }
            }

            if (_pluginLoader != null)
            {
                try
                {
                    _pluginLoader.Dispose();
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<ToastErrorMsg>(new ToastErrorMsg("Error disposing plugin loader ", ex));
                }
            }

            // this can take some time if we have many plugins; should be made asynchronous
            if (Process != null)
            {
                Process.WaitForExit(5000);
                if (!Process.HasExited)
                {
                    Messenger.Default.Send<ToastErrorMsg>(new ToastErrorMsg("Remote process did not exit within timeout period and will be terminated ", new Exception()));
                    Process.Kill();
                }
            }
        }
    }
}
