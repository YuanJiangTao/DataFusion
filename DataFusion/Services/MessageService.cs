using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using Unity;
using DataFusion.Interfaces;
using DataFusion.Data;

namespace DataFusion.Services
{
    public class MessageService
    {
        private ILogDog logDog;
        public MessageService(IUnityContainer unityContainer)
        {
            logDog = unityContainer.Resolve<ILogDog>(Constant.ClietnName);
        }

        public void Warnging(string message)
        {
            Growl.Warning(message);
            logDog.Warn(message);
        }
        public void Info(string message)
        {
            Growl.Info(message);
            LogD.Info(message);
        }
        public void Error(string message)
        {
            Growl.Error(message);
            LogD.Error(message);
        }
    }
}
