using System;
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
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-86JLPO4\SQL;Integrated Security=True;Database=Ayp");    
            }

            public DbSet<KL_Tip> KL_Tip { get; set; }
            public DbSet<UcBirimTur> UcBirimTur { get; set; }
            public DbSet<AgAnahtariTur> AgAnahtariTur { get; set; }
            public DbSet<GucUreticiTur> GucUreticiTur { get; set; }
            public DbSet<UcBirim> UcBirim { get; set; }
            public DbSet<AgAnahtari> AgAnahtari { get; set; }
            public DbSet<GucUretici> GucUretici { get; set; }

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
