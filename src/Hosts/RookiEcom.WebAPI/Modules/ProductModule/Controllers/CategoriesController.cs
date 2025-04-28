using System.Net;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Pagination;
using RookiEcom.Modules.Product.Application.Commands.Category.Create;
using RookiEcom.Modules.Product.Application.Commands.Category.Delete;
using RookiEcom.Modules.Product.Application.Commands.Category.Update;
using RookiEcom.Modules.Product.Application.Dtos;
using RookiEcom.Modules.Product.Application.Queries;

namespace RookiEcom.WebAPI.Modules.ProductModule.Controllers;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Route("api/v{version:apiVersion}/categories")]
[Produces("application/problem+json")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CategoryService _categoryService;

    public CategoriesController(IMediator mediator, CategoryService categoryService)
    {
        _mediator = mediator;
        _categoryService = categoryService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken = default)
    {
        var pagedResult = await _categoryService.GetAllCategories(
            pagingRequest.PageNumber, pagingRequest.PageSize, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = pagedResult,
            StatusCode = HttpStatusCode.OK
        });
    }
    
    [HttpGet("{categoryId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory([FromRoute] int categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryService.GetCategoryById(categoryId, cancellationToken);
        
        return Ok(new ApiResponse
        {
            Result = category,
            StatusCode = HttpStatusCode.OK
        });
    }
    
    [HttpGet("{categoryId:int}/tree")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryTree(
        [FromRoute] int categoryId, 
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryTree(categoryId, cancellationToken);

        return Ok(new ApiResponse
        {
            Result = categories,
            StatusCode = HttpStatusCode.OK
        });
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory(
        [FromForm] CategoryDto body,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateCategoryCommand(
            body.Name,
            body.Description,
            body.ParentId,
            body.IsPrimary,
            body.Image);

        await _mediator.Send(command, cancellationToken);

        return Created();
    }

    [HttpPut("{categoryId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] int categoryId,
        [FromForm] CategoryDto body,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateCategoryCommand(
            categoryId,
            body.Name,
            body.Description,
            body.ParentId,
            body.IsPrimary,
            body.Image);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
    
    [HttpDelete("{categoryId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteCategoryCommand(categoryId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}