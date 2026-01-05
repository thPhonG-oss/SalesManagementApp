using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SalesManagement.WinUI.Models;


namespace SalesManagement.WinUI.Models
{
    public class DashboardResponse
    {
        [JsonPropertyName("totalProducts")]
        public long TotalProducts { get; set; }

        [JsonPropertyName("lowStockProducts")]
        public List<ProductResponse> LowStockProducts { get; set; } = new();

        [JsonPropertyName("topSellingProducts")]
        public List<ProductResponse> TopSellingProducts { get; set; } = new();

        [JsonPropertyName("todayOrderCount")]
        public long TodayOrderCount { get; set; }

        [JsonPropertyName("todayRevenue")]
        public double TodayRevenue { get; set; }

        [JsonPropertyName("recentOrders")]
        public List<OrderResponse> RecentOrders { get; set; } = new();

        [JsonPropertyName("dailyRevenue")]
        public List<DailyRevenueResponse> DailyRevenue { get; set; } = new();
    }

    public class ProductResponse
    {
        [JsonPropertyName("productId")]
        public long Id { get; set; }

        [JsonPropertyName("productName")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("stockQuantity")]
        public long Quantity { get; set; }

        [JsonPropertyName("category")]
        public CategoryResponse Category { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("publicationYear")]
        public int PublicationYear { get; set; }

        [JsonPropertyName("minStockQuantity")]
        public int MinStockQuantity { get; set; }

        [JsonPropertyName("soldQuantity")]
        public int SoldQuantity { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("discountPercentage")]
        public double DiscountPercentage { get; set; }

        [JsonPropertyName("isDiscounted")]
        public bool IsDiscounted { get; set; }
    }

    public class CategoryResponse
    {
        [JsonPropertyName("categoryId")]
        public long Id { get; set; }

        [JsonPropertyName("categoryName")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

  



    public class OrderItemResponse
    {
        [JsonPropertyName("orderItemId")]
        public long Id { get; set; }

        [JsonPropertyName("product")]
        public ProductResponse Product { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public double UnitPrice { get; set; }

        [JsonPropertyName("totalPrice")]
        public double TotalPrice { get; set; }
    }

    public class DailyRevenueResponse
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("revenue")]
        public double Revenue { get; set; }
    }
}