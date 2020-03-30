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

namespace DataFusion.ViewModel
{
    public class PluginStateDisplayViewModel:ViewModelBase
    {
        private PluginEntryController _controller;
        private MessageService _service;
        public PluginStateDisplayViewModel(PluginEntryController controller,MessageService service)
        {
            _controller = controller;
            _service = service;
        }

        public ObservableCollection<MineProtocalConfigInfo> MineProtocalConfigInfos
        {
            get => _controller.MineProtocalConfigInfos;
        }

    }
}
