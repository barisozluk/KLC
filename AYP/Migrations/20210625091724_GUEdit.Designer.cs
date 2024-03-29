﻿// <auto-generated />
using System;
using AYP.DbContext.AYP.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AYP.Migrations
{
    [DbContext(typeof(AYPContext))]
    [Migration("20210625091724_GUEdit")]
    partial class GUEdit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AYP.Entities.AgAnahtari", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgAnahtariTurId")
                        .HasColumnType("int");

                    b.Property<int>("CiktiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GirdiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GucArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<byte[]>("Katalog")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("KatalogDosyaAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Sembol")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SembolDosyaAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StokNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tanim")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("TipId")
                        .HasColumnType("int");

                    b.Property<string>("UreticiAdi")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("UreticiParcaNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AgAnahtariTurId");

                    b.HasIndex("TipId");

                    b.ToTable("AgAnahtari");
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtariAgArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgAnahtariId")
                        .HasColumnType("int");

                    b.Property<int>("AgArayuzuId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgAnahtariId");

                    b.HasIndex("AgArayuzuId");

                    b.ToTable("AgAnahtariAgArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtariGucArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgAnahtariId")
                        .HasColumnType("int");

                    b.Property<int>("GucArayuzuId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgAnahtariId");

                    b.HasIndex("GucArayuzuId");

                    b.ToTable("AgAnahtariGucArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtariTur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("AgAnahtariTur");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Kenar"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "Toplama"
                        },
                        new
                        {
                            Id = 3,
                            Ad = "Omurga"
                        });
                });

            modelBuilder.Entity("AYP.Entities.AgArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FizikselOrtamId")
                        .HasColumnType("int");

                    b.Property<int>("KapasiteId")
                        .HasColumnType("int");

                    b.Property<int>("KullanimAmaciId")
                        .HasColumnType("int");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TipId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FizikselOrtamId");

                    b.HasIndex("KapasiteId");

                    b.HasIndex("KullanimAmaciId");

                    b.HasIndex("TipId");

                    b.ToTable("AgArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.GucArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("CiktiDuraganGerilimDegeri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CiktiUrettigiGucKapasitesi")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("GerilimTipiId")
                        .HasColumnType("int");

                    b.Property<decimal?>("GirdiDuraganGerilimDegeri1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GirdiDuraganGerilimDegeri2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GirdiDuraganGerilimDegeri3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GirdiMaksimumGerilimDegeri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GirdiMinimumGerilimDegeri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("GirdiTukettigiGucMiktari")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("KullanimAmaciId")
                        .HasColumnType("int");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TipId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GerilimTipiId");

                    b.HasIndex("KullanimAmaciId");

                    b.HasIndex("TipId");

                    b.ToTable("GucArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.GucUretici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CiktiGucArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<decimal?>("DahiliGucTuketimDegeri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("GirdiGucArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GucUreticiTurId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Katalog")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("KatalogDosyaAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Sembol")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SembolDosyaAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StokNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tanim")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("TipId")
                        .HasColumnType("int");

                    b.Property<string>("UreticiAdi")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("UreticiParcaNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("VerimlilikDegeri")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("GucUreticiTurId");

                    b.HasIndex("TipId");

                    b.ToTable("GucUretici");
                });

            modelBuilder.Entity("AYP.Entities.GucUreticiGucArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GucArayuzuId")
                        .HasColumnType("int");

                    b.Property<int>("GucUreticiId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GucArayuzuId");

                    b.HasIndex("GucUreticiId");

                    b.ToTable("GucUreticiGucArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.GucUreticiTur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("GucUreticiTur");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Şebeke"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "Güç Kaynağı"
                        },
                        new
                        {
                            Id = 3,
                            Ad = "Kesintisiz Güç Kaynağı"
                        });
                });

            modelBuilder.Entity("AYP.Entities.KL_FizikselOrtam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KL_FizikselOrtam");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Bakır"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "Fiber Optik"
                        });
                });

            modelBuilder.Entity("AYP.Entities.KL_GerilimTipi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KL_GerilimTipi");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "AC"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "DC"
                        });
                });

            modelBuilder.Entity("AYP.Entities.KL_Kapasite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxKapasite")
                        .HasColumnType("int");

                    b.Property<int>("MinKapasite")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("KL_Kapasite");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "10-Megabit Ethernet",
                            MaxKapasite = 10,
                            MinKapasite = 0
                        },
                        new
                        {
                            Id = 2,
                            Ad = "100-Megabit Ethernet",
                            MaxKapasite = 100,
                            MinKapasite = 10
                        },
                        new
                        {
                            Id = 3,
                            Ad = "Gigabit Ethernet",
                            MaxKapasite = 1000,
                            MinKapasite = 100
                        },
                        new
                        {
                            Id = 4,
                            Ad = "10-Gigabit Ethernet",
                            MaxKapasite = 10000,
                            MinKapasite = 1000
                        },
                        new
                        {
                            Id = 5,
                            Ad = "40-Gigabit Ethernet",
                            MaxKapasite = 40000,
                            MinKapasite = 10000
                        });
                });

            modelBuilder.Entity("AYP.Entities.KL_KullanimAmaci", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KL_KullanimAmaci");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Girdi"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "Çıktı"
                        });
                });

            modelBuilder.Entity("AYP.Entities.KL_Tip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("KL_Tip");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Uç Birim"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "Ağ Anahtarı"
                        },
                        new
                        {
                            Id = 3,
                            Ad = "Güç Üretici"
                        },
                        new
                        {
                            Id = 4,
                            Ad = "Uç Birim Ağ Arayüzü"
                        },
                        new
                        {
                            Id = 5,
                            Ad = "Ağ Anahtarı Ağ Arayüzü"
                        },
                        new
                        {
                            Id = 6,
                            Ad = "Uç Birim Güç Arayüzü"
                        },
                        new
                        {
                            Id = 7,
                            Ad = "Ağ Anahtarı Güç Arayüzü"
                        },
                        new
                        {
                            Id = 8,
                            Ad = "Güç Üretici Güç Arayüzü"
                        });
                });

            modelBuilder.Entity("AYP.Entities.UcBirim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CiktiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int?>("GirdiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GucArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<byte[]>("Katalog")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("KatalogDosyaAdi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Sembol")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SembolDosyaAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StokNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tanim")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("TipId")
                        .HasColumnType("int");

                    b.Property<int>("UcBirimTurId")
                        .HasColumnType("int");

                    b.Property<string>("UreticiAdi")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("UreticiParcaNo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("TipId");

                    b.HasIndex("UcBirimTurId");

                    b.ToTable("UcBirim");
                });

            modelBuilder.Entity("AYP.Entities.UcBirimAgArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgArayuzuId")
                        .HasColumnType("int");

                    b.Property<int>("UcBirimId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgArayuzuId");

                    b.HasIndex("UcBirimId");

                    b.ToTable("UcBirimAgArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.UcBirimGucArayuzu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GucArayuzuId")
                        .HasColumnType("int");

                    b.Property<int>("UcBirimId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GucArayuzuId");

                    b.HasIndex("UcBirimId");

                    b.ToTable("UcBirimGucArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.UcBirimTur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("UcBirimTur");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ad = "Kamera"
                        },
                        new
                        {
                            Id = 2,
                            Ad = "NVR"
                        },
                        new
                        {
                            Id = 3,
                            Ad = "Video Wall"
                        },
                        new
                        {
                            Id = 4,
                            Ad = "Sunucu"
                        });
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtari", b =>
                {
                    b.HasOne("AYP.Entities.AgAnahtariTur", "AgAnahtariTur")
                        .WithMany()
                        .HasForeignKey("AgAnahtariTurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_Tip", "KL_Tip")
                        .WithMany()
                        .HasForeignKey("TipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgAnahtariTur");

                    b.Navigation("KL_Tip");
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtariAgArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.AgAnahtari", "AgAnahtari")
                        .WithMany()
                        .HasForeignKey("AgAnahtariId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.AgArayuzu", "AgArayuzu")
                        .WithMany()
                        .HasForeignKey("AgArayuzuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgAnahtari");

                    b.Navigation("AgArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.AgAnahtariGucArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.AgAnahtari", "AgAnahtari")
                        .WithMany()
                        .HasForeignKey("AgAnahtariId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.GucArayuzu", "GucArayuzu")
                        .WithMany()
                        .HasForeignKey("GucArayuzuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgAnahtari");

                    b.Navigation("GucArayuzu");
                });

            modelBuilder.Entity("AYP.Entities.AgArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.KL_FizikselOrtam", "KL_FizikselOrtam")
                        .WithMany()
                        .HasForeignKey("FizikselOrtamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_Kapasite", "KL_Kapasite")
                        .WithMany()
                        .HasForeignKey("KapasiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_KullanimAmaci", "KL_KullanimAmaci")
                        .WithMany()
                        .HasForeignKey("KullanimAmaciId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_Tip", "KL_Tip")
                        .WithMany()
                        .HasForeignKey("TipId");

                    b.Navigation("KL_FizikselOrtam");

                    b.Navigation("KL_Kapasite");

                    b.Navigation("KL_KullanimAmaci");

                    b.Navigation("KL_Tip");
                });

            modelBuilder.Entity("AYP.Entities.GucArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.KL_GerilimTipi", "KL_GerilimTipi")
                        .WithMany()
                        .HasForeignKey("GerilimTipiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_KullanimAmaci", "KL_KullanimAmaci")
                        .WithMany()
                        .HasForeignKey("KullanimAmaciId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_Tip", "KL_Tip")
                        .WithMany()
                        .HasForeignKey("TipId");

                    b.Navigation("KL_GerilimTipi");

                    b.Navigation("KL_KullanimAmaci");

                    b.Navigation("KL_Tip");
                });

            modelBuilder.Entity("AYP.Entities.GucUretici", b =>
                {
                    b.HasOne("AYP.Entities.GucUreticiTur", "GucUreticiTur")
                        .WithMany()
                        .HasForeignKey("GucUreticiTurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.KL_Tip", "KL_Tip")
                        .WithMany()
                        .HasForeignKey("TipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GucUreticiTur");

                    b.Navigation("KL_Tip");
                });

            modelBuilder.Entity("AYP.Entities.GucUreticiGucArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.GucArayuzu", "GucArayuzu")
                        .WithMany()
                        .HasForeignKey("GucArayuzuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.GucUretici", "GucUretici")
                        .WithMany()
                        .HasForeignKey("GucUreticiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GucArayuzu");

                    b.Navigation("GucUretici");
                });

            modelBuilder.Entity("AYP.Entities.UcBirim", b =>
                {
                    b.HasOne("AYP.Entities.KL_Tip", "KL_Tip")
                        .WithMany()
                        .HasForeignKey("TipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.UcBirimTur", "UcBirimTur")
                        .WithMany()
                        .HasForeignKey("UcBirimTurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KL_Tip");

                    b.Navigation("UcBirimTur");
                });

            modelBuilder.Entity("AYP.Entities.UcBirimAgArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.AgArayuzu", "AgArayuzu")
                        .WithMany()
                        .HasForeignKey("AgArayuzuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.UcBirim", "UcBirim")
                        .WithMany()
                        .HasForeignKey("UcBirimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgArayuzu");

                    b.Navigation("UcBirim");
                });

            modelBuilder.Entity("AYP.Entities.UcBirimGucArayuzu", b =>
                {
                    b.HasOne("AYP.Entities.GucArayuzu", "GucArayuzu")
                        .WithMany()
                        .HasForeignKey("GucArayuzuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AYP.Entities.UcBirim", "UcBirim")
                        .WithMany()
                        .HasForeignKey("UcBirimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GucArayuzu");

                    b.Navigation("UcBirim");
                });
#pragma warning restore 612, 618
        }
    }
}
