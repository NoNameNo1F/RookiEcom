using System.Net;
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
        ProblemDetails problemDetails;
        
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
                    Status = StatusCodes.Status404NotFound,
                    Type = nameof(FailToUploadException)
                };
                break;
            
            case InvalidCommandException:
                problemDetails = new ProblemDetails
                {
                    Title = "Invalid Command Exception",
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = nameof(InvalidCommandException)
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
                return false;
        }
        
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        if (exception is InvalidCommandException invalidCommandException)
        {
            await httpContext.Response.WriteAsJsonAsync(new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = invalidCommandException.Errors,
                IsSuccess = false,
            }, cancellationToken: cancellationToken);

            return true;
        }
    
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}