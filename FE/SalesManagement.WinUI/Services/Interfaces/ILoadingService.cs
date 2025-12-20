using Microsoft.UI.Xaml; // <--- Cần dòng này để hiểu XamlRoot
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces;

public interface ILoadingService
{
    // Thêm dòng này vào interface
    void SetXamlRoot(XamlRoot root);

    Task ShowAsync(string message = "Đang tải...");
    void Hide();
}