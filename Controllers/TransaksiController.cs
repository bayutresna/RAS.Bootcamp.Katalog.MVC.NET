using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAS.Bootcamp.Katalog.MVC.NET.Models;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Request;


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
        var dataiduser= int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Keranjang> kr = _dbcontext.Keranjangs.Include(e=> e.IdBarangNavigation).ThenInclude(e=> e.IdPenjualNavigation).Include(e=> e.IdUserNavigation).Where(e=> e.IdUser == dataiduser).ToList();
        return View(kr);
    }



    public IActionResult Pesan (){
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        Pembeli pb = _dbcontext.Pembelis.First(x=> x.IdUser == dataiduser);
        List<Keranjang> kr = _dbcontext.Keranjangs.Where(x=> x.IdUser == dataiduser).ToList();

        var sum = kr.Sum(x => x.Jumlah * x.HargaSatuan);

        Transaksi tr = new Transaksi{
            IdUser = dataiduser,
            TotalHarga = sum,
            MetodePembayaran = "Transfer",
            StatusTransaksi = "MenungguBayar",
            StatusBayar = "Belum Lunas",
            AlamatPengiriman = pb.AlamatPembeli
        };
        _dbcontext.Transakses.Add(tr);
        _dbcontext.SaveChanges();

        var trsementara = _dbcontext.Transakses.First(x=> x.StatusBayar == "Belum Lunas");

        foreach(var item in kr){
            var ItemTransaksi = new ItemTransaksi{
                IdBarang = item.IdBarang,
                Harga = item.HargaSatuan,
                Jumlah = item.Jumlah,
                SubTotal = item.Jumlah*item.HargaSatuan,
                IdTransaksi = trsementara.Id
            };
            _dbcontext.ItemTransakses.Add(ItemTransaksi);
        }

        _dbcontext.Keranjangs.RemoveRange(kr);
        _dbcontext.SaveChanges();
        return RedirectToAction("Index","Home");
    }




    public IActionResult DataTransaksi(){
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Transaksi> tr = _dbcontext.Transakses.Where(x=> x.IdUser == dataiduser).ToList();
        return View(tr);
    }
    public IActionResult DetailTransaksi(int id){
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        var tr = _dbcontext.Transakses.First(x=> x.Id == id);
        var pb = _dbcontext.Pembelis.First(x=> x.IdUser == dataiduser);

        RequestTransaksi reqtr = new RequestTransaksi{
            namapembeli = pb.NamaPembeli,
            TotalHarga = tr.TotalHarga,
            MetodePembayaran = tr.MetodePembayaran,
            StatusTransaksi = tr.StatusTransaksi,
            AlamatPengiriman = tr.AlamatPengiriman,
            StatusBayar = tr.StatusBayar,
            Id = tr.Id
        };
        return View(reqtr);
    }
    public IActionResult BatalTransaksi(int id){
        var tr = _dbcontext.Transakses.First(x=> x.Id == id);
        tr.StatusBayar = "Dibatalkan";
        tr.StatusTransaksi = "Dibatalkan";
        _dbcontext.SaveChanges();
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Transaksi> Ltr = _dbcontext.Transakses.Where(x=> x.IdUser == dataiduser).ToList();
        return View("DataTransaksi",Ltr);
    }

    public IActionResult Pembayaran(int id){
        var tr = _dbcontext.Transakses.First(x=> x.Id == id);
        tr.StatusBayar = "Dibayar";
        tr.StatusTransaksi = "MenungguKonfirmasi";
        _dbcontext.SaveChanges();
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Transaksi> Ltr = _dbcontext.Transakses.Where(x=> x.IdUser == dataiduser).ToList();
        return View("DataTransaksi",Ltr);
    }
    public IActionResult TerimaBarang(int id){
        var tr = _dbcontext.Transakses.First(x=> x.Id == id);
        tr.StatusTransaksi = "Sudah Diterima";
        _dbcontext.SaveChanges();
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Transaksi> Ltr = _dbcontext.Transakses.Where(x=> x.IdUser == dataiduser).ToList();
        return View("DataTransaksi",Ltr);
    }
    public IActionResult SelesaiTransaksi(int id){
        var tr = _dbcontext.Transakses.First(x=> x.Id == id);
        tr.StatusTransaksi = "Pesanan Selesai";
        _dbcontext.SaveChanges();
        int dataiduser = int.Parse(User.Claims.First(e=> e.Type == "ID").Value);
        List<Transaksi> Ltr = _dbcontext.Transakses.Where(x=> x.IdUser == dataiduser).ToList();
        return View("DataTransaksi",Ltr);
    }


}