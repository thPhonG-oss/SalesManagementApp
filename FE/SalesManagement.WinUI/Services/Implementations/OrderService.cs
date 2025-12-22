using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class MockOrderService : IOrderService
    {
        // API 1: Giả lập lấy danh sách đơn hàng (Nhẹ, không có chi tiết sản phẩm)
        public async Task<List<Order>> GetOrdersAsync()
        {
            await Task.Delay(500); // Giả lập mạng chậm

            return new List<Order>
            {
                new Order { OrderId = "ORD001", Date = new DateTime(2025,1,25), ItemsCount = 2, Amount = 1250000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD002", Date = new DateTime(2025,1,24), ItemsCount = 2, Amount = 1650000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD003", Date = new DateTime(2025,1,25), ItemsCount = 1, Amount = 1800000, Status = "Đã hủy" },

                // Thêm 22 đơn hàng mẫu
                new Order { OrderId = "ORD004", Date = new DateTime(2025,1,23), ItemsCount = 3, Amount = 2100000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD005", Date = new DateTime(2025,1,23), ItemsCount = 4, Amount = 5200000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD006", Date = new DateTime(2025,1,22), ItemsCount = 2, Amount = 890000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD007", Date = new DateTime(2025,1,22), ItemsCount = 6, Amount = 4750000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD008", Date = new DateTime(2025,1,21), ItemsCount = 1, Amount = 650000, Status = "Đã hủy" },
                new Order { OrderId = "ORD009", Date = new DateTime(2025,1,21), ItemsCount = 3, Amount = 2890000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD010", Date = new DateTime(2025,1,20), ItemsCount = 7, Amount = 6300000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD011", Date = new DateTime(2025,1,20), ItemsCount = 2, Amount = 1420000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD012", Date = new DateTime(2025,1,19), ItemsCount = 4, Amount = 3180000, Status = "Đã hủy" },
                new Order { OrderId = "ORD013", Date = new DateTime(2025,1,19), ItemsCount = 5, Amount = 4500000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD014", Date = new DateTime(2025,1,18), ItemsCount = 3, Amount = 2650000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD015", Date = new DateTime(2025,1,18), ItemsCount = 8, Amount = 7890000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD016", Date = new DateTime(2025,1,17), ItemsCount = 2, Amount = 1100000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD017", Date = new DateTime(2025,1,17), ItemsCount = 1, Amount = 520000, Status = "Đã hủy" },
                new Order { OrderId = "ORD018", Date = new DateTime(2025,1,16), ItemsCount = 6, Amount = 5340000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD019", Date = new DateTime(2025,1,16), ItemsCount = 4, Amount = 3720000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD020", Date = new DateTime(2025,1,15), ItemsCount = 3, Amount = 2250000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD021", Date = new DateTime(2025,1,15), ItemsCount = 5, Amount = 4980000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD022", Date = new DateTime(2025,1,14), ItemsCount = 2, Amount = 760000, Status = "Đã hủy" },
                new Order { OrderId = "ORD023", Date = new DateTime(2025,1,14), ItemsCount = 4, Amount = 3400000, Status = "Đã thanh toán" },
                new Order { OrderId = "ORD024", Date = new DateTime(2025,1,13), ItemsCount = 1, Amount = 420000, Status = "Đang xử lý" },
                new Order { OrderId = "ORD025", Date = new DateTime(2025,1,12), ItemsCount = 7, Amount = 6890000, Status = "Đã thanh toán" }
            };
        }

        
        public async Task<List<OrderDetail>> GetOrderDetailsAsync(string orderId)
        {
            await Task.Delay(500); 

            
            return orderId switch
            {
                "ORD001" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Laptop Dell XPS 13", Quantity = 1, Price = 1000000 },
                    new OrderDetail { ProductName = "Chuột Logitech", Quantity = 1, Price = 250000 }
                },
                "ORD002" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Màn hình LG 24 inch", Quantity = 1, Price = 1500000 },
                    new OrderDetail { ProductName = "Cáp HDMI", Quantity = 1, Price = 150000 }
                },
                "ORD003" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Bàn phím cơ Keychron", Quantity = 1, Price = 1800000 }
                },
                "ORD004" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Ổ cứng SSD 1TB", Quantity = 1, Price = 1200000 },
                    new OrderDetail { ProductName = "RAM 8GB", Quantity = 1, Price = 400000 }
                },
                "ORD005" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Tai nghe Sony", Quantity = 2, Price = 900000 }
                },
                "ORD006" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "USB 32GB", Quantity = 5, Price = 200000 }
                },
                "ORD007" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Laptop HP Envy", Quantity = 1, Price = 1200000 },
                    new OrderDetail { ProductName = "Chuột không dây", Quantity = 1, Price = 200000 }
                },
                "ORD008" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Adapter sạc", Quantity = 1, Price = 150000 }
                },
                "ORD009" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Màn hình Samsung 27 inch", Quantity = 1, Price = 3500000 }
                },
                "ORD010" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "RAM 16GB", Quantity = 2, Price = 800000 }
                },
                "ORD011" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "HDD 2TB", Quantity = 1, Price = 900000 }
                },
                "ORD012" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Đế tản nhiệt", Quantity = 1, Price = 300000 }
                },
                "ORD013" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Máy in HP", Quantity = 1, Price = 2500000 }
                },
                "ORD014" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Webcam Logitech", Quantity = 1, Price = 600000 }
                },
                "ORD015" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Balo Laptop", Quantity = 1, Price = 450000 }
                },
                "ORD016" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "SSD 512GB", Quantity = 1, Price = 900000 }
                },
                "ORD017" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Chuột chơi game", Quantity = 1, Price = 400000 }
                },
                "ORD018" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Loa Bluetooth", Quantity = 2, Price = 550000 }
                },
                "ORD019" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Bàn làm việc", Quantity = 1, Price = 2000000 }
                },
                "ORD020" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Ghế game", Quantity = 1, Price = 1800000 }
                },
                "ORD021" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Docking station", Quantity = 1, Price = 1250000 }
                },
                "ORD022" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Cáp mạng CAT6", Quantity = 10, Price = 50000 }
                },
                "ORD023" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Bộ phụ kiện văn phòng", Quantity = 4, Price = 850000 }
                },
                "ORD024" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Sạc dự phòng 10000mAh", Quantity = 1, Price = 420000 }
                },
                "ORD025" => new List<OrderDetail>
                {
                    new OrderDetail { ProductName = "Combo PC văn phòng", Quantity = 1, Price = 6890000 }
                },
                _ => new List<OrderDetail>() 
            };
        }

        public Task<bool> DeleteOrderAsync(string orderId)
        {
            return Task.FromResult(true); 
        }

        public Task<bool> UpdateOrderAsync(Order order)
        {
            return Task.FromResult(true);
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return Task.FromResult(new List<Product>
        {
            new Product { Id = "P01", Name = "Laptop Dell XPS 13", Price = 1000000 }, // Giá demo thấp để dễ tính
            new Product { Id = "P02", Name = "Chuột Logitech", Price = 250000 },
            new Product { Id = "P03", Name = "Màn hình LG 24 inch", Price = 1500000 },
            new Product { Id = "P04", Name = "Bàn phím cơ", Price = 1800000 },
            new Product { Id = "P05", Name = "Tai nghe Sony", Price = 900000 }
        });
        }

        // MỚI: Xử lý tạo đơn hàng
        public async Task<bool> CreateOrderAsync(Order order, List<OrderDetail> details)
        {
            await Task.Delay(800); // Giả lập delay mạng
                                   // Trong thực tế: Lưu order và details vào DB
            return true;
        }
    }
}
