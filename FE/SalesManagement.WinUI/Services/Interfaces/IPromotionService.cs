using SalesManagement.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetActivePromotionsAsync();
    }
}
