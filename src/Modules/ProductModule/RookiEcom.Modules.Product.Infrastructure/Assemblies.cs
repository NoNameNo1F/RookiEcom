using System.Reflection;
using RookiEcom.Modules.Product.Application;

namespace RookiEcom.Modules.Product.Infrastructure;

public static class Assemblies
{
    public static readonly Assembly Application = typeof(ProductContext).Assembly;
}