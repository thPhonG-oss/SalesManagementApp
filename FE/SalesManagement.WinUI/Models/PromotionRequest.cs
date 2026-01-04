using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Models
{
    public class PromotionRequest
    {

    }

    public class Promotion
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
        public string DiscountType { get; set; } = "PERCENTAGE"; // "PERCENTAGE" hoặc "FIXED"

        [JsonPropertyName("discountPercentage")]
        public decimal DiscountPercentage { get; set; } // Ví dụ: 10 (là 10%) hoặc 0.1

        [JsonPropertyName("discountValue")]
        public decimal DiscountValue { get; set; } // Số tiền giảm cụ thể

        [JsonPropertyName("minOrderAmount")]
        public decimal MinOrderAmount { get; set; }

        [JsonPropertyName("maxDiscountValue")]
        public decimal MaxDiscountValue { get; set; }

        [JsonPropertyName("startDate")]
        public string StartDate { get; set; } = string.Empty;

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        // Helper property để hiển thị lên ComboBox đẹp hơn
        [JsonIgnore]
        public string DisplayName => $"{PromotionCode} - {PromotionName}";
    }
}
