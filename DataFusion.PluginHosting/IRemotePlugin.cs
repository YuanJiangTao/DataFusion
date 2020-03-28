using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.PluginHosting
{
    public interface IRemotePlugin : IServiceProvider, IDisposable
    {
        INativeHandleContract Contract { get; }
        void Load();
    }
}
