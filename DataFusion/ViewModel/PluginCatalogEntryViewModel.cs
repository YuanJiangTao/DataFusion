using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace DataFusion.ViewModel
{
    public class PluginCatalogEntryViewModel : ViewModelBase
    {
        private readonly PluginEntryController _controller;
        public PluginCatalogEntryViewModel(PluginEntryController controller)
        {
            _controller = controller;
        }
        public ObservableCollection<PluginEntryViewModel> PluginEntryVms
        {
            get => _controller.LoadPluginEntryVms;
        }
    }
}
