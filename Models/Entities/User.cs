using System;
using System.Collections.Generic;

namespace RAS.Bootcamp.Katalog.MVC.NET.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Keranjangs = new HashSet<Keranjang>();
            Pembelis = new HashSet<Pembeli>();
            Penjuals = new HashSet<Penjual>();
            Transaksis = new HashSet<Transaksi>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Tipe { get; set; }

        public virtual ICollection<Keranjang> Keranjangs { get; set; }
        public virtual ICollection<Pembeli> Pembelis { get; set; }
        public virtual ICollection<Penjual> Penjuals { get; set; }
        public virtual ICollection<Transaksi> Transaksis { get; set; }
    }
}
