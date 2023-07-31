using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

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

        CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
        CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
    }
}
