namespace StockService.Dto
{
    public class UpdateProductDto
    {
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
