namespace RookiEcom.Modules.Product.Application.Exceptions;

public class ProductSKUExistedException : Exception
{
    public ProductSKUExistedException(object productSku) : base($"Product SKU {productSku} existed.")
    {
    }
}