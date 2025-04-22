using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Delete;

public sealed class DeleteCategoryCommand(int id) : CommandBase<int>
{
    public int Id { get; set; } = id;
}