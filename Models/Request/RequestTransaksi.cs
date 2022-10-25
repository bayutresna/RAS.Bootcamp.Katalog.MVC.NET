namespace RAS.Bootcamp.Katalog.MVC.NET.Models.Request;
public class RequestTransaksi{
    public int Id {get;set;}
    public string namapembeli{get;set;}
    public decimal TotalHarga { get; set; }
    public string MetodePembayaran { get; set; } = null!;
    public string StatusTransaksi { get; set; } = null!;
    public string StatusBayar { get; set; } = null!;
    public string AlamatPengiriman { get; set; } = null!;
}