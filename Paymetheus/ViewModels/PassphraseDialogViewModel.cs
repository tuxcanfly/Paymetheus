// Copyright (c) 2016 The btcsuite developers
// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Paymetheus.Framework;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paymetheus.ViewModels
{
    sealed class PassphraseDialogViewModel : DialogViewModelBase
    {
        public PassphraseDialogViewModel(ShellViewModel shell, string header, string buttonText,
            Func<string, Task<bool>> executeWithPassphrase, Func<Task> cancel=null, string notice="")
            : base(shell)
        {
            Header = header;
            Notice = notice;
            if (Notice != "") NoticeVisibility = Visibility.Visible;
            ExecuteText = buttonText;
            _execute = executeWithPassphrase;
            _cancel = cancel;
            Execute = new DelegateCommandAsync(ExecuteAction);
            Cancel = new DelegateCommandAsync(CancelAction);
        }

        private Func<string, Task<bool>> _execute;
        private Func<Task> _cancel;

        public string Header { get; }
        public string ExecuteText { get; }
        public string Passphrase { private get; set; } = "";
        public string Notice { get; }

        private Visibility _noticeVisibility = Visibility.Collapsed;
        public Visibility NoticeVisibility
        {
            get { return _noticeVisibility; }
            set { _noticeVisibility = value; RaisePropertyChanged(); }
        }

        public ICommand Execute { get; }
        public ICommand Cancel { get; }

        private async Task ExecuteAction()
        {
            try
            {
                var success = await _execute(Passphrase);
                if (success)
                    HideDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task CancelAction()
        {
            if (_cancel != null)
                await _cancel();
            HideDialog();
        }
    }
}
