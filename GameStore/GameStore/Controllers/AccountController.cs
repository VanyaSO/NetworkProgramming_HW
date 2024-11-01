using GameStore.Interfaces;
using GameStore.Models;
using Microsoft.AspNetCore.Mvc;
using GameStore.Infrastructure;

namespace GameStore.Controllers;

public class AccountController : Controller
{
    private readonly IUser _users;

    public AccountController(IUser users)
    {
        _users = users;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        _users.AddUser(user);
        SaveUser(user);
        return RedirectToAction("Index", "Store");
    }
    
    public IActionResult Login()
    {
        return View();
    } 
    
    [HttpPost]
    public IActionResult LoginUser(User user)
    {
        var findUser = _users.GetUserByEmail(user.Email);
        
        if (findUser == null || user.Password != findUser.Password)
        {
            ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
            return View("Login", user);
        }
        
        SaveUser(findUser);
        return RedirectToAction("Index", "Store");
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("User");
        return RedirectToAction("Login", "Account");
    }

    
    private void SaveUser(User user) => HttpContext.Session.SetJson("User", user);
}