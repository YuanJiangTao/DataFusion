using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataFusion.Interfaces
{
    public interface IPlugin : IServiceProvider, IDisposable
    {
        FrameworkElement CreateControl();
    }
}
