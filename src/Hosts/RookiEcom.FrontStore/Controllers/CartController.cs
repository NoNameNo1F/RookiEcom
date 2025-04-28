using Microsoft.AspNetCore.Mvc;

namespace RookiEcom.FrontStore.Controllers;

public class CartController : Controller
{
    public CartController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}