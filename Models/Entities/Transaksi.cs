using System;
using System.Collections.Generic;

namespace RAS.Bootcamp.Katalog.MVC.NET.Models.Entities
{
    public partial class Transaksi
    {
        public Transaksi()
        {
            ItemTransaksis = new HashSet<ItemTransaksi>();
        }

        public int Id { get; set; }
        public int IdUser { get; set; }
        public decimal TotalHarga { get; set; }
        public string MetodePembayaran { get; set; } = null!;
        public string StatusTransaksi { get; set; } = null!;
        public string StatusBayar { get; set; } = null!;
        public string AlamatPengiriman { get; set; } = null!;
        public int IdPenjual { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<ItemTransaksi> ItemTransaksis { get; set; }
    }
}
