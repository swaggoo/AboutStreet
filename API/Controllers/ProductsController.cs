﻿using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.DTOs;
using System.Collections.Immutable;
using AutoMapper;
using API.Errors;
using API.Helpers;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(
        IGenericRepository<Product> productsRepo, 
        IGenericRepository<ProductBrand> productBrandRepo, 
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper)
    {
        _productsRepo = productsRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
        [FromQuery]ProductSpecParams productSpecParams)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(productSpecParams);

        var countSpec = new ProductWithFiltersForCountSpecification(productSpecParams);

        var totalItems = await _productsRepo.CountAsync(countSpec);

        var products = await _productsRepo.ListAsync(spec);
        
        var data = _mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

        return Ok(new Pagination<ProductToReturnDto>(productSpecParams.PageNumber, 
            productSpecParams.PageSize, totalItems, data));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);

        var product = await _productsRepo.GetEntityWithSpec(spec);

        if (product  == null)
        {
            return NotFound(new ApiResponse(404));
        }

        return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        var brands = await _productBrandRepo.GetAllAsync();

        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        var types = await _productTypeRepo.GetAllAsync();

        return Ok(types);
    }

}
