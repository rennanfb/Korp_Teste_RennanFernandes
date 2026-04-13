using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Models;

public enum InvoiceStatus
{
    Open,
    Closed
}

public class Invoice
{
    [Key]
    [Required]
    public int Id { get; set; }

    public string? Number { get; set; }

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Open;

    [Required]
    public List<InvoiceItem> Items { get; set; } = new();
}