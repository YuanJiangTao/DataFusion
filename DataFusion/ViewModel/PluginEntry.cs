using DataFusion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using DataFusion.Interfaces;
using System.Windows;
using Unity.Lifetime;
using System.IO;
using System.AddIn.Pipeline;
using DataFusion.PluginHosting;

namespace DataFusion.ViewModel
{
    public class PluginEntry : IDisposable, IServiceProvider
    {
        private IUnityContainer _childContainer;


        public MineProtocalConfigInfo MineProtocalConfig { get; private set; }

        public PluginEntrySg PluginEntrySg { get; private set; }

        private PluginStartupInfo _startupInfo;
        private PluginProcessProxy _remoteProcess;

        private bool _isDisposing;
        private bool _fatalErrorOccurred;

        public PluginEntry(IUnityContainer container)
        {
            _childContainer = container.CreateChildContainer();
        }
        internal void Load(PluginEntrySg pluginEntrySg, MineProtocalConfigInfo mineProtocalConfigInfo)
        {
            if (PluginEntrySg != null) throw new InvalidOperationException("Plugin can be loaded only once");
            PluginEntrySg = pluginEntrySg;
            MineProtocalConfig = mineProtocalConfigInfo;

            Initialize();

            var host = _childContainer.Resolve<PluginViewOfHost>();
            host.FatalError += OnFatalError;

            _remoteProcess = _childContainer.Resolve<PluginProcessProxy>();
            _remoteProcess.Start();
            new ProcessMonitor(OnProcessExited).Start(_remoteProcess.Process);
            _remoteProcess.LoadPlugin();

        }
        public void CreateView()
        {
            View = FrameworkElementAdapters.ContractToViewAdapter(_remoteProcess.RemotePlugin.Contract);
        }


        private void OnProcessExited()
        {
            if (!_isDisposing && _fatalErrorOccurred)
            {
                ReportError("Plugin process terminated unexpectedly", null);
            }
        }

        public event EventHandler<PluginErrorEventArgs> Error;

        private void OnFatalError(Exception ex)
        {
            _fatalErrorOccurred = true;
            ReportError(null, ex);
        }

        private void ReportError(string message, Exception ex)
        {
            Error?.Invoke(this, new PluginErrorEventArgs(this, message, ex));
        }


        public void Dispose()
        {
            _isDisposing = true;
            try
            {
                var disposableView = View as IDisposable;
                if (disposableView != null) disposableView.Dispose();
            }
            catch (Exception ex)
            {
                ReportError("Error when disposing view", ex);
            }
            _childContainer.Dispose();
        }
        private void Initialize()
        {
            _childContainer.RegisterType<IPluginHost, PluginViewOfHost>(new ContainerControlledLifetimeManager());
            _childContainer.RegisterType<PluginProcessProxy>(new ContainerControlledLifetimeManager());
            _childContainer.RegisterInstance<MineProtocalConfigInfo>(MineProtocalConfig, new ContainerControlledLifetimeManager());
            _startupInfo = new PluginStartupInfo()
            {
                AssemblyPath = PluginEntrySg.AssemblyPath,
                AssemblyName = Path.GetFileNameWithoutExtension(PluginEntrySg.AssemblyPath),
                Bits = PluginEntrySg.Bits,
                Title = PluginEntrySg.Title,
                Version = PluginEntrySg.Version,
                Description = PluginEntrySg.Description,
                OriginalFilename = PluginEntrySg.OriginalFilename,
                Parameters = PluginEntrySg.Parameters,
                Company = PluginEntrySg.Company,
                IsDebug = PluginEntrySg.IsDebug,
                ProductName = PluginEntrySg.ProductName,
                IsEnable = PluginEntrySg.IsEnable
            };
            _childContainer.RegisterInstance(_startupInfo, new ContainerControlledLifetimeManager());
        }

        public FrameworkElement View { get; private set; }

        public string Title { get; private set; }

        public object GetService(Type serviceType)
        {
            if (_remoteProcess == null) return null;
            return _remoteProcess.RemotePlugin.GetService(serviceType);
        }
    }

    public class PluginErrorEventArgs : EventArgs
    {
        public PluginErrorEventArgs(PluginEntry pluginEntry, string message, Exception exception)
        {
            PluginEntry = pluginEntry;
            Message = message;
            Exception = exception;
        }

        public PluginEntry PluginEntry { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
    }
}
