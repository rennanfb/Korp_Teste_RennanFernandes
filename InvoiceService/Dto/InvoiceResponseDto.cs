using BillingSystem.InvoiceService.Models;

namespace BillingSystem.InvoiceService.Dto;

public class InvoiceResponseDto
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public InvoiceStatus Status { get; set; }
    public List<InvoiceItemResponseDto> Items { get; set; }
}