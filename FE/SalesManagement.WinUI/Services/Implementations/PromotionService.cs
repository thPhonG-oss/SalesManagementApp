using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _client;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public PromotionService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _client = httpClientFactory.CreateClient("API");
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Promotion>> GetActivePromotionsAsync()
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // Gọi API lấy danh sách khuyến mãi
                // Giả sử API trả về List<PromotionResponse> hoặc JSON array
                var response = await _client.GetAsync("/api/v1/promotions");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[GET PROMOTIONS] Error: {response.StatusCode}");
                    return new List<Promotion>();
                }

                // Tùy cấu trúc API trả về, nếu là List<PromotionResponse>
                // Bạn cần mapping sang Model Promotion dùng cho UI
                var promoResponses = await response.Content.ReadFromJsonAsync<List<PromotionResponse>>(_jsonOptions);

                if (promoResponses == null) return new List<Promotion>();

                // Mapping sang UI Model và thêm item "Không áp dụng"
                var uiPromotions = promoResponses
                    .Where(p => p.IsActive && p.EndDate >= DateTime.Now) // Lọc cơ bản: Đang chạy và chưa hết hạn
                    .Select(p => new Promotion
                    {
                        PromotionId = p.PromotionId,
                        PromotionCode = p.PromotionCode,
                        PromotionName = p.PromotionName,
                        Description = p.Description,
                        DiscountType = p.DiscountType,
                        DiscountPercentage = (decimal)p.DiscountPercentage,
                        DiscountValue = p.DiscountValue,
                        MinOrderAmount = p.MinOrderAmount,
                        MaxDiscountValue = p.MaxDiscountValue,
                        
                    }).ToList();

                // Luôn thêm lựa chọn "Không áp dụng" ở đầu
                uiPromotions.Insert(0, new Promotion
                {
                    PromotionId = 0,
                    PromotionCode = "NONE",
                    PromotionName = "Không áp dụng",
                    DiscountType = "FIXED",
                    DiscountValue = 0,
                    MinOrderAmount = 0
                });

                return uiPromotions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GET PROMOTIONS EXCEPTION] {ex.Message}");
                // Trả về ít nhất là item "Không áp dụng" để không crash UI
                return new List<Promotion>
                {
                    new Promotion { PromotionId = 0, PromotionName = "Không áp dụng (Lỗi mạng)" }
                };
            }
        }
    }
}