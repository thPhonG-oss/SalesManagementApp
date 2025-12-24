using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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


}
