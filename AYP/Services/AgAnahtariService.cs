using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Models;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AgAnahtariService()
        {
        }

        public ResponseModel DeleteAgAnahtari(AgAnahtari agAnahtari)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        AgAnahtari d = context.AgAnahtari.Where(x => x.Id == agAnahtari.Id).FirstOrDefault();
                        context.Remove(d);
                        context.SaveChanges();
                        response.SetSuccess();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        context.Reset();
                        response.SetError(exception.Message);

                        if(exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı veritabanından silinemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı veritabanından silinemedi. - " + exception.Message);
                        }

                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public ResponseModel SaveAgAnahtariTur(AgAnahtariTur agAnahtariTur)
        {
            ResponseModel response = new ResponseModel();
            using (AYPContext context = new AYPContext())
            {
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

                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı türü veritabanına kaydedilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı türü veritabanına kaydedilemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public ResponseModel SaveAgAnahtari(AgAnahtari agAnahtari, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
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

                        foreach (var agArayuzu in agArayuzuList)
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

                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı veritabanına kaydedilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı veritabanına kaydedilemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public ResponseModel UpdateAgAnahtari(AgAnahtari agAnahtari, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
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

                        if (agArayuzuList.Count > 0)
                        {
                            var list = context.AgAnahtariAgArayuzu
                                .Include(x => x.AgArayuzu)
                                .Where(aa => aa.AgAnahtariId == agAnahtari.Id).ToList();

                            if (list.Count > 0)
                            {
                                context.AgAnahtariAgArayuzu.RemoveRange(list);
                                context.AgArayuzu.RemoveRange(list.Select(s => s.AgArayuzu));
                            }

                            foreach (var agArayuzu in agArayuzuList)
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
                                dbItem1.AgAnahtariId = agAnahtari.Id;
                                context.SaveChanges();
                            }
                        }

                        if (gucArayuzuList.Count > 0)
                        {
                            var list = context.AgAnahtariGucArayuzu
                                .Include(x => x.GucArayuzu)
                                .Where(ga => ga.AgAnahtariId == agAnahtari.Id).ToList();

                            if (list.Count > 0)
                            {
                                context.AgAnahtariGucArayuzu.RemoveRange(list);
                                context.GucArayuzu.RemoveRange(list.Select(s => s.GucArayuzu));
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
                                dbItem1.AgAnahtariId = agAnahtari.Id;
                                context.SaveChanges();
                            }
                        }

                        response.SetSuccess();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        context.Reset();
                        response.SetError(exception.Message);

                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı veritabanında güncellenemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı veritabanında güncellenemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public List<AgAnahtariTur> ListAgAnahtariTur()
        {
            List<AgAnahtariTur> response = new List<AgAnahtariTur>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtariTur.OrderBy(o => o.Ad).ToList();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı türleri veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı türleri veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<AgAnahtari> ListAgAnahtari()
        {
            List<AgAnahtari> response = new List<AgAnahtari>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtari
                                        .Include(x => x.AgAnahtariTur)
                                        .ToList();

                        foreach (var item in response)
                        {
                            item.SembolSrc = ByteToImage(item.Sembol);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarları veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarları veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<AgArayuzu> ListAgAnahtariAgArayuzu(int agAnahtariId)
        {
            List<AgArayuzu> response = new List<AgArayuzu>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtariAgArayuzu
                                                .Include(x => x.AgAnahtari).ThenInclude(x => x.AgAnahtariTur)
                                                .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_FizikselOrtam)
                                                .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_KullanimAmaci)
                                                .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_Kapasite)
                                                .Where(aa => aa.AgAnahtariId == agAnahtariId)
                                                .Select(s => s.AgArayuzu)
                                                .ToList();

                        var fizikselOrtamList = context.KL_FizikselOrtam.ToList();
                        var kullanimAmaciList = context.KL_KullanimAmaci.ToList();
                        var kapasiteList = context.KL_Kapasite.ToList();

                        foreach (var item in response)
                        {
                            item.KullanimAmaciList = kullanimAmaciList;
                            item.FizikselOrtamList = fizikselOrtamList;
                            item.KapasiteList = kapasiteList;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı ağ arayüzleri veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı ağ arayüzleri veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<GucArayuzu> ListAgAnahtariGucArayuzu(int agAnahtariId)
        {
            List<GucArayuzu> response = new List<GucArayuzu>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtariGucArayuzu
                                                .Include(x => x.AgAnahtari).ThenInclude(x => x.AgAnahtariTur)
                                                .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_KullanimAmaci)
                                                .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_GerilimTipi)
                                                .Where(ga => ga.AgAnahtariId == agAnahtariId)
                                                .Select(s => s.GucArayuzu)
                                                .ToList();

                        var gerilimTipiList = context.KL_GerilimTipi.ToList();
                        var kullanimAmaciList = context.KL_KullanimAmaci.ToList();

                        foreach (var item in response)
                        {
                            item.KullanimAmaciList = kullanimAmaciList;
                            item.GerilimTipiList = gerilimTipiList;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı güç arayüzleri veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı güç arayüzleri veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public AgAnahtari GetAgAnahtariById(int agAnahtariId)
        {
            AgAnahtari response = new AgAnahtari();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtari.Include(x => x.AgAnahtariTur).Where(aa => aa.Id == agAnahtariId).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public AgAnahtariTur GetAgAnahtariTurById(int agAnahtariTurId)
        {
            AgAnahtariTur response = new AgAnahtariTur();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.AgAnahtariTur.Where(aat => aat.Id == agAnahtariTurId).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı türü veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı türü veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
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

        public ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi, string ureticiParcaNo, string dosyaAdi, byte[] sembol)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        foreach (var selectedId in selectedIdList)
                        {
                            var agAnahtari = context.AgAnahtari.Include(x => x.AgAnahtariTur).Where(x => x.Id == selectedId).FirstOrDefault();
                            agAnahtari.UreticiAdi = ureticiAdi;
                            agAnahtari.UreticiParcaNo = ureticiParcaNo;
                            agAnahtari.SembolDosyaAdi = dosyaAdi;
                            agAnahtari.Sembol = sembol;

                            context.SaveChanges();
                        }

                        response.SetSuccess();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        context.Reset();
                        response.SetError(exception.Message);

                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı toplu güncelleme işlemi gerçekleştirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı toplu güncelleme işlemi gerçekleştirilemedi. - " + exception.Message);
                        }

                        transaction.Rollback();
                    }
                }
            }
            return response;
        }

        public void ImportAgAnahtariLibrary(AgAnahtariTur agAnahtariTur, List<AgAnahtari> agAnahtariList, List<AgAnahtariAgArayuzu> aaAgArayuzuList, List<AgArayuzu> agArayuzuList, List<AgAnahtariGucArayuzu> aaGucArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        AgAnahtariTur agAnahtariTurItem = context.AgAnahtariTur.Where(aat => aat.Ad.ToLower() == agAnahtariTur.Ad.ToLower()).FirstOrDefault();

                        if (agAnahtariTurItem == null)
                        {
                            agAnahtariTurItem = new AgAnahtariTur();
                            context.AgAnahtariTur.Add(agAnahtariTurItem);
                            agAnahtariTurItem.Ad = agAnahtariTur.Ad;

                            context.SaveChanges();
                        }

                        foreach (var agAnahtari in agAnahtariList)
                        {
                            AgAnahtari agAnahtariItem = context.AgAnahtari.Include(x => x.AgAnahtariTur).Where(aa => aa.StokNo == agAnahtari.StokNo).FirstOrDefault();

                            if (agAnahtariItem == null)
                            {
                                agAnahtariItem = new AgAnahtari();
                                context.AgAnahtari.Add(agAnahtariItem);
                                agAnahtariItem.CiktiAgArayuzuSayisi = agAnahtari.CiktiAgArayuzuSayisi;
                                agAnahtariItem.GirdiAgArayuzuSayisi = agAnahtari.GirdiAgArayuzuSayisi;
                                agAnahtariItem.GucArayuzuSayisi = agAnahtari.GucArayuzuSayisi;
                                agAnahtariItem.Katalog = agAnahtari.Katalog;
                                agAnahtariItem.KatalogDosyaAdi = agAnahtari.KatalogDosyaAdi;
                                agAnahtariItem.Sembol = agAnahtari.Sembol;
                                agAnahtariItem.SembolDosyaAdi = agAnahtari.SembolDosyaAdi;
                                agAnahtariItem.StokNo = agAnahtari.StokNo;
                                agAnahtariItem.Tanim = agAnahtari.Tanim;
                                agAnahtariItem.TipId = agAnahtari.TipId;
                                agAnahtariItem.AgAnahtariTurId = agAnahtariTurItem.Id;
                                agAnahtariItem.UreticiAdi = agAnahtari.UreticiAdi;
                                agAnahtariItem.UreticiParcaNo = agAnahtari.UreticiParcaNo;
                                context.SaveChanges();

                                var aaIdList = aaAgArayuzuList.Where(aaaa => aaaa.AgAnahtariId == agAnahtari.Id).Select(s => s.AgArayuzuId).ToList();
                                var aaList = agArayuzuList.Where(aa => aaIdList.Contains(aa.Id)).ToList();
                                foreach (var agArayuzu in aaList)
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
                                    dbItem1.AgAnahtariId = agAnahtariItem.Id;
                                    context.SaveChanges();
                                }

                                var gaIdList = aaGucArayuzuList.Where(aaga => aaga.AgAnahtariId == agAnahtari.Id).Select(s => s.GucArayuzuId).ToList();
                                var gaList = gucArayuzuList.Where(ga => gaIdList.Contains(ga.Id)).ToList();
                                foreach (var gucArayuzu in gaList)
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
                                    dbItem1.AgAnahtariId = agAnahtariItem.Id;
                                    context.SaveChanges();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        context.Reset();
                        transaction.Rollback();

                        if (exception.InnerException != null)
                        {
                            log.Error("Ağ anahtarı kütüphanesi veritabanına aktarılamadı. - " + exception.InnerException?.Message);
                        }
                        else
                        {
                            log.Error("Ağ anahtarı kütüphanesi veritabanına aktarılamadı. - " + exception.Message);
                        }
                    }
                }
            }
        }
    }
}
