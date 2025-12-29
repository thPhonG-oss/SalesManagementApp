using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class CreateOrderRequest
    {
        [JsonPropertyName("customerId")]
        public long CustomerId { get; set; }

        [JsonPropertyName("promotionId")]
        public long? PromotionId { get; set; } 

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
        public string PaymentMethod { get; set; } = "CASH"; 

        [JsonPropertyName("orderItems")]
        public List<CreateOrderItemRequest> OrderItems { get; set; } = new List<CreateOrderItemRequest>();
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