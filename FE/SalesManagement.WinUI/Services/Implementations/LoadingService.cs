using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SalesManagement.WinUI.Services.Interfaces;
using SalesManagement.WinUI.Views.Components;

public class LoadingService : ILoadingService
{
    private XamlRoot? _xamlRoot;
    private ContentDialog? _activeDialog;

    public void SetXamlRoot(XamlRoot root) => _xamlRoot = root;

   
    public Task ShowAsync(string message = "Đang tải...")
    {
        if (_xamlRoot == null) return Task.CompletedTask;
        if (_activeDialog != null) return Task.CompletedTask;

        _activeDialog = new LoadingDialog(message)
        {
            XamlRoot = _xamlRoot
        };


        _ = _activeDialog.ShowAsync();

        return Task.CompletedTask;
    }

    public void Hide()
    {
        if (_activeDialog != null)
        {
            _activeDialog.Hide();
            _activeDialog = null;
        }
    }
}