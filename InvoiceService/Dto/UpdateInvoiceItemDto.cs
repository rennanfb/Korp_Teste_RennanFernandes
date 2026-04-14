using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Dto
{
    public class UpdateInvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}