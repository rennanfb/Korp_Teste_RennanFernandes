using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Models;

public class InvoiceItem
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}