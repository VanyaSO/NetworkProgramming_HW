using GameStore.Infrastructure;
using GameStore.Interfaces;
using GameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

[ViewComponent(Name = "Cart")]
public class CartController : Controller
{
    private readonly IProduct _products;
    private readonly IOrder _order;

    public CartController(IProduct products, IOrder order)
    {
        _products = products;
        _order = order;
    }
    private Cart GetCart() => HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
    private void SaveCart(Cart cart) => HttpContext.Session.SetJson("Cart", cart);
    private User GetUser() => HttpContext.Session.GetJson<User>("User");

    [HttpGet]
    public IActionResult Index(string returnUrl)
    {
        var user = GetUser();
        if (user == null)
        {
            return RedirectToAction(nameof(Index), "Account");
        }
        
        ViewBag.returnUrl = returnUrl;
        ViewBag.User = user;
        return View(GetCart());
    }
    
    [HttpPost]
    public IActionResult AddToCart(Product product, string returnUrl)
    {
        SaveCart(GetCart().AddItem(product, 1));
        return RedirectToAction(nameof(Index), new { returnUrl });
    }
    
    [HttpPost]
    public IActionResult RemoveFromCart(int productId, string returnUrl)
    {
        SaveCart(GetCart().RemoveItem(productId));
        return RedirectToAction(nameof(Index), new { returnUrl });
    }
    
    public IActionResult Completed()
    {
        GetCart().Clear();
        HttpContext.Session.Remove("Cart");
        var user = GetUser();
        if (user == null)
        {
            return RedirectToAction("Index", "Account");
        }
        
        ViewBag.User = user;
        return View();
    }
    
    public IActionResult CreateOrder()
    {
        var user = GetUser();
        if (user == null)
        {
            return RedirectToAction("Index", "Account");
        }
        
        ViewBag.User = user;
        return View();
    }
    
    [HttpPost]
    public IActionResult CreateOrder(Order order)
    {
        order.Lines = GetCart().Selections.Select(e => new OrderLine
        {
            ProductId = e.ProductId,
            Quantity = e.Quantity
        }).ToArray();
        _order.AddOrder(order);
        SaveCart(new Cart());
        return RedirectToAction(nameof(Completed));
    }
    
    public IViewComponentResult Invoke(ISession session)
    {
        return new ViewViewComponentResult()
        {
            ViewData = new ViewDataDictionary<Cart>(ViewData, session.GetJson<Cart>("Cart"))
        };
    }
}