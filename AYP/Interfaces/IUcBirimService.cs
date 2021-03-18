using AYP.Entities;
using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AYP.Interfaces
{
    public interface IUcBirimService
    {
        ResponseModel SaveUcBirimTur(UcBirimTur ucBirimTur);
        ResponseModel SaveUcBirim(UcBirim ucBirim);
        List<UcBirimTur> ListUcBirimTur();
        List<UcBirim> ListUcBirim();
        UcBirim GetUcBirimById(int ucBirimId);
    }
}
