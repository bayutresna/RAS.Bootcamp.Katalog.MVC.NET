using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;

namespace RAS.Bootcamp.Katalog.MVC.NET.Models
{
    public partial class dbmarketContext : DbContext
    {
        public dbmarketContext()
        {
        }

        public dbmarketContext(DbContextOptions<dbmarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Barang> Barangs { get; set; } = null!;
        public virtual DbSet<ItemTransaksi> ItemTransakses { get; set; } = null!;
        public virtual DbSet<Keranjang> Keranjangs { get; set; } = null!;
        public virtual DbSet<Pembeli> Pembelis { get; set; } = null!;
        public virtual DbSet<Penjual> Penjuals { get; set; } = null!;
        public virtual DbSet<Transaksi> Transakses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server = localhost; port = 5432; Database=dbmarket; User Id= postgres; Password = skadi;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Barang>(entity =>
            {
                entity.HasIndex(e => e.IdPenjual, "IX_Barangs_IdPenjual");

                entity.Property(e => e.Imgname)
                    .HasMaxLength(250)
                    .HasColumnName("imgname")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.Kode).HasMaxLength(10);

                entity.Property(e => e.Nama).HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .HasColumnName("url")
                    .HasDefaultValueSql("''::text");

                entity.HasOne(d => d.IdPenjualNavigation)
                    .WithMany(p => p.Barangs)
                    .HasForeignKey(d => d.IdPenjual);
            });

            modelBuilder.Entity<ItemTransaksi>(entity =>
            {
                entity.ToTable("ItemTransaksis");

                entity.HasIndex(e => e.IdBarang, "IX_ItemTransaksis_IdBarang");

                entity.HasIndex(e => e.IdTransaksi, "IX_ItemTransaksis_IdTransaksi");

                entity.HasOne(d => d.IdBarangNavigation)
                    .WithMany(p => p.ItemTransaksis)
                    .HasForeignKey(d => d.IdBarang);

                entity.HasOne(d => d.IdTransaksiNavigation)
                    .WithMany(p => p.ItemTransaksis)
                    .HasForeignKey(d => d.IdTransaksi);
            });

            modelBuilder.Entity<Keranjang>(entity =>
            {
                entity.HasIndex(e => e.IdBarang, "IX_Keranjangs_IdBarang")
                    .IsUnique();

                entity.HasIndex(e => e.IdUser, "IX_Keranjangs_IdUser");

                entity.HasOne(d => d.IdBarangNavigation)
                    .WithOne(p => p.Keranjang)
                    .HasForeignKey<Keranjang>(d => d.IdBarang);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Keranjangs)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Pembeli>(entity =>
            {
                entity.HasIndex(e => e.IdUser, "IX_Pembelis_IdUser");

                entity.Property(e => e.NoHp).HasColumnName("NoHP");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Pembelis)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Penjual>(entity =>
            {
                entity.HasIndex(e => e.IdUser, "IX_Penjuals_IdUser");

                entity.Property(e => e.NoHp).HasColumnName("NoHP");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Penjuals)
                    .HasForeignKey(d => d.IdUser);
            });

            modelBuilder.Entity<Transaksi>(entity =>
            {
                entity.ToTable("Transaksis");

                entity.HasIndex(e => e.IdUser, "IX_Transaksis_IdUser");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Transaksis)
                    .HasForeignKey(d => d.IdUser);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
