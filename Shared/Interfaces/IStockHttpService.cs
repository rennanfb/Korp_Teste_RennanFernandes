using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Shared.Interfaces;

public interface IStockHttpService
{
    Task<bool> ProductExists(int productId);
    Task<int?> GetStock(int productId);
    Task<bool> DecreaseStock(int productId, int quantity);
    Task<bool> HasStock(int productId, int quantity);
}