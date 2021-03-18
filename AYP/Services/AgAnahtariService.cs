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

            using(var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
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
                catch(Exception exception)
                {
                    context.Reset();
                    response.SetError(exception.Message);
                    transaction.Rollback();
                }
            }

            return response;
        }

        public ResponseModel SaveAgAnahtari(AgAnahtari agAnahtari)
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
                    dbItem.Sembol = agAnahtari.Sembol;
                    dbItem.StokNo = agAnahtari.StokNo;
                    dbItem.Tanim = agAnahtari.Tanim;
                    dbItem.TipId = agAnahtari.TipId;
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
    }
}
