using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Request;
using RAS.Bootcamp.Katalog.MVC.NET.Models;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RAS.Bootcamp.Katalog.MVC.NET.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly dbmarketContext _dbcontext;
    public HomeController(ILogger<HomeController> logger,dbmarketContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    public IActionResult Index()
    {
        List<Barang> br = _dbcontext.Barangs.Include(e=> e.IdPenjualNavigation).ToList();
        return View(br);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize (Roles = "Pembeli")]
    public IActionResult MasukKeranjang(int id) {
        int userid = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        // Keranjang cekBarang = _dbcontext.Keranjangs.FirstOrDefault(x=> x.IdBarang == id & x.IdUser == userid);
        
        // if (cekBarang != null){
        //     cekBarang.Jumlah = cekBarang.Jumlah + 1;
        //     _dbcontext.SaveChanges();

        // List<Barang> listbr = _dbcontext.Barangs.Include(e=> e.IdPenjualNavigation).ToList();
        // return View("Index",listbr);
        // }

        Barang br = _dbcontext.Barangs.First(x => x.Id == id);
        Keranjang kr = new Keranjang{
            IdBarang = id,
            IdUser = userid,
            HargaSatuan = br.Harga,
            Jumlah = 1,
        };
        _dbcontext.Keranjangs.Add(kr);
        _dbcontext.SaveChanges();

        List<Barang> barang = _dbcontext.Barangs.Include(e=> e.IdPenjualNavigation).ToList();
        return View("Index",barang);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
