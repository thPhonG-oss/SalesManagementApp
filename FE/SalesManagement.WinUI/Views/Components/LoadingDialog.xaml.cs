using Microsoft.UI.Xaml.Controls;

namespace SalesManagement.WinUI.Views.Components;

public sealed partial class LoadingDialog : ContentDialog
{
    // Biến để hứng nội dung thông báo
    public string Message { get; set; }

    public LoadingDialog(string message = "Đang xử lý...")
    {
        this.InitializeComponent();
        this.Message = message;
    }
}