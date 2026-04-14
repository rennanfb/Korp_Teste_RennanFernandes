using AutoMapper;
using BillingSystem.InvoiceService.Data;
using BillingSystem.InvoiceService.Dto;
using BillingSystem.InvoiceService.Models;
using BillingSystem.Shared.Interfaces;
using InvoiceService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.InvoiceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly InvoiceDbContext _context;
    private readonly IStockHttpService _stockHttpService;
    private readonly IMapper _mapper;

    public InvoicesController(InvoiceDbContext context, IStockHttpService stockHttpService, IMapper mapper)
    {
        _context = context;
        _stockHttpService = stockHttpService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceDto createInvoiceDto)
    {
        if (createInvoiceDto.Items == null || !createInvoiceDto.Items.Any())
            return BadRequest("Invoice must have at least one item");

        var invoice = new Invoice
        {
            Status = InvoiceStatus.Open,
            Items = new List<InvoiceItem>()
        };

        foreach (var itemDto in createInvoiceDto.Items)
        {
            ProductSummaryDto? product;

            try
            {
                product = await _stockHttpService.GetProductById(itemDto.ProductId);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Stock service unavailable");
            }
            catch (Exception)
            {
                return StatusCode(503, "Error communicating with stock service");
            }

            if (product == null)
                return BadRequest($"Product {itemDto.ProductId} not found");

            invoice.Items.Add(new InvoiceItem
            {
                ProductId = itemDto.ProductId,
                ProductDescription = product.Description,
                Quantity = itemDto.Quantity
            });
        }

        _context.Invoices.Add(invoice);

        try
        {
            await _context.SaveChangesAsync();

            invoice.Number = $"INV-{DateTime.UtcNow.Year}-{invoice.Id:D4}";

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }

        var invoiceResponse = _mapper.Map<InvoiceResponseDto>(invoice);

        return CreatedAtAction(
            nameof(GetById),
            new { id = invoice.Id },
            invoiceResponse
        );
    }

    [HttpPost("{id}/print")]
    public async Task<IActionResult> Print(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status != InvoiceStatus.Open)
            return BadRequest("Invoice is not open");

        foreach (var item in invoice.Items)
        {
            var hasStock = await _stockHttpService.HasStock(item.ProductId, item.Quantity);

            if (!hasStock)
                return BadRequest($"Insufficient stock for product {item.ProductId}");
        }

        foreach (var item in invoice.Items)
        {
            var success = await _stockHttpService.DecreaseStock(item.ProductId, item.Quantity);

            if (!success)
                return BadRequest($"Failed to update stock for product {item.ProductId}");
        }

        invoice.Status = InvoiceStatus.Closed;

        await _context.SaveChangesAsync();

        var invoiceResponse = _mapper.Map<InvoiceResponseDto>(invoice);

        return Ok(invoiceResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _context.Invoices
            .Include(i => i.Items)
            .ToListAsync();

        var result = _mapper.Map<List<InvoiceResponseDto>>(invoices);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        var result = _mapper.Map<InvoiceResponseDto>(invoice);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateById(int id, UpdateInvoiceDto dto)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound("Invoice not found");

        _mapper.Map(dto, invoice);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status == InvoiceStatus.Closed) return BadRequest("Cannot delete a closed invoice");

        _context.Remove(invoice);
        await _context.SaveChangesAsync();
        return NoContent();
    }


    //Concurrency Test

    [HttpGet("test-concurrency/{id}")]
    public async Task<IActionResult> TestConcurrency(int id)
    {

        var tasks = new List<Task<bool>>();

        for (int i = 0; i < 2; i++)
        {
            tasks.Add(_stockHttpService.DecreaseStock(id, 1));
        }

        var results = await Task.WhenAll(tasks);

        return Ok(results);
    }

}