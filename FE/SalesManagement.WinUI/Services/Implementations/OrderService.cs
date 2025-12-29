using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly HttpClient _client;
        private readonly IAuthService _authService;

        private readonly JsonSerializerOptions _jsonOptions;

        public OrderService(IHttpClientFactory httpClientFactory, IAuthService authService)
        {
            _client = httpClientFactory.CreateClient("API");
            _authService = authService;

            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // Cập nhật hàm này để xử lý Filter và Pagination
        public async Task<(List<Order> Items, int TotalCount, decimal TotalRevenue, int PendingCount)> GetOrdersAsync(
            int pageIndex, int pageSize, string status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // API dùng page index bắt đầu từ 0, UI dùng từ 1 -> trừ đi 1
            var apiPage = pageIndex > 0 ? pageIndex - 1 : 0;
            var url = $"/api/v1/orders?pageNumber={apiPage}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
            {
                url += $"&status={MapStatusToApi(status)}";
            }
            if (fromDate.HasValue) url += $"&fromDate={fromDate.Value:yyyy-MM-dd}";
            if (toDate.HasValue) url += $"&toDate={toDate.Value:yyyy-MM-dd}";

            Debug.WriteLine($"{url}");

            try
            {
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[API Error] {response.StatusCode}");
                    return (new List<Order>(), 0, 0, 0);
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                // 🔥 THAY ĐỔI Ở ĐÂY: Dùng class cụ thể OrderApiResponse
                var apiResponse = JsonSerializer.Deserialize<OrderApiResponse>(jsonString, _jsonOptions);

                if (apiResponse == null || apiResponse.Content == null)
                    return (new List<Order>(), 0, 0, 0);

                // Mapping dữ liệu API sang UI Model
                var uiOrders = apiResponse.Content.Select(o => new Order
                {
                    Id = o.OrderId,                 // ID thật (long)
                    OrderCode = o.OrderCode,        // Mã hiển thị (string)
                    Date = o.OrderDate,
                    Status = MapStatusToUI(o.Status),
                    ItemsCount = o.OrderItems?.Count ?? 0,
                    Amount = o.TotalAmount
                }).ToList();

                // Tính toán sơ bộ (nên lấy từ API thống kê riêng nếu có)
                decimal revenue = uiOrders.Sum(x => x.Amount);

                return (uiOrders, apiResponse.TotalElements, revenue, 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return (new List<Order>(), 0, 0, 0);
            }
        }



        public async Task<List<OrderDetail>> GetOrderDetailsAsync(string orderIdStr)
        {
            // 1. Validate and Parse ID
            // The API requires an integer ID (int64/long). The UI might be passing "ORD001" or "123".
            // If your UI is passing the numeric ID as a string, this parse will succeed.
            if (!long.TryParse(orderIdStr, out long orderId))
            {
                Debug.WriteLine($"[GetOrderDetailsAsync] Invalid Order ID format: {orderIdStr}. Expected a numeric ID.");
                return new List<OrderDetail>();
            }

            // 2. Attach Authorization Token
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // 3. Call API Endpoint: GET /api/v1/orders/{orderId}
                var response = await _client.GetAsync($"/api/v1/orders/{orderId}");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[GetOrderDetailsAsync] API Error: {response.StatusCode} for ID {orderId}");
                    return new List<OrderDetail>();
                }

                // 4. Deserialize Response
                // We use OrderResponse model which matches the JSON structure provided
                var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>(_jsonOptions);

                if (orderResponse == null || orderResponse.OrderItems == null)
                {
                    return new List<OrderDetail>();
                }

                // 5. Map to UI Model (OrderDetail)
                // We transform the API's OrderItem list into the simple OrderDetail list used by the UI
                var details = orderResponse.OrderItems.Select(item => new OrderDetail
                {
                    // Handle potential null product reference
                    ProductName = item.Product?.ProductName ?? "Unknown Product",
                    Quantity = item.Quantity,
                    // Use UnitPrice or TotalPrice based on what you want to display per row
                    Price = item.UnitPrice
                }).ToList();

                return details;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetOrderDetailsAsync] Exception: {ex.Message}");
                return new List<OrderDetail>();
            }
        }

        public async Task<bool> DeleteOrderAsync(string orderIdStr)
        {
            // 1. Validate and Parse ID
            // The UI passes the ID as a string, but the API path likely needs the numeric ID.
            if (!long.TryParse(orderIdStr, out long orderId))
            {
                Debug.WriteLine($"[DeleteOrderAsync] Invalid Order ID format: {orderIdStr}. Expected a numeric ID.");
                return false;
            }

            // 2. Attach Authorization Token
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // 3. Call API Endpoint: DELETE /api/v1/orders/{orderId}
                var response = await _client.DeleteAsync($"/api/v1/orders/{orderId}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[DeleteOrderAsync] Successfully deleted order {orderId}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DeleteOrderAsync] Failed to delete order {orderId}. Status: {response.StatusCode}. Error: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeleteOrderAsync] Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            Debug.WriteLine($"[UpdateOrderAsync] Success for Order");
            if (order.Id <= 0) return false;

            
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            
            var request = new UpdateOrderRequest
            {
                Status = MapStatusToApi(order.Status),
                PaymentMethod = "CASH_ON_DELIVERY", 
                OrderItems = new List<UpdateOrderItemRequest>()
            };

            try
            {
                
                var response = await _client.PutAsJsonAsync($"/api/v1/orders/{order.Id}", request, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[UpdateOrderAsync] Success for Order {order.Id}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[UpdateOrderAsync] Failed: {response.StatusCode} - {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateOrderAsync] Exception: {ex.Message}");
                return false;
            }
        }

        // Helper to map UI status back to API enum strings
        private string MapStatusToApi(string uiStatus)
        {
            return uiStatus switch
            {
                "Mới tạo" => "CREATED",
                "Đã thanh toán" => "PAID",
                "Đã hủy" => "CANCELLED",
                _ => "CREATED" 
            };
        }



        public async Task<bool> CreateOrderAsync(CreateOrderRequest request)
        {
            var token = _authService.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API endpoint thật
            var response = await _client.PostAsJsonAsync("/api/v1/orders", request);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("[CREATE ORDER] Success");
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[CREATE ORDER] Failed: {response.StatusCode} - {error}");
                return false;
            }
        }

        private string MapStatusToUI(string apiStatus)
        {
            return apiStatus switch
            {
                "CREATED" => "Mới tạo",
                "PAID" => "Đã thanh toán",
                "CANCELLED" => "Đã hủy",
                _ => apiStatus
            };
        }
    }
}