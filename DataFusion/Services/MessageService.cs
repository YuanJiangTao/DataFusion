using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;

namespace DataFusion.Services
{
    public class MessageService
    {
        public MessageService()
        {

        }

        public void Warnging(string message)
        {
            Growl.Warning(message);
        }
    }
}
