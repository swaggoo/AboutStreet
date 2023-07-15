﻿using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;
public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
        : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || 
            x.Name.ToLower().Contains(productSpecParams.Search)) &&
            (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId) && 
            (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId)
        )
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderBy(x => x.Name);
        ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageNumber - 1), 
            productSpecParams.PageSize);

        if (!string.IsNullOrEmpty(productSpecParams.Sort))
        {
            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    {
                        AddOrderBy(p => p.Price); break;
                    }
                case "priceDesc":
                    {
                        AddOrderByDescending(p => p.Price); break;
                    }
                default:
                    {
                        AddOrderBy(p => p.Name); break;
                    }
            }
        }
    }
    
    public ProductsWithTypesAndBrandsSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}
