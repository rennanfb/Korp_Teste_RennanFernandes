using BillingSystem.InvoiceService.Dto;

namespace InvoiceService.Dto
{
    public class UpdateInvoiceDto
    {
        public List<UpdateInvoiceItemDto> Items { get; set; } = new();
    }
}
