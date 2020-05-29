using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Unity;
using DataFusion.Services;
using System.Collections.ObjectModel;
using DataFusion.Interfaces;
using DataFusion.Model;

namespace DataFusion.ViewModel
{
    public class PluginStateDisplayViewModel:ViewModelBase
    {
        private PluginEntryController _controller;
        public PluginStateDisplayViewModel(PluginEntryController controller)
        {
            _controller = controller;
        }

        public ObservableCollection<MinePluginConfigModel> MineProtocalConfigInfos
        {
            get => _controller.MineProtocalConfigInfos;
        }

    }
}
