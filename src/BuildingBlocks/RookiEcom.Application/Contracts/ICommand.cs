using MediatR;

namespace RookiEcom.Application.Contracts;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
