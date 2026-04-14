namespace StockService.Dto
{
    public class UpdateProductDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
