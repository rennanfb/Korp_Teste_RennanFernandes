using BillingSystem.InvoiceService.Models;

namespace BillingSystem.InvoiceService.Dto;

public class InvoiceResponseDto
{
    /// <summary>
    /// Unique invoice identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique invoice number.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Current status of the invoice.
    /// </summary>
    public InvoiceStatus Status { get; set; }

    /// <summary>
    /// Items that compose the invoice.
    /// </summary>
    public List<InvoiceItemResponseDto> Items { get; set; }
}