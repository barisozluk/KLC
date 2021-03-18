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

        public ResponseModel SaveUcBirim(UcBirim ucBirim)
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
                    dbItem.Sembol = ucBirim.Sembol;
                    dbItem.StokNo = ucBirim.StokNo;
                    dbItem.Tanim = ucBirim.Tanim;
                    dbItem.TipId = ucBirim.TipId;
                    dbItem.UcBirimTurId = ucBirim.UcBirimTurId;
                    dbItem.UreticiAdi = ucBirim.UreticiAdi;
                    dbItem.UreticiParcaNo = ucBirim.UreticiParcaNo;

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
    }
}
