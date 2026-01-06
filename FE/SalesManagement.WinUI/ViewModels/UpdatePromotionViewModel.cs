using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.ViewModels;

public class UpdatePromotionViewModel : BaseViewModel
{
    // 🔥 Danh sách string
    public List<string> DiscountTypes { get; }
        = new()
        {
            "FIXED_AMOUNT",
            "PERCENTAGE"
        };

    private string _discountType;
    public string DiscountType
    {
        get => _discountType;
        set => SetProperty(ref _discountType, value);
    }

    public string PromotionName { get; set; }
    public string PromotionCode { get; set; }
    public string Description { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int UsageLimit { get; set; }

    public void LoadFrom(PromotionResponse p)
    {
        PromotionName = p.PromotionName;
        PromotionCode = p.PromotionCode;
        Description = p.Description;

        // 🔥 enum → string
        DiscountType = p.DiscountType.ToString();

        DiscountValue = p.DiscountValue;
        StartDate = p.StartDate;
        EndDate = p.EndDate;
        UsageLimit = p.UsageLimit;
    }
}
