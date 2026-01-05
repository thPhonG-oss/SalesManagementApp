using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class PromotionResponse
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

        public DateOnly NewStartDate => DateOnly.FromDateTime(StartDate);
        public DateOnly NewEndDate => DateOnly.FromDateTime(EndDate);

        public string FormattedDiscount
        {
            get
            {
                return DiscountType.ToLower() == "percentage"
                    ? $"{DiscountPercentage}%"
                    : DiscountValue.ToString("C2");
            }
        }

        public string FormattedDateRange
        {
            get
            {
                return $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";
            }
        }

        public string FormattedDiscountRange
        {
            get
            {
                string minOrder = MinOrderAmount.ToString("C2");
                string maxDiscount = MaxDiscountValue.ToString("C2");
                return $"Min Order: {minOrder}, Max Discount: {maxDiscount}";
            }
        }

        public string StatusText => IsActive ? "active" : "inactive";

        public bool statusBool => IsActive;

        public string StatusColor => IsActive ? "Green" : "Red";


    }

    public class PromotionListData
    {
        // Tên property phải khớp với JSON (case-insensitive)
        public List<PromotionResponse> Promotions { get; set; } = new List<PromotionResponse>();
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
    }
}