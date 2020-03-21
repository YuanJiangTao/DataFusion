using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using DataFusion.Services;
using DataFusion.Model;

namespace DataFusion.ViewModel
{
    public class SystemConfigViewModel:ViewModelBase
    {
        private SystemConfigModel _systemConfigModel;
        private DataService _dataService;
        public SystemConfigViewModel(DataService dataService)
        {
            _dataService = dataService;
            _systemConfigModel = dataService.GetSystemConfigModel();
        }

        public SystemConfigModel SystemConfigModel
        {
            get => _systemConfigModel;
            set
            {
                Set(ref _systemConfigModel, value);
            }
        } 

    }
}
