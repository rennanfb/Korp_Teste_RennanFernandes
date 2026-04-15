namespace BillingSystem.StockService.Dto
{
    public class ProductResponseDto
    {
        /// <summary>
        /// Unique product identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product code used for identification.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable product description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Stock of the product.
        /// </summary>
        public int Stock { get; set; }
    }
}
