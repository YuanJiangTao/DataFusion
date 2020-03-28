using DataFusion.Interfaces;
using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.PluginHosting
{
    public class RemotePlugin : MarshalByRefObject, IRemotePlugin
    {
        private readonly IPlugin _plugin;

        public RemotePlugin(IPlugin plugin)
        {
            _plugin = plugin;
            var control = plugin.CreateControl();
            var localContract = FrameworkElementAdapters.ViewToContractAdapter(control);
            Contract = new NativeHandleContractInsulator(localContract);
        }

        public INativeHandleContract Contract { get; private set; }

        public void Dispose()
        {
            _plugin.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _plugin.GetService(serviceType);
        }

        public void Load()
        {
            _plugin.Load();
        }
        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}
