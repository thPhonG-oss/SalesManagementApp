using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesManagement.WinUI.ViewModels
{
    public class AddPromotionViewModel : INotifyPropertyChanged
    {
        private readonly IPromotionService _promotionService;

        public AddPromotionViewModel(IPromotionService promotionService)
        {
            _promotionService = promotionService;

            DiscountTypes = new ObservableCollection<string>
            {
                "PERCENTAGE",
                "FIXED_AMOUNT"
            };

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        public ObservableCollection<string> DiscountTypes { get; }

        public string PromotionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SelectedDiscountType { get; set; } = "PERCENTAGE";

        public double DiscountValue { get; set; }
        public double MinOrderValue { get; set; }
        public double MaxDiscountValue { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UsageLimit { get; set; }

        public async Task<bool> SaveAsync()
        {



            var request = new CreatePromotionRequest
            {
                PromotionName = PromotionName,
                Description = Description,
                DiscountType = SelectedDiscountType,
                DiscountValue = (decimal)DiscountValue,
                MinOrderValue = (decimal)MinOrderValue,
                MaxDiscountValue = (decimal)MaxDiscountValue,

                // 👇 FORMAT CHUẨN LocalDate
                StartDate = StartDate.ToString("yyyy-MM-dd"),
                EndDate = EndDate.ToString("yyyy-MM-dd"),

                UsageLimit = UsageLimit
            };


            return await _promotionService.CreatePromotionAsync(request);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
