using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Windows.Storage;

public class ProductService : IProductService
{
    private readonly HttpClient _client;
    private readonly IAuthService _authService;

    public ProductService(
        IHttpClientFactory httpClientFactory,
        IAuthService authService)
    {
        _client = httpClientFactory.CreateClient("API");
        _authService = authService;
    }

    public async Task<ProductListData?> GetProductsAsync(
        int page = 1,
        int size = 20,
        string sortBy = "productName",
        string sortDir = "asc")
    {
        var token = _authService.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var url =
            $"/api/v1/products?page={page}&size={size}&sortBy={sortBy}&sortDir={sortDir}";

        var response = await _client.GetAsync(url);



        var rawJson = await response.Content.ReadAsStringAsync();


        if (!response.IsSuccessStatusCode)
            return null;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<
            ApiResponse<ProductListData>>(rawJson, options);


        Debug.WriteLine($"Success: {apiResponse?.Success}");
        Debug.WriteLine($"Products: {apiResponse?.Data?.Products?.Count}");


        if (apiResponse == null || !apiResponse.Success)
            return null;

        return apiResponse.Data;
    }

    public async Task<bool> CreateProductAsync(CreateProductRequest request)
    {
        var token = _authService.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var res = await _client.PostAsJsonAsync("/api/v1/products", request);

        Debug.WriteLine($"[CREATE PRODUCT] {res.StatusCode}");

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var token = _authService.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // Nếu API chỉ cần productId (không cần body)
        var response = await _client.PatchAsync($"/api/v1/products/{productId}", null);

        Debug.WriteLine($"[DELETE PRODUCT] {response.StatusCode}");

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UploadImageAsync(int productId, StorageFile file)
    {
        try
        {
            var token = _authService.GetAccessToken();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var stream = await file.OpenStreamForReadAsync();

            using var form = new MultipartFormDataContent();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(GetMimeType(file.FileType));

            // ⚠️ "file" PHẢI TRÙNG với @RequestParam("file") bên BE
            form.Add(fileContent, "file", file.Name);

            var response = await _client.PostAsync(
                $"/api/v1/products/{productId}/images",
                form
            );

            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(body);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UploadImageAsync error: " + ex);
            return false;
        }
    }


    private string GetMimeType(string extension)
    {
        return extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }


    public async Task<bool> UpdateProductAsync(int productId, Product product)
    {
        var token = _authService.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var url = $"/api/v1/products/{productId}";

        var response = await _client.PutAsJsonAsync(url, new
        {
            categoryId = product.Category.CategoryId,
            productName = product.ProductName,
            description = product.Description,
            author = product.Author,
            publisher = product.Publisher,
            publicationYear = product.PublicationYear,
            price = product.Price,
            stockQuantity = product.StockQuantity,
            minStockQuantity = product.MinStockQuantity
        });

        Debug.WriteLine($"[UPDATE PRODUCT] {response.StatusCode}");
        return response.IsSuccessStatusCode;
    }

}
