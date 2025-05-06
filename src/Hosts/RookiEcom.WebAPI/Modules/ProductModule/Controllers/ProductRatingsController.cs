using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Pagination;
using RookiEcom.Modules.Product.Application.Commands.ProductRating.Create;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.WebAPI.Modules.ProductModule.Controllers;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Route("api/v{version:apiVersion}/products/{productId:int}/ratings")]
[Produces("application/problem+json")]
public class ProductRatingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ProductRatingService _productRatingService;
    private readonly ILogger<ProductRatingsController> _logger;
    
    public ProductRatingsController(
        IMediator mediator,
        ProductRatingService productRatingService,
        ILogger<ProductRatingsController> logger)
    {
        _mediator = mediator;
        _productRatingService = productRatingService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductRatings(
        [FromRoute] int productId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken = default)
    {
        var pagedResult = await _productRatingService.GetRatingsPaging(
            productId, 
            pagingRequest.PageNumber, 
            pagingRequest.PageSize,
            cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = pagedResult,
            StatusCode = HttpStatusCode.OK
        });
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateProductRating(
        [FromRoute] int productId,
        [FromForm] ProductRatingCreateDto body,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateProductRatingCommand
        {
            ProductId = productId,
            CustomerId = body.CustomerId,
            CustomerName = body.CustomerName,
            Score = body.Score,
            Content = body.Content,
            Image = body.Image
        };

        await _mediator.Send(command, cancellationToken);

        return Created();
    }
}
