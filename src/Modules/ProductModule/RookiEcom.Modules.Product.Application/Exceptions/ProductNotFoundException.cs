namespace RookiEcom.Modules.Product.Application.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(object productId) : base($"Product {productId} was Not Found")
    {
    }
}