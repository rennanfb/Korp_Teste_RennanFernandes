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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        var productsResponse = _mapper.Map<List<ProductResponseDto>>(products);

        return Ok(productsResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        var productResponse = _mapper.Map<ProductResponseDto>(product);

        return Ok(productResponse);
    }

    [HttpGet("{id}/stock")]
    public async Task<IActionResult> GetStock(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product.Stock);
    }

    [HttpPost]
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

        return Ok(productResponse);
    }

    [HttpPost("{id}/decrease")]
    public async Task<IActionResult> DecreaseStock(int id, DecreaseStockDto dto)
    {
        // 1. Verifica se o produto existe
        var productExists = await _context.Products
            .AnyAsync(p => p.Id == id);

        if (!productExists)
            return NotFound("Product not found");

        // 2. Update ATÔMICO (resolve concorrência)
        var rows = await _context.Database.ExecuteSqlRawAsync(@"
        UPDATE Products
        SET Stock = Stock - {0}
        WHERE Id = {1} AND Stock >= {0}
    ", dto.Quantity, id);

        // 3. Se não atualizou, é estoque insuficiente
        if (rows == 0)
            return BadRequest("Insufficient stock");

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateById(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound("Product not found");

        _mapper.Map(dto, product);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
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