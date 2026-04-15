using AutoMapper;
using BillingSystem.StockService.Data;
using BillingSystem.StockService.Dto;
using BillingSystem.StockService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Dto;

namespace BillingSystem.StockService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;
    private readonly IMapper _mapper;
     

    public ProductsController(ProductDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        var productsResponse = _mapper.Map<List<ProductResponseDto>>(products);

        return Ok(productsResponse);
    }


    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        var productResponse = _mapper.Map<ProductResponseDto>(product);

        return Ok(productResponse);
    }

    /// <summary>
    /// Retrieves the stock quantity of a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    [HttpGet("{id}/stock")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStock(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product.Stock);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="createProductDto">The product data to be created.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exists = await _context.Products
            .AnyAsync(p => p.Code == createProductDto.Code);

        if (exists)
            return BadRequest("Product with this code already exists");

        var product = _mapper.Map<Product>(createProductDto);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var productResponse = _mapper.Map<ProductResponseDto>(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            productResponse
        );
    }

    /// <summary>
    /// Decreases the stock of a product atomically, ensuring concurrency safety.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="dto">The quantity to decrease from stock.</param>
    [HttpPost("{id}/decrease")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DecreaseStock(int id, DecreaseStockDto dto)
    {
        var productExists = await _context.Products
            .AnyAsync(p => p.Id == id);

        if (!productExists)
            return NotFound("Product not found");

        var rows = await _context.Database.ExecuteSqlRawAsync(@"
        UPDATE Products
        SET Stock = Stock - {0}
        WHERE Id = {1} AND Stock >= {0}
    ", dto.Quantity, id);

        if (rows == 0)
            return BadRequest("Insufficient stock");

        return Ok();
    }


    /// <summary>
    /// Updates a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="dto">The updated product data.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateById(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound("Product not found");

        _mapper.Map(dto, product);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(i => i.Id == id);

        if (product == null)
            return NotFound();

        _context.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}