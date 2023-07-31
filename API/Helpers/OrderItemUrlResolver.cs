﻿using API.DTOs;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
        {
            // If PictureUrl has a value, construct the full image URL by
            // concatenating the "ApiUrl" configuration value and the relative
            // picture URL from the source product.
            return _configuration["ApiUrl"] + source.ItemOrdered.PictureUrl;
        }

        return null;
    }
}
