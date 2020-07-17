using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DataFusion.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DataFusion.ViewModel
{
    public class PluginAddViewModel : ViewModelBase, IDataErrorInfo
    {

        public PluginAddViewModel(Action<PluginAddViewModel> closeHandler)
        {
            CloseCommand = new SimpleCommand(CanSaveItem, o =>
              {
                  DialogResult = true;
                  closeHandler(this);
              });
            CancelCommand = new SimpleCommand(o => true, o =>
              {
                  DialogResult = false;
                  closeHandler(this);
              });
            //CloseCommand = new RelayCommand(() =>
            //{
            //    DialogResult = true;
            //    closeHandler(this);
            //}, CanSaveItem, true);

            //CancelCommand = new RelayCommand(() =>
            //{
            //    DialogResult = false;
            //    closeHandler(this);
            //}, true);
        }

        private string _mineName = "默认名称";
        public string MineName
        {
            get => _mineName;
            set
            {
                _mineName = value;
                RaisePropertyChanged();
            }
        }
        private string _mineCode = "0123456789";
        public string MineCode
        {
            get => _mineCode;
            set
            {
                _mineCode = value;
                RaisePropertyChanged();
            }
        }

        private bool CanSaveItem()
        {
            return _propertyErrorMessage.Values.All(p => string.IsNullOrEmpty(p));
        }
        private bool CanSaveItem(object obj)
        {
            return _propertyErrorMessage.Values.All(p => string.IsNullOrEmpty(p));
        }

        public ICommand CloseCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public bool DialogResult { get; set; } = false;


        private Dictionary<string, string> _propertyErrorMessage = new Dictionary<string, string>();

        public string this[string propertyName]
        {
            get
           {
                string validationMessage = string.Empty;
                switch (propertyName)
                {
                    case "MineCode":
                        {
                            if (string.IsNullOrEmpty(MineCode))
                            {
                                validationMessage = "煤矿编号不能为空.";
                            }
                        }
                        break;
                    case "MineName":
                        {
                            if (string.IsNullOrEmpty(MineName))
                            {
                                validationMessage = "煤矿名称不能为空.";
                            }
                        }
                        break;
                }
                if (_propertyErrorMessage.ContainsKey(propertyName))
                {
                    _propertyErrorMessage[propertyName] = validationMessage;
                }
                else
                {
                    _propertyErrorMessage.Add(propertyName, validationMessage);
                }
                CommandManager.InvalidateRequerySuggested();
                Error = validationMessage;
                return validationMessage;
            }

        }
        private Dictionary<string, string> dataErrors = new Dictionary<string, string>();
        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                //((RelayCommand)CloseCommand).RaiseCanExecuteChanged();
                if (value == _error) return;
                _error = value;
                RaisePropertyChanged();
            }
        }

    }

    public class SimpleCommand : ICommand
    {
        public SimpleCommand(Func<object, bool> canExecute = null, Action<object> execute = null)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute;
        }

        public Func<object, bool> CanExecuteDelegate { get; set; }

        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            var canExecute = this.CanExecuteDelegate;
            return canExecute == null || canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            this.ExecuteDelegate?.Invoke(parameter);
        }
    }
}
