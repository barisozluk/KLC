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
    [Migration("20210318070257_AgAnahtari")]
    partial class AgAnahtari
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
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Sembol")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

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

                    b.Property<int>("CiktiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GirdiAgArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<int>("GucArayuzuSayisi")
                        .HasColumnType("int");

                    b.Property<byte[]>("Katalog")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Sembol")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

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
#pragma warning restore 612, 618
        }
    }
}