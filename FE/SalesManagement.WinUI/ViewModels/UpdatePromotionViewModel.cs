using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.ViewModels;

public class UpdatePromotionViewModel : BaseViewModel
{
    // 🔥 Danh sách string cho ComboBox
    public List<string> DiscountTypes { get; } = new()
    {
        "FIXED_AMOUNT",
        "PERCENTAGE"
    };

    // Store promotion ID
    private int _promotionId;
    public int PromotionId
    {
        get => _promotionId;
        set => SetProperty(ref _promotionId, value);
    }

    // Promotion Name
    private string _promotionName;
    public string PromotionName
    {
        get => _promotionName;
        set => SetProperty(ref _promotionName, value);
    }

    // Promotion Code (Read-only khi update)
    private string _promotionCode;
    public string PromotionCode
    {
        get => _promotionCode;
        set => SetProperty(ref _promotionCode, value);
    }

    // Description
    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    // Discount Type
    private string _discountType;
    public string DiscountType
    {
        get => _discountType;
        set => SetProperty(ref _discountType, value);
    }

    // Discount Value
    private decimal _discountValue;
    public decimal DiscountValue
    {
        get => _discountValue;
        set => SetProperty(ref _discountValue, value);
    }

    // Min Order Value
    private double _minOrderValue;
    public double MinOrderValue
    {
        get => _minOrderValue;
        set => SetProperty(ref _minOrderValue, value);
    }

    // Max Discount Value
    private double _maxDiscountValue;
    public double MaxDiscountValue
    {
        get => _maxDiscountValue;
        set => SetProperty(ref _maxDiscountValue, value);
    }

    // Start Date
    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    // End Date
    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    // Usage Limit
    private int _usageLimit;
    public int UsageLimit
    {
        get => _usageLimit;
        set => SetProperty(ref _usageLimit, value);
    }

    // Is Active
    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    // Load data from PromotionResponse
    public void LoadFrom(PromotionResponse p)
    {
        PromotionId = (int)p.PromotionId;
        PromotionName = p.PromotionName;
        PromotionCode = p.PromotionCode;
        Description = p.Description;

        // Convert enum to string
        DiscountType = p.DiscountType.ToString();

        DiscountValue = p.DiscountValue;
        MinOrderValue = (double)p.MinOrderAmount;
        MaxDiscountValue = (double)p.MaxDiscountValue;
        StartDate = p.StartDate;
        EndDate = p.EndDate;
        UsageLimit = p.UsageLimit;
        IsActive = p.IsActive;
    }
}