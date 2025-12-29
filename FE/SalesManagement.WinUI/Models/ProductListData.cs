using System.Text.Json.Serialization;

namespace SalesManagement.WinUI.Models
{
    public class ProductListData
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }

        [JsonPropertyName("lastPage")]
        public bool LastPage { get; set; }
    }
}
