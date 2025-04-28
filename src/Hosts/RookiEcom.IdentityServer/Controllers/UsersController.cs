using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Pagination;

namespace RookiEcom.IdentityServer.Controllers;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Route("api/v{version:apiVersion}/users")]
[Produces("application/problem+json")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserQueryService _userQueryService;
    
    public UsersController(ILogger<UsersController> logger,UserQueryService userQueryService)
    {
        _logger = logger;
        _userQueryService = userQueryService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers([FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get All Customers ");
        var pagedResult = await
            _userQueryService.GetCustomersAsync(pagingRequest.PageNumber, pagingRequest.PageSize, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = pagedResult,
            StatusCode = HttpStatusCode.OK
        });
    }

    [HttpGet("{userId:Guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AuthenticatedUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProfile([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(userId);
        var user = await _userQueryService.GetUserByIdAsync(userId, cancellationToken);
        return Ok(new ApiResponse
        {
            Result = user,
            StatusCode = HttpStatusCode.OK
        });
    }
}