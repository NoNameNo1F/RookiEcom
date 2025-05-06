using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Exceptions;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.WebAPI.ExceptionHandlers;

public class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ApiExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = new ProblemDetails();
        
        switch (exception)
        {
            case ProductNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Title = "Product was Not Found",
                    Detail = exception.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = nameof(ProductNotFoundException)
                };
                break;
            
            case ProductSKUExistedException:
                problemDetails = new ProblemDetails
                {
                    Title = "Product SKU exists",
                    Detail = exception.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = nameof(ProductNotFoundException)
                };
                break;
            
            case CategoryNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Title = "Category was Not Found",
                    Detail = exception.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = nameof(CategoryNotFoundException)
                };
                break;
            
            case FailToUploadException:
                problemDetails = new ProblemDetails
                {
                    Title = "Blob Service Fail To Upload Image",
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = nameof(FailToUploadException)
                };
                break;
                
            case ValidationException validationException:
                problemDetails = new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest,
                    Type = nameof(ValidationException),
                    Extensions = { { "errors", validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) } }
                };
                break;
            
            default:
                problemDetails = new ProblemDetails
                {
                    Title = "An error occurred",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Type = exception.GetType().Name
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
    
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}