using SalesManagement.WinUI.Models;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetActivePromotionsAsync();
        Task<List<PromotionResponse>> GetAllPromotionsAsync(
            int page = 0,
            int size = 100,
            string sortBy = "createdAt",
            string sortDir = "desc");
        Task<bool> CreatePromotionAsync(CreatePromotionRequest request);
    }
}
