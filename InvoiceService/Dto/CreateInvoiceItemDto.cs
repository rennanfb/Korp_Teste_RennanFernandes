using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Dto
{
    public class CreateInvoiceItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
