using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Modules.Cart.Application.Commands.AddItem;
using RookiEcom.Modules.Cart.Application.Commands.ClearCart;
using RookiEcom.Modules.Cart.Application.Commands.RemoveItem;
using RookiEcom.Modules.Cart.Application.Commands.UpdateQuantity;
using RookiEcom.Modules.Cart.Application.Queries;
using RookiEcom.Modules.Cart.Contracts.Dtos;

namespace RookiEcom.WebAPI.Modules.CartModule;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Route("api/v{version:apiVersion}/carts")]
[Produces("application/problem+json")]
[Authorize(Policy = "AuthenticatedUser")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CartService _cartService;

    public CartsController(IMediator mediator, CartService cartService)
    {
        _mediator = mediator;
        _cartService = cartService;
    }
    
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        
        throw new UnauthorizedAccessException("User ID claim not found or invalid.");
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart(
        CancellationToken cancellationToken = default)
    { 
        var customerId = GetCurrentUserId();
        var cartDto = await _cartService.GetCartByCustomerId(customerId, cancellationToken);
        
        if (cartDto == null)
        {
            cartDto = new CartDto { CustomerId = customerId };
        }

        return Ok(new ApiResponse
        {
            Result = cartDto, 
            StatusCode = HttpStatusCode.OK
        });
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDto body, CancellationToken cancellationToken = default)
    {
        var customerId = GetCurrentUserId();

        var command = new AddCartItemCommand
        {
            CustomerId = customerId,
            ProductId = body.ProductId,
            ProductName = body.ProductName,
            Price = body.Price,
            Quantity = body.Quantity,
            Image = body.Image
        };

        await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse
        {
            StatusCode = HttpStatusCode.OK
        });
    }

    [HttpPut("items/{cartItemId:int}")]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemQuantityRequestDto request, CancellationToken cancellationToken = default)
    {
        var customerId = GetCurrentUserId();
        var command = new UpdateCartItemQuantityCommand(customerId, cartItemId, request.Quantity);
        
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("items/{cartItemId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveItemFromCart(int cartItemId, CancellationToken cancellationToken = default)
    {
        var customerId = GetCurrentUserId();
        var command = new RemoveCartItemCommand(customerId, cartItemId);
        
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken = default)
    {
        var customerId = GetCurrentUserId();
        var command = new ClearCartCommand(customerId);
        
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}