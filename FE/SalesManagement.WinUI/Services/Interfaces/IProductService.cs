using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductListData?> GetProductsAsync(
            int page = 1,
            int size = 200,
            string sortBy = "productName",
            string sortDir = "asc");
        Task<bool> CreateProductAsync(CreateProductRequest request);

    }
}
