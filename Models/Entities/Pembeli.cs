using System;
using System.Collections.Generic;

namespace RAS.Bootcamp.Katalog.MVC.NET.Models.Entities
{
    public partial class Pembeli
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string? NamaPembeli { get; set; }
        public string? AlamatPembeli { get; set; }
        public string? NoHp { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
