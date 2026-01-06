using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class UpdatePromotionRequest
    {
        [JsonPropertyName("promotionName")]
        public string PromotionName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("discountType")]
        public string DiscountType { get; set; } = "PERCENTAGE";

        [JsonPropertyName("discountValue")]
        public double DiscountValue { get; set; }

        [JsonPropertyName("minOrderValue")]
        public double MinOrderValue { get; set; }

        [JsonPropertyName("maxDiscountValue")]
        public double MaxDiscountValue { get; set; }

        [JsonPropertyName("usageLimit")]
        public int UsageLimit { get; set; }

        [JsonPropertyName("isActive")]
        public bool Active { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
