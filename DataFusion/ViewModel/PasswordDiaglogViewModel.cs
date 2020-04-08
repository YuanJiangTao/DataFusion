using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.ViewModel
{
    public class PasswordDiaglogViewModel : ViewModelBase, IDialogResultable<string>
    {
        public PasswordDiaglogViewModel()
        {

        }
        public Action CloseAction { get; set; }

        public RelayCommand CloseCmd => new Lazy<RelayCommand>(() => new RelayCommand(() => CloseAction?.Invoke())).Value;
        public RelayCommand ConfirmCmd => new Lazy<RelayCommand>(() => new RelayCommand(() => CloseAction?.Invoke())).Value;

        private string _message;
        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }


        private string _result;
        public string Result
        {
            get => _result;
            set => Set(ref _result, value);
        }
    }
}
