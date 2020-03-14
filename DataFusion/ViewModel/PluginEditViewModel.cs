using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace DataFusion.ViewModel
{
    public class PluginEditViewModel : ViewModelBase
    {
        public PluginEditViewModel()
        {

        }

        public string TestStr { get; set; } = "Test";
    }
}
