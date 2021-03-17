using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
