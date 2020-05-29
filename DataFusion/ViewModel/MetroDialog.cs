using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;


namespace DataFusion.ViewModel
{
    public class MetroDialog
    {
        private MainViewModel _mainVm;
        private static MetroDialog _instance;

        public MetroDialog(MainViewModel mainVm)
        {
            this._mainVm = mainVm;
            _instance = this;
        }

        private IDialogCoordinator Dialog
        {
            get
            {
                return _mainVm.Dialog;
            }
        }

        public async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await Dialog.ShowMessageAsync(_mainVm, title, message, style);
        }

        public async Task ShowTipsAsync(string message)
        {
            await Dialog.ShowMessageAsync(_mainVm, "提示", message);
        }

        public async Task HideMetroDialogAsync(BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            await Dialog.HideMetroDialogAsync(_mainVm, dialog, settings);
        }

       public async Task ShowMetroDialogAsync(BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            await Dialog.ShowMetroDialogAsync(_mainVm, dialog, settings);
        }


        public static async Task<MessageDialogResult> StaticShowMessageAsync(string title,
            string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await _instance.ShowMessageAsync(title, message, style);
        }

        public static async Task StaticShowTipsAsync(string message)
        {
            await _instance.ShowTipsAsync(message);
        }
        public static  async Task<T> ShowCustomDialog<T, TControl>(string title, Func<Action<object>, T> newTFunc)
           where TControl : new()
        {
            var customDialog = new CustomDialog() { Title = title };

            var vm = newTFunc(async instance => 
            {
                await _instance.HideMetroDialogAsync(customDialog);
            });

            dynamic userControl = new TControl();
            userControl.DataContext = vm;
            customDialog.Content = userControl;
            await _instance.ShowMetroDialogAsync(customDialog);
            await customDialog.WaitUntilUnloadedAsync();
            return vm;
        }
    }
}
