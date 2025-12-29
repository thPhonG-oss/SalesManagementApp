// SalesManagement.WinUI/Services/Interfaces/IDialogService.cs
using System.Threading.Tasks;
using SalesManagement.WinUI.ViewModels;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IDialogService
    {
        
        void SetXamlRoot(Microsoft.UI.Xaml.XamlRoot xamlRoot);

        
        Task<bool> ShowConfirmationAsync(string title, string message, string primaryText = "Có", string closeText = "Không");

        
        Task<bool> ShowEditOrderDialogAsync(OrderItemViewModel orderVm);

        Task ShowOrderDetailDialogAsync(OrderItemViewModel orderVm);

        Task<bool> ShowCreateOrderDialogAsync();
    }
}