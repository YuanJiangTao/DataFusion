using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.ViewModel
{
    public class ToastErrorMsg
    {
        public Exception Exception { get; private set; }

        public string ErrorMessage { get; private set; }

        public ToastErrorMsg(string errorMessage, Exception exception)
        {
            this.ErrorMessage = errorMessage;
            this.Exception = exception;
        }

        public override string ToString()
        {
            return $"{ErrorMessage}\r\n{Exception}";
        }
    }
}
