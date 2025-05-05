using AuthTokenServer.EntityLayer.DTOs;
using AuthTokenServer.EntityLayer.Entities;
using AutoMapper;

namespace AuthTokenServer.BusinessLayer.Mapping;

public class ProductMapping:Profile
{
    public ProductMapping()
    {
        CreateMap<AddProductDto, Product>().ReverseMap();
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}
