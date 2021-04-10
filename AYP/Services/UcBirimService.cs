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
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AYP.Services
{
    public class UcBirimService: IUcBirimService
    {
        private readonly AYPContext context;
        public UcBirimService(AYPContext context)
        {
            this.context = context;
        }

        public ResponseModel SaveUcBirimTur(UcBirimTur ucBirimTur)
        {
            ResponseModel response = new ResponseModel();

            using(var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    UcBirimTur dbItem = new UcBirimTur();
                    context.UcBirimTur.Add(dbItem);
                    dbItem.Ad = ucBirimTur.Ad;

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

        public ResponseModel SaveUcBirim(UcBirim ucBirim, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    UcBirim dbItem = new UcBirim();
                    context.UcBirim.Add(dbItem);
                    dbItem.CiktiAgArayuzuSayisi = ucBirim.CiktiAgArayuzuSayisi;
                    dbItem.GirdiAgArayuzuSayisi = ucBirim.GirdiAgArayuzuSayisi;
                    dbItem.GucArayuzuSayisi = ucBirim.GucArayuzuSayisi;
                    dbItem.Katalog = ucBirim.Katalog;
                    dbItem.KatalogDosyaAdi = ucBirim.KatalogDosyaAdi;
                    dbItem.Sembol = ucBirim.Sembol;
                    dbItem.SembolDosyaAdi = ucBirim.SembolDosyaAdi;
                    dbItem.StokNo = ucBirim.StokNo;
                    dbItem.Tanim = ucBirim.Tanim;
                    dbItem.TipId = ucBirim.TipId;
                    dbItem.UcBirimTurId = ucBirim.UcBirimTurId;
                    dbItem.UreticiAdi = ucBirim.UreticiAdi;
                    dbItem.UreticiParcaNo = ucBirim.UreticiParcaNo;
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

                        UcBirimAgArayuzu dbItem1 = new UcBirimAgArayuzu();
                        context.UcBirimAgArayuzu.Add(dbItem1);
                        dbItem1.AgArayuzuId = dbItemAgArayuzu.Id;
                        dbItem1.UcBirimId = dbItem.Id;
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

                        UcBirimGucArayuzu dbItem1 = new UcBirimGucArayuzu();
                        context.UcBirimGucArayuzu.Add(dbItem1);
                        dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                        dbItem1.UcBirimId = dbItem.Id;
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

        public ResponseModel UpdateUcBirim(UcBirim ucBirim, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var dbItem = context.UcBirim.Where(ub => ub.Id == ucBirim.Id).FirstOrDefault();

                    dbItem.CiktiAgArayuzuSayisi = ucBirim.CiktiAgArayuzuSayisi;
                    dbItem.GirdiAgArayuzuSayisi = ucBirim.GirdiAgArayuzuSayisi;
                    dbItem.GucArayuzuSayisi = ucBirim.GucArayuzuSayisi;
                    dbItem.Katalog = ucBirim.Katalog;
                    dbItem.KatalogDosyaAdi = ucBirim.KatalogDosyaAdi;
                    dbItem.SembolDosyaAdi = ucBirim.SembolDosyaAdi;
                    dbItem.Sembol = ucBirim.Sembol;
                    dbItem.StokNo = ucBirim.StokNo;
                    dbItem.Tanim = ucBirim.Tanim;
                    dbItem.UcBirimTurId = ucBirim.UcBirimTurId;
                    dbItem.UreticiAdi = ucBirim.UreticiAdi;
                    dbItem.UreticiParcaNo = ucBirim.UreticiParcaNo;
                    context.SaveChanges();

                    if(agArayuzuList.Count > 0)
                    {
                        var list = context.UcBirimAgArayuzu.Where(aa => aa.UcBirimId == ucBirim.Id).ToList();

                        if(list.Count > 0)
                        {
                            context.UcBirimAgArayuzu.RemoveRange(list);
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

                            UcBirimAgArayuzu dbItem1 = new UcBirimAgArayuzu();
                            context.UcBirimAgArayuzu.Add(dbItem1);
                            dbItem1.AgArayuzuId = dbItemAgArayuzu.Id;
                            dbItem1.UcBirimId = ucBirim.Id;
                            context.SaveChanges();
                        }
                    }

                    if (gucArayuzuList.Count > 0)
                    {
                        var list = context.UcBirimGucArayuzu.Where(ga => ga.UcBirimId == ucBirim.Id).ToList();

                        if (list.Count > 0)
                        {
                            context.UcBirimGucArayuzu.RemoveRange(list);
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

                            UcBirimGucArayuzu dbItem1 = new UcBirimGucArayuzu();
                            context.UcBirimGucArayuzu.Add(dbItem1);
                            dbItem1.GucArayuzuId = dbItemGucArayuzu.Id;
                            dbItem1.UcBirimId = ucBirim.Id;
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
                    transaction.Rollback();
                }
            }

            return response;
        }

        public List<UcBirimTur> ListUcBirimTur()
        {
            List<UcBirimTur> response = new List<UcBirimTur>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.UcBirimTur.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {
                    
                }
            }

            return response;
        }

        public List<UcBirim> ListUcBirim()
        {
            List<UcBirim> response = new List<UcBirim>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.UcBirim.ToList();

                    foreach(var item in response)
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

        public List<AgArayuzu> ListUcBirimAgArayuzu(int ucBirimId)
        {
            List<AgArayuzu> response = new List<AgArayuzu>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.UcBirimAgArayuzu
                                            .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_FizikselOrtam)
                                            .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_KullanimAmaci)
                                            .Include(x => x.AgArayuzu).ThenInclude(x => x.KL_Kapasite)
                                            .Where(aa => aa.UcBirimId == ucBirimId)
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

                }
            }

            return response;
        }

        public List<GucArayuzu> ListUcBirimGucArayuzu(int ucBirimId)
        {
            List<GucArayuzu> response = new List<GucArayuzu>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.UcBirimGucArayuzu
                                            .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_KullanimAmaci)
                                            .Include(x => x.GucArayuzu).ThenInclude(x => x.KL_GerilimTipi)
                                            .Where(ga => ga.UcBirimId == ucBirimId)
                                            .Select(s => s.GucArayuzu)
                                            .ToList();

                    var gerilimTipiList = context.KL_GerilimTipi.ToList();
                    var kullanimAmaciList = context.KL_KullanimAmaci.ToList();

                    foreach(var item in response)
                    {
                        item.KullanimAmaciList = kullanimAmaciList;
                        item.GerilimTipiList = gerilimTipiList;
                    }
                }
                catch (Exception exception)
                {

                }
            }

            return response;
        }

        public UcBirim GetUcBirimById(int ucBirimId)
        {
            UcBirim response = new UcBirim();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.UcBirim.Include(x => x.UcBirimTur).Where(ub => ub.Id == ucBirimId).FirstOrDefault();
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
                        var ucBirim = context.UcBirim.Where(x => x.Id == selectedId).FirstOrDefault();
                        ucBirim.UreticiAdi = ureticiAdi;
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
