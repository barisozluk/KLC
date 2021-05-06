using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Interfaces;
using AYP.Models;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AYP.Services
{
    public class KodListeService : IKodListeService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly AYPContext context;
        public KodListeService(AYPContext context)
        {
            this.context = context;
        }

        public List<KL_FizikselOrtam> ListFizikselOrtam()
        {
            List<KL_FizikselOrtam> response = new List<KL_FizikselOrtam>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.KL_FizikselOrtam.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {
                    log.Error("Fiziksel ortam listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

        public List<KL_GerilimTipi> ListGerilimTipi()
        {
            List<KL_GerilimTipi> response = new List<KL_GerilimTipi>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.KL_GerilimTipi.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {
                    log.Error("Gerilim tipi listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

        public List<KL_Kapasite> ListKapasite()
        {
            List<KL_Kapasite> response = new List<KL_Kapasite>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.KL_Kapasite.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {
                    log.Error("Kapasite listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

        public KL_Kapasite GetKapasiteById(int id)
        {
            KL_Kapasite response = new KL_Kapasite();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.KL_Kapasite.Where(k => k.Id == id).FirstOrDefault();
                }
                catch (Exception exception)
                {
                    log.Error("Kapasite veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

        public List<KL_KullanimAmaci> ListKullanimAmaci()
        {
            List<KL_KullanimAmaci> response = new List<KL_KullanimAmaci>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = context.KL_KullanimAmaci.OrderBy(o => o.Ad).ToList();
                }
                catch (Exception exception)
                {
                    log.Error("Kullanım amacı listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

        public List<KodListModel> ListAgAkisTipi()
        {
            List<KodListModel> response = new List<KodListModel>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = Enum.GetValues(typeof(AgAkisTipEnum))
                            .Cast<AgAkisTipEnum>()
                            .Select(s => new KodListModel
                            {
                                Id = Convert.ToInt32(s),
                                Ad = Convert.ToInt32(s) == 1 ? "Unicast" : (Convert.ToInt32(s) == 2 ? "Multicast" : "Broadcast")
                            })
                            .ToList();

                }
                catch (Exception exception)
                {
                    log.Error("Ağ akış tipi listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }
        public List<KodListModel> ListAgAkisProtokolu()
        {
            List<KodListModel> response = new List<KodListModel>();

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    response = Enum.GetValues(typeof(AgAkisProtokuluEnum))
                            .Cast<AgAkisProtokuluEnum>()
                            .Select(s => new KodListModel
                            {
                                Id = Convert.ToInt32(s),
                                Ad = Convert.ToInt32(s) == 1 ? "TCP" : "UDP"
                            })
                            .ToList();

                }
                catch (Exception exception)
                {
                    log.Error("Ağ akış protokolu listesi veritabanından getirilemedi. - " + exception.InnerException?.Message);
                }
            }

            return response;
        }

    }
}
