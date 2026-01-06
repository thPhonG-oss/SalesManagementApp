using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IUserService
    {
        Task ChangePasswordAsync(ChangePasswordRequest request);
    }
}
