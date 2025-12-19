using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class MockOrderService : IOrderService
    {
        public async Task<List<Order>> GetOrdersAsync()
        {
            // Giả lập delay mạng
            await Task.Delay(5000);

            return new List<Order>
            {
                new Order { OrderId = "ORD001", Date = new DateTime(2025,1,25), ItemsCount = 2, Amount = 1250000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD002", Date = new DateTime(2025,1,24), ItemsCount = 5, Amount = 3450000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD003", Date = new DateTime(2025,1,25), ItemsCount = 1, Amount = 1150000, Status = "Đã hủy" },
                new Order { OrderId = "ORD004", Date = new DateTime(2025,1,23), ItemsCount = 3, Amount = 2100000, Status = "Mới tạo" },
                new Order { OrderId = "ORD005", Date = new DateTime(2025,1,23), ItemsCount = 4, Amount = 5200000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD006", Date = new DateTime(2025,1,22), ItemsCount = 2, Amount = 890000, Status = "Mới tạo" },
                new Order { OrderId = "ORD007", Date = new DateTime(2025,1,22), ItemsCount = 6, Amount = 4750000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD008", Date = new DateTime(2025,1,21), ItemsCount = 1, Amount = 650000, Status = "Đã hủy" },
               
                new Order { OrderId = "ORD009", Date = new DateTime(2025,1,21), ItemsCount = 3, Amount = 2890000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD010", Date = new DateTime(2025,1,20), ItemsCount = 7, Amount = 6300000, Status = "Mới tạo" }
            };
        }


    }
}
