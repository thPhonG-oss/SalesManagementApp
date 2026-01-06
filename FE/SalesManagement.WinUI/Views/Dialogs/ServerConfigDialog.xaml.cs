using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces;

namespace SalesManagement.WinUI.Views.Dialogs;

public sealed partial class ServerConfigDialog : ContentDialog
{
    private readonly IAppSettingsService _appSettingsService;

    public ServerConfigDialog(IAppSettingsService appSettingsService)
    {
        InitializeComponent();
        _appSettingsService = appSettingsService;

        // load port hiện tại
        PortTextBox.Text = _appSettingsService.GetApiPort().ToString();
    }

    private void OnPrimaryButtonClick(
            ContentDialog sender,
            ContentDialogButtonClickEventArgs args)
    {
        if (!int.TryParse(PortTextBox.Text, out var port) || port <= 0)
        {
            ErrorBar.IsOpen = true;
            args.Cancel = true; // ❗ Ngăn dialog đóng
            return;
        }

        _appSettingsService.UpdateApiPort(port);
    }
}
