using System.Collections.Generic;

namespace RookiEcom.Modules.Product.Domain.Shared
{
    public class ProductOption
    {
        public string Code { get; set; }
        public List<string> Values { get; set; }

        public ProductOption()
        {
        }
        public ProductOption(string code, List<string> values)
        {
            Code = code;
            Values = values;
        }
    }
}