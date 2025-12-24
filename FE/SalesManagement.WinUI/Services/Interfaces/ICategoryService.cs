using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
    }
}