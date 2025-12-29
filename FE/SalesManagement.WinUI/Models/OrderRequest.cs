using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class CreateOrderRequest
    {
      
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("promotionId")]
        public long? PromotionId { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;

        [JsonPropertyName("shippingAddress")]
        public string ShippingAddress { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = "CASH_ON_DELIVERY";

        [JsonPropertyName("orderItems")]
        public List<CreateOrderItemRequest> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemRequest
    {
        [JsonPropertyName("productId")]
        public long ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }
    }

    public class UpdateOrderRequest
    {
        [JsonPropertyName("promotionId")]
        public long? PromotionId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("shippingAddress")]
        public string ShippingAddress { get; set; } = string.Empty;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        [JsonPropertyName("orderItems")]
        public List<UpdateOrderItemRequest> OrderItems { get; set; } = new();
    }

    public class UpdateOrderItemRequest
    {
        [JsonPropertyName("orderItemId")]
        public long OrderItemId { get; set; } // 0 for new items

        [JsonPropertyName("productId")]
        public long ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}