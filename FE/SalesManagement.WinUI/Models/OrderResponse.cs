using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    /// <summary>
    /// Class gốc phản hồi thông tin chi tiết đơn hàng
    /// </summary>
    public class OrderResponse
    {
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("subTotal")]
        public decimal SubTotal { get; set; }

        [JsonPropertyName("discountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;

        [JsonPropertyName("shippingAddress")]
        public string ShippingAddress { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // --- Nested Objects (Đã đổi tên type) ---

        [JsonPropertyName("customer")]
        public OrderCustomer Customer { get; set; } = new OrderCustomer();

        [JsonPropertyName("promotion")]
        public OrderPromotion Promotion { get; set; }  = new OrderPromotion();

        [JsonPropertyName("orderItems")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    /// <summary>
    /// Thông tin khách hàng gắn liền với đơn hàng này
    /// </summary>
    public class OrderCustomer
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; }  = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; }  = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("totalOrder")]
        public int TotalOrder { get; set; }

        [JsonPropertyName("totalSpent")]
        public decimal TotalSpent { get; set; }

        [JsonPropertyName("lastOrderDate")]
        public DateTime? LastOrderDate { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Thông tin khuyến mãi áp dụng cho đơn hàng
    /// </summary>
    public class OrderPromotion
    {
        [JsonPropertyName("promotionId")]
        public long PromotionId { get; set; }

        [JsonPropertyName("promotionCode")]
        public string PromotionCode { get; set; } = string.Empty;

        [JsonPropertyName("promotionName")]
        public string PromotionName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("discountType")]
        public string DiscountType { get; set; } = string.Empty;

        [JsonPropertyName("discountPercentage")]
        public double DiscountPercentage { get; set; }

        [JsonPropertyName("discountValue")]
        public decimal DiscountValue { get; set; }

        [JsonPropertyName("minOrderAmount")]
        public decimal MinOrderAmount { get; set; }

        [JsonPropertyName("maxDiscountValue")]
        public decimal MaxDiscountValue { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("usageLimit")]
        public int UsageLimit { get; set; }

        [JsonPropertyName("usedCount")]
        public int UsedCount { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Chi tiết từng dòng sản phẩm trong đơn hàng
    /// </summary>
    public class OrderItem
    {
        [JsonPropertyName("orderItemId")]
        public long OrderItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        // Tham chiếu đến OrderProduct thay vì ProductModel chung
        [JsonPropertyName("product")]
        public OrderProduct Product { get; set; } = new OrderProduct();
    }

    /// <summary>
    /// Sản phẩm cụ thể trong ngữ cảnh của đơn hàng
    /// </summary>
    public class OrderProduct
    {
        [JsonPropertyName("productId")]
        public long ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; } = string.Empty;

        [JsonPropertyName("publicationYear")]
        public int PublicationYear { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }

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

        [JsonPropertyName("specialPrice")]
        public decimal SpecialPrice { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Tham chiếu đến OrderCategory
        [JsonPropertyName("category")]
        public OrderCategory Category { get; set; } = new OrderCategory();

        // Tham chiếu đến OrderProductImage
        [JsonPropertyName("images")]
        public List<OrderProductImage> Images { get; set; } = new List<OrderProductImage>();
    }

    /// <summary>
    /// Danh mục sản phẩm (trong view đơn hàng)
    /// </summary>
    public class OrderCategory
    {
        [JsonPropertyName("categoryId")]
        public long CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Hình ảnh sản phẩm
    /// </summary>
    public class OrderProductImage
    {
        [JsonPropertyName("imageId")]
        public long ImageId { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }



    public class OrderApiResponse
    {
        [JsonPropertyName("content")]
        public List<OrderResponse> Content { get; set; } = new();

        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("number")]
        public int PageNumber { get; set; }
    }

}