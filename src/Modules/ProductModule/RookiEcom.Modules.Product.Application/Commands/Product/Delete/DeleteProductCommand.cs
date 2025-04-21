using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Delete;

public class DeleteProductCommand(int id) : CommandBase
{
    public int Id { get; set; } = id;
}