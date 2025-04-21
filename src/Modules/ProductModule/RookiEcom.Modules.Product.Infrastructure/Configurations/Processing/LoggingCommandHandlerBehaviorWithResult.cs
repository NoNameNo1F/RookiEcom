using MediatR;
using Microsoft.Extensions.Logging;

namespace RookiEcom.Modules.Product.Infrastructure.Configurations.Processing;
public class LoggingCommandHandlerBehaviorWithResult<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingCommandHandlerBehaviorWithResult<TRequest, TResponse>> _logger;

    public LoggingCommandHandlerBehaviorWithResult(ILogger<LoggingCommandHandlerBehaviorWithResult<TRequest, TResponse>> logger)
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
