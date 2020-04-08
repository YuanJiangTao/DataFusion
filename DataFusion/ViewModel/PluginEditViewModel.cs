using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace DataFusion.ViewModel
{
    public class PluginEditViewModel : ViewModelBase
    {

        private PluginEntryController _controller;
        public PluginEditViewModel(PluginEntryController controller)
        {
            _controller = controller;
            AddCommand = new Lazy<RelayCommand>(() => new RelayCommand(() =>
            {

            })).Value;
        }

        public ObservableCollection<PluginEntryViewModel> PluginEntryViewModels
        {
            get => _controller.LoadPluginEntryVms;
        }

        public ICommand AddCommand { get; set; }


        public string TestStr { get; set; } = "Test";
    }
}
