using BillingSystem.Shared.Interfaces;
using System.Net.Http.Json;

public class StockHttpService : IStockHttpService
{
    private readonly HttpClient _http;

    public StockHttpService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ProductExists(int productId)
    {
        try
        {
            var res = await _http.GetAsync($"/api/products/{productId}");
            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }

    public async Task<int?> GetStock(int productId)
    {
        try
        {
            return await _http.GetFromJsonAsync<int>($"/api/products/{productId}/stock");
        }
        catch (HttpRequestException)
        {
            throw; 
        }
    }

    public async Task<bool> DecreaseStock(int productId, int quantity)
    {
        try
        {
            var res = await _http.PostAsJsonAsync(
                $"/api/products/{productId}/decrease",
                new { quantity });

            return res.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            throw; // serviço indisponível
        }
    }

    public async Task<bool> HasStock(int productId, int quantity)
    {
        try
        {
            var stock = await GetStock(productId);
            return stock >= quantity;
        }
        catch
        {
            throw;
        }
    }

}