using Microsoft.UI.Xaml.Controls;

namespace SalesManagement.WinUI.Views.Dialogs
{
    public sealed partial class ChangePasswordDialog : ContentDialog
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        public string OldPassword => OldPasswordBox.Password;
        public string NewPassword => NewPasswordBox.Password;
        public string ConfirmPassword => ConfirmPasswordBox.Password;
    }
}
