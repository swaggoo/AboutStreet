using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(p => p.ProductBrand, opt =>
            opt.MapFrom(s => s.ProductBrand.Name))
            .ForMember(p => p.ProductType, opt =>
            opt.MapFrom(s => s.ProductType.Name));
    }
}
