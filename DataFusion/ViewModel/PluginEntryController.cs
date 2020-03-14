using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DataFusion.ViewModel
{
    class PluginEntryController
    {
        private IUnityContainer _unityContainer;
        public PluginEntryController(IUnityContainer container)
        {
            _unityContainer = container;
        }
    }
}
