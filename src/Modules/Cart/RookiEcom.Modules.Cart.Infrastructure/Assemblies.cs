using System.Reflection;
using RookiEcom.Modules.Cart.Application.Queries;

namespace RookiEcom.Modules.Cart.Infrastructure;

public class Assemblies
{
    public static readonly Assembly Application = typeof(CartService).Assembly;
}
