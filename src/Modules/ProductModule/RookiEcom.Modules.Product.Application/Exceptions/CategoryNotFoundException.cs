namespace RookiEcom.Modules.Product.Application.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(int categoryId) : base($"Category {categoryId} was Not Found")
    {
    }
}