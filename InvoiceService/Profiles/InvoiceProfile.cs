using AutoMapper;
using BillingSystem.InvoiceService.Dto;
using BillingSystem.InvoiceService.Models;

namespace BillingSystem.InvoiceService.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<CreateInvoiceItemDto, InvoiceItem>();

            CreateMap<Invoice, InvoiceResponseDto>();

            CreateMap<InvoiceItem, InvoiceItemResponseDto>();
        }
    }
}
