using System.ComponentModel.DataAnnotations;

namespace BillingSystem.StockService.Models;

public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

}