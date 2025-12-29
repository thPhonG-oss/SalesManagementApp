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
        Task<(List<Order> Items, int TotalCount, decimal TotalRevenue, int PendingCount)> GetOrdersAsync(
            int pageIndex,
            int pageSize,
            string status = "",
            DateTime? fromDate = null,
            DateTime? toDate = null);
        Task<List<OrderDetail>> GetOrderDetailsAsync(string orderId);
        Task<bool> DeleteOrderAsync(string orderId);
        Task<bool> UpdateOrderAsync(Order order);

        Task<bool> CreateOrderAsync(CreateOrderRequest request);

        Task<bool> PrintOrderAsync (string orderId);
    }
}
