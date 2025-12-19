using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Models
{
    // Status enum

    
    public class Order
    {
        public string OrderId { get; set; } = string.Empty;
        public DateTime Date { get; set; } 
        public int ItemsCount { get; set; } 
        public decimal Amount { get; set; } 
        public string Status { get; set; } = string.Empty;
    }
}
