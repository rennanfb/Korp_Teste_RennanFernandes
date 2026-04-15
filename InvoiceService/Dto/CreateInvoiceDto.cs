namespace BillingSystem.InvoiceService.Dto;

public class CreateInvoiceDto
{
    /// <summary>
    /// Items that compose the invoice.
    /// </summary>
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}

