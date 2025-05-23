﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using RookiEcom.Modules.Product.Domain.Shared;

namespace RookiEcom.Modules.Product.Contracts.Dtos
{
    public class ProductCreateDto
    {
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsFeature { get; set; }
        public IFormFile[] Images { get; set; }
        public List<ProductAttribute>? ProductAttributes { get; set; } = new List<ProductAttribute>();
        public ProductOption? ProductOption { get; set; }
    }
}