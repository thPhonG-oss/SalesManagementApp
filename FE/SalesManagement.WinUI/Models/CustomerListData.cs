using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    // Wrapper cho phản hồi danh sách khách hàng phân trang

    public class Customer
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        // Lưu ý: Tên property JSON phải khớp với API Backend trả về.
        // Nếu Backend trả về "customerName", hãy sửa JsonPropertyName thành "customerName"
        [JsonPropertyName("customerName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
    }
    public class CustomerListData
    {
        [JsonPropertyName("content")]
        public List<Customer> Customers { get; set; } = new();

        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("number")] // API trả về "number" là số trang hiện tại
        public int Page { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}