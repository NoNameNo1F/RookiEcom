namespace RookiEcom.Modules.Product.Application.Exceptions;

public class ProductSkuExistedException : Exception
{
    public ProductSkuExistedException(string productSku) : base($"Product SKU {productSku} existed.")
    {
    }
}