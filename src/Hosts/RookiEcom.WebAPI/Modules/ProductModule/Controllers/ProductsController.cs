using System.Net;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Pagination;
using RookiEcom.Modules.Product.Application.Commands.Product.Create;
using RookiEcom.Modules.Product.Application.Commands.Product.Delete;
using RookiEcom.Modules.Product.Application.Commands.Product.Update;
using RookiEcom.Modules.Product.Application.Queries;

using ProductDto = RookiEcom.WebAPI.Modules.ProductModule.Dtos.ProductDto;

namespace RookiEcom.WebAPI.Modules.ProductModule.Controllers;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Route("api/v{version:apiVersion}/products")]
[Produces("application/problem+json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ProductService _productService;
    
    public ProductsController(
        IMediator mediator,
        ProductService productService)
    {
        _mediator = mediator;
        _productService = productService;
    }

    [HttpGet("c{categoryId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByCategoryId(
        [FromQuery] PagingRequestDto pagingRequest,
        [FromRoute] int categoryId,
        CancellationToken cancellationToken = default)
    {
        var products = await _productService.GetProductsByCategoryId(pagingRequest.PageNumber, pagingRequest.PageSize,
            categoryId, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = products,
            StatusCode = HttpStatusCode.OK,
        });
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsPaging(
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken = default)
    {
        var pagedResult =
            await _productService.GetProducts(pagingRequest.PageNumber, pagingRequest.PageSize, cancellationToken);

        return Ok(new ApiResponse
        {
            Result = pagedResult,
            StatusCode = HttpStatusCode.OK
        });
    }

    [HttpGet("{productId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(
        [FromRoute] int productId, CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductById(productId, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = product,
            StatusCode = HttpStatusCode.OK
        });
    }

    [HttpGet("{sku}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBySKU(
        [FromRoute] string sku, CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductBySKU(sku, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = product,
            StatusCode = HttpStatusCode.OK
        });
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(
        [FromForm] ProductDto body, CancellationToken cancellationToken = default)
    {
        var command = new CreateProductCommand(
            body.SKU,
            body.CategoryId,
            body.Name,
            body.Description,
            body.MarketPrice,
            body.Price,
            body.StockQuantity,
            body.IsFeature,
            body.Images,
            body.ProductAttributes,
            body.ProductOption);

        await _mediator.Send(command, cancellationToken);

        return Created();
    }

    [HttpPut("{productId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        [FromRoute] int productId,
        [FromForm] ProductUpdateDto body,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateProductCommand(
            productId,
            body.SKU,
            body.CategoryId,
            body.Name,
            body.Description,
            body.MarketPrice,
            body.Price,
            body.StockQuantity,
            body.IsFeature,
            body.OldImages,
            body.NewImages,
            body.ProductAttributes,
            body.ProductOption);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{productId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] int productId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteProductCommand(productId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}