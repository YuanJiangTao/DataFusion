using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataFusion.Interfaces
{
    public abstract class PluginBase : IPlugin
    {
        public abstract FrameworkElement CreateControl();

        public virtual void Dispose()
        {

        }

        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}
