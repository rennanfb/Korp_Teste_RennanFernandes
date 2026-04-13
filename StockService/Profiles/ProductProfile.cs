using AutoMapper;
using BillingSystem.StockService.Dto;
using BillingSystem.StockService.Models;
using StockService.Dto;
namespace BillingSystem.StockService.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductResponseDto>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
