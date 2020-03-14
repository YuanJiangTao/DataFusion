using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using DataFusion.Model;
using DataFusion.Services;

namespace DataFusion.ViewModel
{
    public class NoUserContentViewModel:ViewModelBase
    {

        public NoUserContentViewModel(DataService dataService)
        {
            systemConfigModel = dataService.GetSystemConfigModel();
        }
        private SystemConfigModel systemConfigModel;

        public SystemConfigModel SystemConfigModel
        {
            get => systemConfigModel;
            set
            {
                Set(ref systemConfigModel, value);
                //TODO:保存配置信息
            }
        }

    }
}
