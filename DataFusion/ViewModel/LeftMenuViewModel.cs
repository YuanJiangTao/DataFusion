using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using DataFusion.Model;

namespace DataFusion.ViewModel
{
    public class LeftMenuViewModel : ViewModelBase
    {

        public LeftMenuViewModel()
        {

        }
        private ObservableCollection<MainItemMenu> mainItemMenus;

        public ObservableCollection<MainItemMenu> MainItemMenus
        {
            get => mainItemMenus;
            set => Set(ref mainItemMenus, value);
        }

    }
}
