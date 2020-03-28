using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.PluginHosting
{
    public interface IPluginLoader : IDisposable
    {
        void Hello();
        IRemotePlugin LoadPlugin(IPluginHost host, PluginStartupInfo startupInfo);
    }
}
