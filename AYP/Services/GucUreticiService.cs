using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AYP.Services
{
    public class GucUreticiService : IGucUreticiService
    {
        private readonly AYPContext context;
        public GucUreticiService(AYPContext context)
        {
            this.context = context;
        }

        public GucUretici GetGucUreticiById(int gucUreticiId)
        {
            GucUretici response = new GucUretici();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.GucUretici.Include(x => x.GucUreticiTur).Where(gu => gu.Id == gucUreticiId).FirstOrDefault();
                }
                catch (Exception exception)
                {

                }
            }

            return response;
        }

        public List<GucUretici> ListGucUretici()
        {
            List<GucUretici> response = new List<GucUretici>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.GucUretici.ToList();

                    foreach (var item in response)
                    {
                        item.SembolSrc = ByteToImage(item.Sembol);
                    }
                }
                catch (Exception exception)
                {

                }
            }

            return response;
        }

        public List<GucUreticiTur> ListGucUreticiTur()
        {
            List<GucUreticiTur> response = new List<GucUreticiTur>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.GucUreticiTur.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {

                }
            }

            return response;
        }

        public ResponseModel SaveGucUretici(GucUretici gucUretici, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    GucUretici dbItem = new GucUretici();
                    context.GucUretici.Add(dbItem);
                    dbItem.CiktiGucArayuzuSayisi = gucUretici.CiktiGucArayuzuSayisi;
                    dbItem.GirdiGucArayuzuSayisi = gucUretici.GirdiGucArayuzuSayisi;
                    dbItem.Katalog = gucUretici.Katalog;
                    dbItem.KatalogDosyaAdi = gucUretici.KatalogDosyaAdi;
                    dbItem.Sembol = gucUretici.Sembol;
                    dbItem.SembolDosyaAdi = gucUretici.SembolDosyaAdi;
                    dbItem.StokNo = gucUretici.StokNo;
                    dbItem.Tanim = gucUretici.Tanim;
                    dbItem.TipId = gucUretici.TipId;
                    dbItem.GucUreticiTurId = gucUretici.GucUreticiTurId;
                    dbItem.UreticiAdi = gucUretici.UreticiAdi;
                    dbItem.UreticiParcaNo = gucUretici.UreticiParcaNo;
                    dbItem.DahiliGucTuketimDegeri = gucUretici.DahiliGucTuketimDegeri;
                    dbItem.VerimlilikDegeri = gucUretici.VerimlilikDegeri;

                    context.SaveChanges();

                    foreach(var gucArayuzu in gucArayuzuList)
                    {
                        GucArayuzu dbItemGucArayuzu = new GucArayuzu();
                        context.GucArayuzu.Add(dbItemGucArayuzu);
                        dbItemGucArayuzu.CiktiDuraganGerilimDegeri = gucArayuzu.CiktiDuraganGerilimDegeri;
                        dbItemGucArayuzu.CiktiUrettigiGucKapasitesi = gucArayuzu.CiktiUrettigiGucKapasitesi;
                        dbItemGucArayuzu.GirdiDuraganGerilimDegeri1 = gucArayuzu.GirdiDuraganGerilimDegeri1;
                        dbItemGucArayuzu.GirdiDuraganGerilimDegeri2 = gucArayuzu.GirdiDuraganGerilimDegeri2;
                        dbItemGucArayuzu.GirdiDuraganGerilimDegeri3 = gucArayuzu.GirdiDuraganGerilimDegeri3;
                        dbItemGucArayuzu.GirdiMaksimumGerilimDegeri = gucArayuzu.GirdiMaksimumGerilimDegeri;
                        dbItemGucArayuzu.GirdiMinimumGerilimDegeri = gucArayuzu.GirdiMinimumGerilimDegeri;
                        dbItemGucArayuzu.GirdiTukettigiGucMiktari = gucArayuzu.GirdiTukettigiGucMiktari;
                        dbItemGucArayuzu.GerilimTipiId = gucArayuzu.GerilimTipiId;
                        dbItemGucArayuzu.KullanimAmaciId = gucArayuzu.KullanimAmaciId;
                        dbItemGucArayuzu.TipId = gucArayuzu.TipId;
                        dbItemGucArayuzu.Adi = gucArayuzu.Adi;
                        dbItemGucArayuzu.Port = gucArayuzu.Port;
                        context.SaveChanges();

                        GucUreticiGucArayuzu dbItem1 = new GucUreticiGucArayuzu();
                        context.GucUreticiGucArayuzu.Add(dbItem1);
                        dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                        dbItem1.GucUreticiId = dbItem.Id;
                        context.SaveChanges();
                    }

                    response.SetSuccess();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    context.Reset();
                    response.SetError(exception.Message);
                    transaction.Rollback();
                }
            }

            return response;
        }

        public ResponseModel UpdateGucUretici(GucUretici gucUretici)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var dbItem = context.GucUretici.Where(gu => gu.Id == gucUretici.Id).FirstOrDefault();

                    dbItem.CiktiGucArayuzuSayisi = gucUretici.CiktiGucArayuzuSayisi;
                    dbItem.GirdiGucArayuzuSayisi = gucUretici.GirdiGucArayuzuSayisi;
                    dbItem.Katalog = gucUretici.Katalog;
                    dbItem.KatalogDosyaAdi = gucUretici.KatalogDosyaAdi;
                    dbItem.Sembol = gucUretici.Sembol;
                    dbItem.SembolDosyaAdi = gucUretici.SembolDosyaAdi;
                    dbItem.StokNo = gucUretici.StokNo;
                    dbItem.Tanim = gucUretici.Tanim;
                    dbItem.GucUreticiTurId = gucUretici.GucUreticiTurId;
                    dbItem.UreticiAdi = gucUretici.UreticiAdi;
                    dbItem.UreticiParcaNo = gucUretici.UreticiParcaNo;
                    dbItem.DahiliGucTuketimDegeri = gucUretici.DahiliGucTuketimDegeri;
                    dbItem.VerimlilikDegeri = gucUretici.VerimlilikDegeri;

                    context.SaveChanges();
                    response.SetSuccess();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    context.Reset();
                    response.SetError(exception.Message);
                    transaction.Rollback();
                }
            }

            return response;
        }

        public ResponseModel SaveGucUreticiTur(GucUreticiTur gucUreticiTur)
        {
            ResponseModel response = new ResponseModel();

            using(var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    GucUreticiTur dbItem = new GucUreticiTur();
                    context.GucUreticiTur.Add(dbItem);
                    dbItem.Ad = gucUreticiTur.Ad;

                    context.SaveChanges();
                    response.SetSuccess();
                    transaction.Commit();
                }
                catch(Exception exception)
                {
                    context.Reset();
                    response.SetError(exception.Message);
                    transaction.Rollback();
                }
            }

            return response;
        }

        private ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi)
        {
            ResponseModel response = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var selectedId in selectedIdList)
                    {
                        var gucUretici = context.GucUretici.Where(x => x.Id == selectedId).FirstOrDefault();
                        gucUretici.UreticiAdi = ureticiAdi;
                        context.SaveChanges();
                    }
                    response.SetSuccess();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    context.Reset();
                    response.SetError(exception.Message);
                    transaction.Rollback();
                }
            }
            return response;
        }
    }
}
