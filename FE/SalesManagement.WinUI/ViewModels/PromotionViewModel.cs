using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesManagement.WinUI.ViewModels
{
    public class PromotionViewModel : INotifyPropertyChanged
    {
        private readonly IPromotionService _promotionService;

        public ObservableCollection<Promotion> Promotions { get; } = new();

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

            var promotions = await _promotionService.GetActivePromotionsAsync();
            foreach (var promo in promotions)
            {
                Promotions.Add(promo);
            }

            IsLoading = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
