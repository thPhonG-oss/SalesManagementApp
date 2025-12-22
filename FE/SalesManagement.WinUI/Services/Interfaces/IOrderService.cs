using SalesManagement.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<List<OrderDetail>> GetOrderDetailsAsync(string orderId);
        Task<bool> DeleteOrderAsync(string orderId);
        Task<bool> UpdateOrderAsync(Order order);

        Task<List<Product>> GetProductsAsync();
        Task<bool> CreateOrderAsync(Order order, List<OrderDetail> details);
    }
}
