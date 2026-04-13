using Microsoft.EntityFrameworkCore;
using BillingSystem.InvoiceService.Models;

namespace BillingSystem.InvoiceService.Data;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> opts) : base(opts)
    {
    }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<InvoiceItem> InvoiceItems { get; set; }
}