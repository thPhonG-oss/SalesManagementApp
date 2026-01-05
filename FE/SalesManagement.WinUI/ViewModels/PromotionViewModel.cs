using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SalesManagement.WinUI.ViewModels
{
    public class PromotionViewModel : INotifyPropertyChanged
    {
        private readonly IPromotionService _promotionService;

        public ObservableCollection<PromotionResponse> Promotions { get; } = new();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public PromotionViewModel(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public async Task LoadPromotionsAsync()
        {
            IsLoading = true;
            Promotions.Clear();

            var promotions = await _promotionService.GetAllPromotionsAsync();
            foreach (var promo in promotions)
            {
                Promotions.Add(promo);
                Debug.WriteLine(promo.DiscountValue);
                Debug.WriteLine(promo.FormattedDiscount);

            }

            IsLoading = false;
        }

        public async Task<bool> DeactivatePromotionAsync(long promotionId)
        {
            return await _promotionService.DeactivatePromotionAsync(promotionId);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
