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
using DataFusion.Data;

namespace DataFusion.ViewModel
{
    public class PluginStateDisplayViewModel : ViewModelBase
    {
        private PluginEntryController _controller;
        public PluginStateDisplayViewModel(PluginEntryController controller)
        {
            _controller = controller;
            Messenger.Default.Register<string>(this, MessageToken.ProtocalStateChanged, p =>
            {
                IsEmpty = _controller.MineProtocalEnableConfigViewModels.Count == 0;
            });
        }

        public ObservableCollection<MineProtocalEnableConfigViewModel> MineProtocalEnableConfigViewModels
        {
            get => _controller.MineProtocalEnableConfigViewModels;
        }
        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                RaisePropertyChanged();
            }
        }

        //public bool IsEmpty
        //{
        //    get => _controller.MineProtocalEnableConfigViewModels.Count == 0;
        //}
        public override void Cleanup()
        {
            try
            {
                base.Cleanup();
                Messenger.Default.Unregister<string>(this, MessageToken.ProtocalStateChanged);
            }
            catch
            {
            }

        }
    }
}
