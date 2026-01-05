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

                        // Xử lý logic DiscountValue/Percentage
                        DiscountPercentage = p.DiscountType == "PERCENTAGE" ? (decimal)p.DiscountValue : 0,
                        DiscountValue = (decimal)p.DiscountValue,

                        // Nếu API không trả về MinOrderAmount thì mặc định là 0
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
            if (endDateObj is DateTime dt) return dt >= DateTime.Now;
            if (endDateObj is string dateStr && DateTime.TryParse(dateStr, out var parsedDate))
            {
                return parsedDate >= DateTime.Now;
            }

            return true;
        }

        public async Task<List<PromotionResponse>> GetAllPromotionsAsync(
            int page = 0,
            int size = 100,
            string sortBy = "createdAt",
            string sortDir = "desc")
        {
            // 1. Gắn token
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // 2. Build query string đúng swagger
                var url =
                    $"/api/v1/promotions?page={page}&size={size}&sortBy={sortBy}&sortDir={sortDir}";

                var response = await _client.GetAsync(url);
                var rawJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[GET ALL PROMOTIONS ERROR] {response.StatusCode} - {rawJson}");
                    return new List<PromotionResponse>();
                }

                // 3. Deserialize wrapper
                var apiResponse =
                    JsonSerializer.Deserialize<ApiResponse<PromotionListData>>(rawJson, _jsonOptions);

                if (apiResponse == null || !apiResponse.Success || apiResponse.Data?.Promotions == null)
                {
                    Debug.WriteLine("[GET ALL PROMOTIONS] Invalid API response");
                    return new List<PromotionResponse>();
                }

                Debug.WriteLine($"[GET ALL PROMOTIONS] Count: {apiResponse.Data.Promotions.Count}");

                // 4. Mapping sang UI model
                return apiResponse.Data.Promotions.Select(p => new PromotionResponse
                {
                    PromotionId = p.PromotionId,
                    PromotionCode = p.PromotionCode,
                    PromotionName = p.PromotionName,
                    Description = p.Description,

                    DiscountType = p.DiscountType,

                    DiscountPercentage = p.DiscountPercentage,
                    DiscountValue = p.DiscountValue,

                    MinOrderAmount = p.MinOrderAmount,
                    MaxDiscountValue = p.MaxDiscountValue,

                    StartDate = p.StartDate,
                    EndDate = p.EndDate,

                    UsageLimit = p.UsageLimit,
                    UsedCount = p.UsedCount,

                    IsActive = p.IsActive,

                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GET ALL PROMOTIONS EXCEPTION] {ex.Message}");
                return new List<PromotionResponse>();
            }
        }

        public async Task<bool> CreatePromotionAsync(CreatePromotionRequest request)
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("/api/v1/promotions", content);
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[CREATE PROMOTION ERROR] {response.StatusCode} - {raw}");
                    return false;
                }

                Debug.WriteLine("[CREATE PROMOTION] Success");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CREATE PROMOTION EXCEPTION] {ex.Message}");
                return false;
            }
        }

    }
}