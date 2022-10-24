using System;
using System.Collections.Generic;

namespace RAS.Bootcamp.Katalog.MVC.NET.Models.Entities
{
    public partial class Penjual
    {
        public Penjual()
        {
            Barangs = new HashSet<Barang>();
        }

        public int Id { get; set; }
        public int IdUser { get; set; }
        public string? NamaToko { get; set; }
        public string? NoHp { get; set; }
        public string? AlamatToko { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<Barang> Barangs { get; set; }
    }
}
