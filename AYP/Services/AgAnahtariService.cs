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
    public class AgAnahtariService : IAgAnahtariService
    {
        private readonly AYPContext context;
        public AgAnahtariService(AYPContext context)
        {
            this.context = context;
        }

        public ResponseModel SaveAgAnahtariTur(AgAnahtariTur agAnahtariTur)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    AgAnahtariTur dbItem = new AgAnahtariTur();
                    context.AgAnahtariTur.Add(dbItem);
                    dbItem.Ad = agAnahtariTur.Ad;

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

        public ResponseModel SaveAgAnahtari(AgAnahtari agAnahtari, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    AgAnahtari dbItem = new AgAnahtari();
                    context.AgAnahtari.Add(dbItem);
                    dbItem.CiktiAgArayuzuSayisi = agAnahtari.CiktiAgArayuzuSayisi;
                    dbItem.GirdiAgArayuzuSayisi = agAnahtari.GirdiAgArayuzuSayisi;
                    dbItem.GucArayuzuSayisi = agAnahtari.GucArayuzuSayisi;
                    dbItem.Katalog = agAnahtari.Katalog;
                    dbItem.KatalogDosyaAdi = agAnahtari.KatalogDosyaAdi;
                    dbItem.Sembol = agAnahtari.Sembol;
                    dbItem.SembolDosyaAdi = agAnahtari.SembolDosyaAdi;
                    dbItem.StokNo = agAnahtari.StokNo;
                    dbItem.Tanim = agAnahtari.Tanim;
                    dbItem.TipId = agAnahtari.TipId;
                    dbItem.AgAnahtariTurId = agAnahtari.AgAnahtariTurId;
                    dbItem.UreticiAdi = agAnahtari.UreticiAdi;
                    dbItem.UreticiParcaNo = agAnahtari.UreticiParcaNo;
                    context.SaveChanges();

                    foreach(var agArayuzu in agArayuzuList)
                    {
                        AgArayuzu dbItemAgArayuzu = new AgArayuzu();
                        context.AgArayuzu.Add(dbItemAgArayuzu);
                        dbItemAgArayuzu.FizikselOrtamId = agArayuzu.FizikselOrtamId;
                        dbItemAgArayuzu.KapasiteId = agArayuzu.KapasiteId;
                        dbItemAgArayuzu.KullanimAmaciId = agArayuzu.KullanimAmaciId;
                        dbItemAgArayuzu.TipId = agArayuzu.TipId;
                        dbItemAgArayuzu.Adi = agArayuzu.Adi;
                        dbItemAgArayuzu.Port = agArayuzu.Port;
                        context.SaveChanges();

                        AgAnahtariAgArayuzu dbItem1 = new AgAnahtariAgArayuzu();
                        context.AgAnahtariAgArayuzu.Add(dbItem1);
                        dbItem1.AgArayuzuId = dbItemAgArayuzu.Id;
                        dbItem1.AgAnahtariId = dbItem.Id;
                        context.SaveChanges();
                    }

                    foreach (var gucArayuzu in gucArayuzuList)
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

                        AgAnahtariGucArayuzu dbItem1 = new AgAnahtariGucArayuzu();
                        context.AgAnahtariGucArayuzu.Add(dbItem1);
                        dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                        dbItem1.AgAnahtariId = dbItem.Id;
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

        public ResponseModel UpdateAgAnahtari(AgAnahtari agAnahtari)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var dbItem = context.AgAnahtari.Where(aa => aa.Id == agAnahtari.Id).FirstOrDefault();

                    dbItem.CiktiAgArayuzuSayisi = agAnahtari.CiktiAgArayuzuSayisi;
                    dbItem.GirdiAgArayuzuSayisi = agAnahtari.GirdiAgArayuzuSayisi;
                    dbItem.GucArayuzuSayisi = agAnahtari.GucArayuzuSayisi;
                    dbItem.Katalog = agAnahtari.Katalog;
                    dbItem.KatalogDosyaAdi = agAnahtari.KatalogDosyaAdi;
                    dbItem.Sembol = agAnahtari.Sembol;
                    dbItem.SembolDosyaAdi = agAnahtari.SembolDosyaAdi;
                    dbItem.StokNo = agAnahtari.StokNo;
                    dbItem.Tanim = agAnahtari.Tanim;
                    dbItem.AgAnahtariTurId = agAnahtari.AgAnahtariTurId;
                    dbItem.UreticiAdi = agAnahtari.UreticiAdi;
                    dbItem.UreticiParcaNo = agAnahtari.UreticiParcaNo;

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

        public List<AgAnahtariTur> ListAgAnahtariTur()
        {
            List<AgAnahtariTur> response = new List<AgAnahtariTur>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.AgAnahtariTur.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {

                }
            }

            return response;
        }

        public List<AgAnahtari> ListAgAnahtari()
        {
            List<AgAnahtari> response = new List<AgAnahtari>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.AgAnahtari.ToList();

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

        public AgAnahtari GetAgAnahtariById(int agAnahtariId)
        {
            AgAnahtari response = new AgAnahtari();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.AgAnahtari.Include(x => x.AgAnahtariTur).Where(aa => aa.Id == agAnahtariId).FirstOrDefault();
                }
                catch (Exception exception)
                {

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
                        var agAnahtari = context.AgAnahtari.Where(x => x.Id == selectedId).FirstOrDefault();
                        agAnahtari.UreticiAdi = ureticiAdi;
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
