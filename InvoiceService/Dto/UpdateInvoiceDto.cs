using BillingSystem.InvoiceService.Dto;

namespace InvoiceService.Dto
{
    public class UpdateInvoiceDto
    {
        /// <summary>
        /// Items that compose the invoice.
        /// </summary>
        public List<UpdateInvoiceItemDto> Items { get; set; } = new();
    }
}
