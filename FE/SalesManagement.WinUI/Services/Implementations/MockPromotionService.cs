using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Implementations
{
    public class MockPromotionService : IPromotionService
    {
        public Task<List<Promotion>> GetActivePromotionsAsync()
        {
            // Mock dữ liệu theo cấu trúc JSON bạn đưa
            var promotions = new List<Promotion>
            {
                // Item 1: Mặc định (Không áp dụng)
                new Promotion
                {
                    PromotionId = 0,
                    PromotionCode = "NONE",
                    PromotionName = "Không áp dụng",
                    DiscountType = "FIXED",
                    DiscountValue = 0
                },
                // Item 2: Giảm theo %
                new Promotion
                {
                    PromotionId = 1,
                    PromotionCode = "SALE10",
                    PromotionName = "Giảm 10% đơn hàng",
                    DiscountType = "PERCENTAGE",
                    DiscountPercentage = 10, 
                    MaxDiscountValue = 100000,
                    MinOrderAmount = 200000
                },
                // Item 3: Giảm tiền mặt
                new Promotion
                {
                    PromotionId = 2,
                    PromotionCode = "FREESHIP",
                    PromotionName = "Giảm 50k phí ship",
                    DiscountType = "FIXED",
                    DiscountValue = 50000,
                    MinOrderAmount = 0
                }
            };

            return Task.FromResult(promotions);
        }
    }
}
