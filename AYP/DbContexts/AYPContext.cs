using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.DbContext
{
    using global::AYP.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Configuration;
    using System.Linq;

    namespace AYP.DbContexts
    {
        public partial class AYPContext : DbContext
        {
            public AYPContext() : base() { }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["AYPContext"].ConnectionString);
                //optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLExpress;Integrated Security=True;Database=master");
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
            public DbSet<AgArayuzu> AgArayuzu { get; set; }
            public DbSet<UcBirimAgArayuzu> UcBirimAgArayuzu { get; set; }
            public DbSet<AgAnahtariAgArayuzu> AgAnahtariAgArayuzu { get; set; }

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
                   new KL_Kapasite() { Ad = "10-Megabit Ethernet", Id = 1, MinKapasite = 0, MaxKapasite = 10 },
                   new KL_Kapasite() { Ad = "100-Megabit Ethernet", Id = 2, MinKapasite = 10, MaxKapasite = 100 },
                   new KL_Kapasite() { Ad = "Gigabit Ethernet", Id = 3, MinKapasite = 100, MaxKapasite = 1000 },
                   new KL_Kapasite() { Ad = "10-Gigabit Ethernet", Id = 4, MinKapasite = 1000, MaxKapasite = 10000 },
                   new KL_Kapasite() { Ad = "40-Gigabit Ethernet", Id = 5, MinKapasite = 10000, MaxKapasite = 40000 }
                );

                modelBuilder.Entity<UcBirimTur>().HasData(
                   new UcBirimTur() { Ad = "Kamera", Id = 1 },
                   new UcBirimTur() { Ad = "NVR", Id = 2 },
                   new UcBirimTur() { Ad = "Video Wall", Id = 3 },
                   new UcBirimTur() { Ad = "Sunucu", Id = 4 }
                );

                modelBuilder.Entity<AgAnahtariTur>().HasData(
                   new AgAnahtariTur() { Ad = "Kenar", Id = 1 },
                   new AgAnahtariTur() { Ad = "Toplama", Id = 2 },
                   new AgAnahtariTur() { Ad = "Omurga", Id = 3 }
                );

                modelBuilder.Entity<GucUreticiTur>().HasData(
                   new GucUreticiTur() { Ad = "Şebeke", Id = 1 },
                   new GucUreticiTur() { Ad = "Güç Kaynağı", Id = 2 },
                   new GucUreticiTur() { Ad = "Kesintisiz Güç Kaynağı", Id = 3 }
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
