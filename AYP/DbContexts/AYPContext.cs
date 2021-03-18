﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.DbContext
{
    using global::AYP.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    namespace AYP.DbContexts
    {
        public partial class AYPContext : DbContext
        {
            public AYPContext() : base() { }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-86PG2PU\SQL;Integrated Security=True;Database=Ayp");    
            }

            public DbSet<KL_Tip> KL_Tip { get; set; }
            public DbSet<KL_FizikselOrtam> KL_FizikselOrtam { get; set; }
            public DbSet<KL_GerilimTipi> KL_GerilimTipi { get; set; }
            public DbSet<KL_KullanimAmaci> KL_KullanimAmaci { get; set; }
            public DbSet<KL_Kapasite> KL_Kapasite { get; set; }
            public DbSet<UcBirimTur> UcBirimTur { get; set; }
            public DbSet<AgAnahtariTur> AgAnahtariTur { get; set; }
            public DbSet<GucUreticiTur> GucUreticiTur { get; set; }
            public DbSet<UcBirim> UcBirim { get; set; }
            public DbSet<AgAnahtari> AgAnahtari { get; set; }
            public DbSet<GucUretici> GucUretici { get; set; }
            public DbSet<GucArayuzu> GucArayuzu { get; set; }
            public DbSet<UcBirimGucArayuzu> UcBirimGucArayuzu { get; set; }
            public DbSet<AgAnahtariGucArayuzu> AgAnahtariGucArayuzu { get; set; }
            public DbSet<GucUreticiGucArayuzu> GucUreticiGucArayuzu { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<KL_Tip>().HasData(
                    new KL_Tip() { Ad = "Uç Birim", Id = 1 },
                    new KL_Tip() { Ad = "Ağ Anahtarı", Id = 2 },
                    new KL_Tip() { Ad = "Güç Üretici", Id = 3 },
                    new KL_Tip() { Ad = "Uç Birim Ağ Arayüzü", Id = 4 },
                    new KL_Tip() { Ad = "Ağ Anahtarı Ağ Arayüzü", Id = 5 },
                    new KL_Tip() { Ad = "Uç Birim Güç Arayüzü", Id = 6 },
                    new KL_Tip() { Ad = "Ağ Anahtarı Güç Arayüzü", Id = 7 },
                    new KL_Tip() { Ad = "Güç Üretici Güç Arayüzü", Id = 8 }
                );

                modelBuilder.Entity<KL_GerilimTipi>().HasData(
                    new KL_GerilimTipi() { Ad = "AC", Id = 1 },
                    new KL_GerilimTipi() { Ad = "DC", Id = 2 }
                );

                modelBuilder.Entity<KL_KullanimAmaci>().HasData(
                    new KL_KullanimAmaci() { Ad = "Girdi", Id = 1 },
                    new KL_KullanimAmaci() { Ad = "Çıktı", Id = 2 }
                );

                modelBuilder.Entity<KL_FizikselOrtam>().HasData(
                    new KL_FizikselOrtam() { Ad = "Bakır", Id = 1 },
                    new KL_FizikselOrtam() { Ad = "Fiber Optik", Id = 2 }
                );

                modelBuilder.Entity<KL_Kapasite>().HasData(
                   new KL_Kapasite() { Ad = "Ethernet", Id = 1 },
                   new KL_Kapasite() { Ad = "Fast Ethernet", Id = 2 },
                   new KL_Kapasite() { Ad = "10-Gigabit Ethernet", Id = 3 },
                   new KL_Kapasite() { Ad = "40-Gigabit Ethernet", Id = 4 }
                );
            }

            public void Reset()
            {
                var entries = this.ChangeTracker
                                               .Entries()
                                               .Where(e => e.State != EntityState.Unchanged)
                                               .ToList();
                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                    }
                }
            }
        }
    }
}
