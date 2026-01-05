using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class CreatePromotionRequest
    {
        [JsonPropertyName("promotionName")]
        public string PromotionName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("discountType")]
        public string DiscountType { get; set; } = "PERCENTAGE";

        [JsonPropertyName("discountValue")]
        public decimal DiscountValue { get; set; }

        [JsonPropertyName("minOrderValue")]
        public decimal MinOrderValue { get; set; }

        [JsonPropertyName("maxDiscountValue")]
        public decimal MaxDiscountValue { get; set; }

        // 👇 QUAN TRỌNG
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; } = string.Empty;

        [JsonPropertyName("endDate")]
        public string EndDate { get; set; } = string.Empty;

        [JsonPropertyName("usageLimit")]
        public int UsageLimit { get; set; }
    }
}
