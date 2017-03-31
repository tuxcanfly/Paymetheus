using System.Windows;
using System.Windows.Controls;

namespace Paymetheus
{
    /// <summary>
    /// Interaction logic for AutoBuyerDialog.xaml
    /// </summary>
    public partial class AutoBuyerDialog: UserControl
    {
        public AutoBuyerDialog()
        {
            InitializeComponent();
        }

        private void PasswordBoxPrivatePassphrase_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).PrivatePassphrase = ((PasswordBox)sender).Password;
            }
        }
    }
}
