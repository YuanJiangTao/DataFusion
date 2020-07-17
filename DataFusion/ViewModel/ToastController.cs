using DataFusion.Data;
using DataFusion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DataFusion.ViewModel
{
   public class ToastController
    {
        private MainViewModel _mainVm;
        private ILogDog _log;

        public ToastController(IUnityContainer container)
        {
            _log = container.Resolve<ILogDog>(Constant.ClietnName);
        }

        public void Initializer(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public void ShowToast(string msg, Exception ex)
        {
            _mainVm.ToastText = $"{msg}\r\n{ex}";
            _log.Error(msg, ex);
        }
    }

    
}
