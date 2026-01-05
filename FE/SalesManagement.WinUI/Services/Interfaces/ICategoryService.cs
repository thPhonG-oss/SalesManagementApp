using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<bool> UpdateAsync(int id, UpdateCategoryRequest request);
        Task<bool> CreateAsync(CreateCategoryRequest request);
        Task<bool> DeleteAsync(int id);
    }
}