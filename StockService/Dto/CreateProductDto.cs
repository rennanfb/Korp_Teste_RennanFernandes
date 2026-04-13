using System.ComponentModel.DataAnnotations;

namespace BillingSystem.StockService.Dto;

public class CreateProductDto
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
