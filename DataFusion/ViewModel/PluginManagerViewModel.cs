using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Unity;
using System.Collections.ObjectModel;

namespace DataFusion.ViewModel
{
    public class PluginManagerViewModel:ViewModelBase
    {
        private IUnityContainer _container;
        private PluginEntryController _controller;
        public PluginManagerViewModel(IUnityContainer container, PluginEntryController pluginEntryController )
        {
            _container = container;
            _controller = pluginEntryController;
        }
        public ObservableCollection<MinePluginConfigInfoViewModel> MinePluginConfigInfoViewModels
        {
            get => _controller.MinePluginConfigInfoViewModels;
        }
    }
}
