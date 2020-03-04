using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Services;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;
        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
        }



        private object _mainContent;
        public object MainContent
        {
            get => _mainContent;
            set => Set(ref _mainContent, value);
        }
    }
}