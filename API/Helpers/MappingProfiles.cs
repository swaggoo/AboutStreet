﻿using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(d => d.ProductBrand, opt =>
            opt.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, opt =>
            opt.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, opt =>
            opt.MapFrom<ProductUrlResolver>());

        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
    }
}
