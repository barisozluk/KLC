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
    public class AgAnahatariService : IAgAnahatariService
    {
        private readonly AYPContext context;
        public AgAnahatariService(AYPContext context)
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
    }
}
