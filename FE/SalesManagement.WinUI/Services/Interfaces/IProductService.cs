using SalesManagement.WinUI.Models;
using Windows.Storage;

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
        Task<bool> DeleteProductAsync(int productId);

        // ✅ Upload file ảnh trực tiếp dạng multipart/form-data
        Task<bool> UploadImageAsync(int productId, StorageFile file);

        Task<bool> UpdateProductAsync(int productId, Product product);

        // Upload file excel
        Task<bool> ImportProductsFromExcelAsync(StorageFile file);
    }
}
