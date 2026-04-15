using System.ComponentModel.DataAnnotations;

namespace BillingSystem.StockService.Dto;

public class CreateProductDto
{

    /// <summary>
    /// Product code used for identification.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;


    /// <summary>
    /// Human-readable product description.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Stock of the product.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
