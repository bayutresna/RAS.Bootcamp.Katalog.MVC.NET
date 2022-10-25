using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAS.Bootcamp.Katalog.MVC.NET.Models;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;

namespace RAS.Bootcamp.Katalog.MVC.NET.Controllers;

public class TransaksiController : Controller
{
    private readonly ILogger<TransaksiController> _logger;
    private readonly dbmarketContext _dbcontext;
    public TransaksiController(ILogger<TransaksiController> logger,dbmarketContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    public IActionResult DaftarKeranjang(){
        var dataiduser= int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Keranjang> kr = _dbcontext.Keranjangs.Include(e=> e.IdBarangNavigation).ThenInclude(e=> e.IdPenjualNavigation).Include(e=> e.IdUserNavigation).Where(e=> e.IdUser == dataiduser).ToList();
        
        return View(kr);
    }
    public IActionResult DeleteKeranjang(int id){
        var dataiduser= int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        Keranjang delete = _dbcontext.Keranjangs.First(e=> e.Id == id);
        _dbcontext.Keranjangs.Remove(delete);
        _dbcontext.SaveChanges();
        List<Keranjang> kr = _dbcontext.Keranjangs.Include(e=> e.IdBarangNavigation).ThenInclude(e=> e.IdPenjualNavigation).Include(e=> e.IdUserNavigation).Where(e=> e.IdUser == dataiduser).ToList();
        return View("DaftarKeranjang",kr);
    }
    public IActionResult ClearKeranjang(){
        var dataiduser= int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Keranjang> kr = _dbcontext.Keranjangs.Where(e=> e.IdUser == dataiduser).ToList();
        _dbcontext.Keranjangs.RemoveRange(kr);
        _dbcontext.SaveChanges();
        List<Keranjang> upkr = _dbcontext.Keranjangs.Include(e=> e.IdBarangNavigation).ThenInclude(e=> e.IdPenjualNavigation).Include(e=> e.IdUserNavigation).Where(e=> e.IdUser == dataiduser).ToList();
        return View("DaftarKeranjang",upkr);
    }
    [HttpPost]
    public IActionResult UpdateKeranjang(int id, int jumlah){
        Keranjang kr = _dbcontext.Keranjangs.First(e=> e.Id == id);
        kr.Jumlah = jumlah;
        _dbcontext.SaveChanges();

        var dataiduser= int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Keranjang> upkr = _dbcontext.Keranjangs.Include(e=> e.IdBarangNavigation).ThenInclude(e=> e.IdPenjualNavigation).Include(e=> e.IdUserNavigation).Where(e=> e.IdUser == dataiduser).ToList();
        return View("DaftarKeranjang",upkr);
    }

    public IActionResult CheckOut(){

        return View();
    }
}