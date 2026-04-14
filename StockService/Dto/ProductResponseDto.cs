namespace BillingSystem.StockService.Dto
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
