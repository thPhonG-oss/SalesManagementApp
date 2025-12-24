using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SalesManagement.WinUI.ViewModels
{
    public class DetailItemViewModel : INotifyPropertyChanged
    {
        private readonly IProductService _productService;

        public Product Product { get; }

        public DetailItemViewModel(
            Product product,
            IProductService productService)
        {
            Product = product;
            _productService = productService;
        }

        public async Task<bool> DeleteAsync()
        {
            if (Product == null)
                return false;

            try
            {
                Debug.WriteLine("Deleting product: " + Product.ProductId);
                return await _productService.DeleteProductAsync(Product.ProductId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAsync error: " + ex.Message);
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
