using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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
            // 1. Gắn Token
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // 2. Gọi API
                var response = await _client.GetAsync("/api/v1/promotions/search");

                // 3. Đọc Raw JSON (Giống ProductService)
                var rawJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[GET PROMOTIONS ERROR] {response.StatusCode} - {rawJson}");
                    return GetDefaultNonePromotion();
                }

                Debug.WriteLine(rawJson);

                // 4. Deserialize vào Wrapper (ApiResponse<PromotionListData>)
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<PromotionListData>>(rawJson, _jsonOptions);

                // 5. Kiểm tra Success và Data
                if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null || apiResponse.Data.Promotions == null)
                {
                    Debug.WriteLine("[GET PROMOTIONS] API returned success=false or null data");
                    return GetDefaultNonePromotion();
                }

                Debug.WriteLine($"[GET PROMOTIONS] Success. Count: {apiResponse.Data.Promotions.Count}");

                // 6. Mapping sang UI Model
                var uiPromotions = apiResponse.Data.Promotions
                    .Where(p => p.IsActive && IsDateValid(p.EndDate))
                    .Select(p => new Promotion
                    {
                        PromotionId = p.PromotionId,
                        PromotionCode = p.PromotionCode,
                        PromotionName = p.PromotionName,
                        Description = p.Description,
                        DiscountType = p.DiscountType,

                        // Logic này ĐÚNG: Nếu là % thì lấy value làm %, ngược lại % là 0
                        DiscountPercentage = p.DiscountType == "PERCENTAGE" ? (decimal)p.DiscountValue : 0,

                        // Value tiền mặt
                        DiscountValue = (decimal)p.DiscountValue,

                        // Giờ đây MinOrderAmount đã có dữ liệu từ JSON
                        MinOrderAmount = p.MinOrderAmount,
                        MaxDiscountValue = p.MaxDiscountValue > 0 ? p.MaxDiscountValue : 0,
                    }).ToList();

                // 7. Luôn thêm mục "Không áp dụng" vào đầu danh sách
                uiPromotions.Insert(0, new Promotion
                {
                    PromotionId = 0,
                    PromotionCode = "NONE",
                    PromotionName = "Không áp dụng",
                    DiscountType = "FIXED",
                    DiscountValue = 0,
                    MinOrderAmount = 0
                });

                foreach (var promo in uiPromotions)
                {
                    Debug.WriteLine($"[PROMOTION] ID: {promo.PromotionId}, Name: {promo.PromotionName}, Type: {promo.DiscountType}, Value: {promo.DiscountValue}, MinOrder: {promo.MinOrderAmount}");
                }
                return uiPromotions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GET PROMOTIONS EXCEPTION] {ex.Message}");
                return GetDefaultNonePromotion();
            }
        }

        // Hàm helper trả về list mặc định để tránh crash UI
        private List<Promotion> GetDefaultNonePromotion()
        {
            return new List<Promotion>
            {
                new Promotion { PromotionId = 0, PromotionName = "Không áp dụng" }
            };
        }

        private bool IsDateValid(object endDateObj)
        {
            // Vì Model PromotionResponse đã định nghĩa EndDate là DateTime, 
            // nên object này chắc chắn là DateTime sau khi Deserialize.
            if (endDateObj is DateTime dt)
            {
                // Sử dụng .Date để so sánh chỉ ngày, bỏ qua giờ phút 
                // (Để tránh trường hợp hết hạn vào 23:59 hôm nay nhưng so sánh với Now lại sai)
                return dt.Date >= DateTime.Now.Date;
            }

            return true;
        }
    }
}