namespace RookiEcom.Modules.Product.Domain.Shared
{
    public class ProductAttribute
    {
        public string Code { get; set; }
        public string Value { get; set; }

        public ProductAttribute()
        {
        }
        public ProductAttribute(string code, string value)
        {
            Code = code;
            Value = value;
        }
    }   
}