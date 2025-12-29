using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Models
{
    // Status enum


    public class OrderDetail
    {
        public string ProductName { get; set; } = string.Empty; 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class Order
    {

        public long Id { get; set; }


        public string OrderCode { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ItemsCount { get; set; }
        public decimal Amount { get; set; }
    }

   


}