using BillingSystem.Shared.Interfaces;
using System.Text.Json;

public class StockHttpService : IStockHttpService
{
    private readonly HttpClient _http;

    public StockHttpService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Checks if a product exists in the stock service.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>True if the product exists; otherwise, false.</returns>
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

    /// <summary>
    /// Retrieves the current stock quantity of a product.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>The stock quantity, or null if not found.</returns>
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

    /// <summary>
    /// Decreases the stock of a product.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The quantity to decrease.</param>
    /// <returns>True if the operation succeeded; otherwise, false.</returns>
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

    /// <summary>
    /// Checks if the product has sufficient stock.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantity">The required quantity.</param>
    /// <returns>True if enough stock is available; otherwise, false.</returns>
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


    /// <summary>
    /// Retrieves product summary information by its ID.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>The product summary, or null if not found.</returns>
    public async Task<ProductSummaryDto?> GetProductById(int productId)
    {
        var response = await _http.GetAsync($"api/products/{productId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        var product = JsonSerializer.Deserialize<ProductSummaryDto>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return product;
    }

}