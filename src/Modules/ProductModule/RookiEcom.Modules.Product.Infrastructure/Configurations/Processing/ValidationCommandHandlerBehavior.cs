using FluentValidation;
using MediatR;
using RookiEcom.Application.Exceptions;

namespace RookiEcom.Modules.Product.Infrastructure.Configurations.Processing;

public class ValidationCommandHandlerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationCommandHandlerBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            // InvalidCommand
            throw new InvalidCommandException(failures.Select(f => f.ErrorMessage).ToList());
        }

        return await next();
    }
}
