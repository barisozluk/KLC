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
    public class GucUreticiService : IGucUreticiService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GucUreticiService()
        {
        }

        public ResponseModel DeleteGucUretici(GucUretici gucUretici)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        GucUretici d = context.GucUretici.Where(x => x.Id == gucUretici.Id).FirstOrDefault();
                        context.Remove(d);
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
                            log.Error("Güç üretici veritabanından silinemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici veritabanından silinemedi. - " + exception.Message);
                        }

                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public GucUretici GetGucUreticiById(int gucUreticiId)
        {
            GucUretici response = new GucUretici();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.GucUretici.Include(x => x.GucUreticiTur).Where(gu => gu.Id == gucUreticiId).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public GucUreticiTur GetGucUreticiTurById(int gucUreticiTurId)
        {
            GucUreticiTur response = new GucUreticiTur();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.GucUreticiTur.Where(gut => gut.Id == gucUreticiTurId).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici türü veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici türü veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<GucUretici> ListGucUretici()
        {
            List<GucUretici> response = new List<GucUretici>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.GucUretici.Include(x => x.GucUreticiTur).ToList();

                        foreach (var item in response)
                        {
                            item.SembolSrc = ByteToImage(item.Sembol);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üreticiler veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üreticiler veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<GucUreticiTur> ListGucUreticiTur()
        {
            List<GucUreticiTur> response = new List<GucUreticiTur>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.GucUreticiTur.OrderBy(o => o.Ad).ToList();
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici türleri veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici türleri veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public List<GucArayuzu> ListGucUreticiGucArayuzu(int gucUreticiId)
        {
            List<GucArayuzu> response = new List<GucArayuzu>();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        response = context.GucUreticiGucArayuzu
                                                .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_KullanimAmaci)
                                                .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_GerilimTipi)
                                                .Where(ga => ga.GucUreticiId == gucUreticiId)
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
                            log.Error("Güç üretici güç arayüzleri veritabanından getirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici güç arayüzleri veritabanından getirilemedi. - " + exception.Message);
                        }
                    }
                }
            }

            return response;
        }

        public ResponseModel SaveGucUretici(GucUretici gucUretici, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
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
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici veritabanına kaydedilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici veritabanına kaydedilemedi. - " + exception.Message);
                        }

                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public ResponseModel UpdateGucUretici(GucUretici gucUretici, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
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

                        if (gucArayuzuList.Count > 0)
                        {
                            var list = context.GucUreticiGucArayuzu
                                .Include(x => x.GucArayuzu)
                                .Where(ga => ga.GucUreticiId == gucUretici.Id).ToList();

                            if (list.Count > 0)
                            {
                                context.GucUreticiGucArayuzu.RemoveRange(list);
                                context.GucArayuzu.RemoveRange(list.Select(s => s.GucArayuzu).ToList());
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

                                GucUreticiGucArayuzu dbItem1 = new GucUreticiGucArayuzu();
                                context.GucUreticiGucArayuzu.Add(dbItem1);
                                dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                                dbItem1.GucUreticiId = gucUretici.Id;
                                context.SaveChanges();
                            }
                        }


                        transaction.Commit();
                        response.SetSuccess();
                    }
                    catch (Exception exception)
                    {
                        context.Reset();
                        response.SetError(exception.Message);
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici veritabanında güncellenemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici veritabanında güncellenemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public ResponseModel SaveGucUreticiTur(GucUreticiTur gucUreticiTur)
        {
            ResponseModel response = new ResponseModel();

            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
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
                    catch (Exception exception)
                    {
                        context.Reset();
                        response.SetError(exception.Message);
                        if (exception.InnerException != null)
                        {
                            log.Error("Güç üretici türü veritabanına kaydedilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici türü veritabanına kaydedilemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
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
                            var gucUretici = context.GucUretici.Where(x => x.Id == selectedId).FirstOrDefault();
                            gucUretici.UreticiAdi = ureticiAdi;
                            gucUretici.UreticiParcaNo = ureticiParcaNo;
                            gucUretici.SembolDosyaAdi = dosyaAdi;
                            gucUretici.Sembol = sembol;

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
                            log.Error("Güç üretici toplu güncelleme işlemi gerçekleştirilemedi. - " + exception.InnerException.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici toplu güncelleme işlemi gerçekleştirilemedi. - " + exception.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }

            return response;
        }

        public void ImportGucUreticiLibrary(GucUreticiTur gucUreticiTur, List<GucUretici> gucUreticiList, List<GucUreticiGucArayuzu> guGucArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            using (AYPContext context = new AYPContext())
            {
                using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        GucUreticiTur gucUreticiTurItem = context.GucUreticiTur.Where(gut => gut.Ad.ToLower() == gucUreticiTur.Ad.ToLower()).FirstOrDefault();

                        if (gucUreticiTurItem == null)
                        {
                            gucUreticiTurItem = new GucUreticiTur();
                            context.GucUreticiTur.Add(gucUreticiTurItem);
                            gucUreticiTurItem.Ad = gucUreticiTur.Ad;

                            context.SaveChanges();
                        }

                        foreach (var gucUretici in gucUreticiList)
                        {
                            GucUretici gucUreticiItem = context.GucUretici.Include(x => x.GucUreticiTur).Where(gu => gu.StokNo == gucUretici.StokNo).FirstOrDefault();

                            if (gucUreticiItem == null)
                            {
                                gucUreticiItem = new GucUretici();
                                context.GucUretici.Add(gucUreticiItem);
                                gucUreticiItem.CiktiGucArayuzuSayisi = gucUretici.CiktiGucArayuzuSayisi;
                                gucUreticiItem.GirdiGucArayuzuSayisi = gucUretici.GirdiGucArayuzuSayisi;
                                gucUreticiItem.Katalog = gucUretici.Katalog;
                                gucUreticiItem.KatalogDosyaAdi = gucUretici.KatalogDosyaAdi;
                                gucUreticiItem.Sembol = gucUretici.Sembol;
                                gucUreticiItem.SembolDosyaAdi = gucUretici.SembolDosyaAdi;
                                gucUreticiItem.StokNo = gucUretici.StokNo;
                                gucUreticiItem.Tanim = gucUretici.Tanim;
                                gucUreticiItem.TipId = gucUretici.TipId;
                                gucUreticiItem.GucUreticiTurId = gucUreticiTurItem.Id;
                                gucUreticiItem.UreticiAdi = gucUretici.UreticiAdi;
                                gucUreticiItem.UreticiParcaNo = gucUretici.UreticiParcaNo;
                                context.SaveChanges();

                                var gaIdList = guGucArayuzuList.Where(guga => guga.GucUreticiId == gucUretici.Id).Select(s => s.GucArayuzuId).ToList();
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

                                    GucUreticiGucArayuzu dbItem1 = new GucUreticiGucArayuzu();
                                    context.GucUreticiGucArayuzu.Add(dbItem1);
                                    dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                                    dbItem1.GucUreticiId = gucUreticiItem.Id;
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
                            log.Error("Güç üretici kütüphanesi veritabanına aktarılamadı. - " + exception.InnerException?.Message);
                        }
                        else
                        {
                            log.Error("Güç üretici kütüphanesi veritabanına aktarılamadı. - " + exception.Message);
                        }
                    }
                }
            }
        }
    }
}
