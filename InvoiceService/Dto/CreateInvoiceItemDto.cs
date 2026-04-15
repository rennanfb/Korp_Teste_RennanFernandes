using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Dto
{
    public class CreateInvoiceItemDto
    {
        /// <summary>
        /// Unique product identifier.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of products for the invoice line.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
