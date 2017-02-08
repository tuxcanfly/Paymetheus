using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paymetheus.Framework
{
    public class ShellViewModelBase : ViewModelBase
    {
        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; RaisePropertyChanged(); }
        }

        private DialogViewModelBase _visibleDialogContent;
        public DialogViewModelBase VisibleDialogContent
        {
            get { return _visibleDialogContent; }
            set { _visibleDialogContent = value; RaisePropertyChanged(); }
        }

        public void ShowDialog(DialogViewModelBase dialog)
        {
            if (VisibleDialogContent != null)
            {
                // Do not allow multiple dialogs.  Keep the current one.
                return;
            }

            VisibleDialogContent = dialog;

            if (dialog is IActivity)
            {
                var activity = (IActivity)dialog;
                Task.Run(activity.RunActivityAsync).ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        var ex = t.Exception.InnerException;
                        MessageBox.Show(ex.Message, "Loading dialog failed");
                    }
                });
            }
        }

        public void HideDialog()
        {
            var dialog = VisibleDialogContent;
            if (dialog == null)
                return;

            if (dialog is INotifyCompletion)
            {
                var tcs = ((INotifyCompletion)dialog).NotifyCompletionSource;
                tcs.SetResult(null);
            }

            VisibleDialogContent = null;
        }
    }
}
