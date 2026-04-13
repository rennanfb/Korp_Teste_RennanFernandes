using BillingSystem.InvoiceService.Models;
using System.ComponentModel.DataAnnotations;

namespace BillingSystem.InvoiceService.Dto;

public class CreateInvoiceDto
{
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}

