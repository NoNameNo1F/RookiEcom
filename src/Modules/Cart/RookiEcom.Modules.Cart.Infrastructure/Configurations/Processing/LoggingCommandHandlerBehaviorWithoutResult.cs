using MediatR;
using Microsoft.Extensions.Logging;
using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Cart.Infrastructure.Configurations.Processing;
public class LoggingCommandHandlerBehaviorWithoutResult<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly ILogger<LoggingCommandHandlerBehaviorWithoutResult<TRequest, TResponse>> _logger;

    public LoggingCommandHandlerBehaviorWithoutResult(ILogger<LoggingCommandHandlerBehaviorWithoutResult<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {typeof(TRequest).Name}");
        var response = await next();
        _logger.LogInformation($"Handled {typeof(TRequest).Name}");

        return response;
    }
}
