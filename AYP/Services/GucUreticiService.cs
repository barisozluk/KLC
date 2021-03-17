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
    public class GucUreticiService : IGucUreticiService
    {
        private readonly AYPContext context;
        public GucUreticiService(AYPContext context)
        {
            this.context = context;
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
    }
}
