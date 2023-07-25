using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //
    public string Resolve(
        Product source, 
        ProductToReturnDto destination, 
        string destMember, 
        ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            // If PictureUrl has a value, construct the full image URL by
            // concatenating the "ApiUrl" configuration value and the relative
            // picture URL from the source product.
            return _configuration["ApiUrl"] + source.PictureUrl;
        }

        return null;
    }
}
