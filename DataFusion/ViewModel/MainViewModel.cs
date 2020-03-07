using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using DataFusion.Services;
using DataFusion.Data;
using System.Windows;
using DataFusion.Model;
using System;
using System.Collections.ObjectModel;

namespace DataFusion.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DataService _dataService;
        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
            Messenger.Default.Register<SubMenuItem>(this, MessageToken.LoadShowContent, t =>
             {
                 if (t != null)
                 {
                     if(MainContent is IDisposable dispose)
                     {
                         dispose.Dispose();
                     }
                     ContentTitle = t.Name;
                     MainContent = t.Screen;
                 }
             });
        }




        private ObservableCollection<MainItemMenuViewModel> _mainItemMenuViews;

        public ObservableCollection<MainItemMenuViewModel> MenuInfoList
        {
            get => _mainItemMenuViews;
            set => Set(ref _mainItemMenuViews, value);
        }




        private string _contentTitle;
        public string ContentTitle
        {
            get=> _contentTitle;
            set => Set(ref _contentTitle, value);
        }

        private object _mainContent;
        public object MainContent
        {
            get => _mainContent;
            set => Set(ref _mainContent, value);
        }
        private object _mainDataContext;
        public object MainDataContext
        {
            get => _mainDataContext;
            set => Set(ref _mainDataContext, value);
        }
    }
}