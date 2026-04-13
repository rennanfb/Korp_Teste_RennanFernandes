using Microsoft.EntityFrameworkCore;
using BillingSystem.StockService.Models;

namespace BillingSystem.StockService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> opts) : base(opts)
    {
    }

    public DbSet<Product> Products { get; set; }

}