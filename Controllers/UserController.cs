using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Request;
using RAS.Bootcamp.Katalog.MVC.NET.Models;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace RAS.Bootcamp.MVC.NET.Controllers;

public class UserController : Controller
{
    private readonly dbmarketContext _dbcontext;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, dbmarketContext dbContext)
    {
        _dbcontext = dbContext;
        _logger = logger;
        
    }
  public IActionResult Login()
    {

        return View();
    }
    [HttpPost]
      public async Task<IActionResult> Login(LoginRequest login)
    {
        if (!ModelState.IsValid){
            return View(login);
        }

        var user = _dbcontext.Users.FirstOrDefault(x=>x.Username == login.Username);

        if (user == null){
            ViewBag.ErrorMessage = "Invalid Username or Password";
            return View(login);
        }

        if (user.Tipe != "Pembeli"){
            ViewBag.ErrorMessage = "You're not Pembeli";
            return View(login);
        }

        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name,user.Username),
            new Claim("Fullname",user.Username),
            new Claim(ClaimTypes.Role,user.Tipe),
            new Claim("ID",user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme
        );

        var authProperties = new AuthenticationProperties(){
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20), 
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);
        

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout(){
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index","Home");
    }

    public IActionResult Register(){
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterRequest request){
        if(!ModelState.IsValid){

            return View(request);
        }

        var newUser = new Katalog.MVC.NET.Models.Entities.User{
            Username = request.Username,
            Password = request.Password,
            Tipe = request.Tipe
        };
        if (request.Tipe == "Admin"){
            _dbcontext.Users.Add(newUser);
            _dbcontext.SaveChanges();
        }
        
        if (request.Tipe == "Penjual"){
            var penjual = new Katalog.MVC.NET.Models.Entities.Penjual{
            IdUser = newUser.Id,
            AlamatToko = request.Alamat,
            NamaToko = $"{request.FullName} Store",
            NoHp = request.NoHp,
            IdUserNavigation = newUser
        };

        _dbcontext.Users.Add(newUser);
        _dbcontext.Penjuals.Add(penjual);
        _dbcontext.SaveChanges();
        }
        
        if (request.Tipe == "Pembeli"){
            var pembeli = new Katalog.MVC.NET.Models.Entities.Pembeli{
                IdUser = newUser.Id,
                AlamatPembeli = request.Alamat,
                NamaPembeli = request.FullName,
                NoHp = request.NoHp,
                IdUserNavigation = newUser
            };

            _dbcontext.Users.Add(newUser);
            _dbcontext.Pembelis.Add(pembeli);
            _dbcontext.SaveChanges();
        }
        
        return RedirectToAction("Login");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}