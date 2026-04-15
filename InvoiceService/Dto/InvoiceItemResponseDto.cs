namespace BillingSystem.InvoiceService.Dto
{
    public class InvoiceItemResponseDto
    {
        /// <summary>
        /// Unique product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of products for the invoice line.
        /// </summary>
        public int Quantity { get; set; }
    }
}
