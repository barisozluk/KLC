using AYP.Entities;
using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AYP.Interfaces
{
    public interface IGucUreticiService
    {
        ResponseModel SaveGucUreticiTur(GucUreticiTur gucUreticiTur);
        ResponseModel SaveGucUretici(GucUretici gucUretici);
        ResponseModel UpdateGucUretici(GucUretici gucUretici);
        List<GucUreticiTur> ListGucUreticiTur();
        List<GucUretici> ListGucUretici();
        GucUretici GetGucUreticiById(int gucUreticiId);
        ResponseModel SaveGucUreticiGucArayuzu(GucArayuzu gucArayuzu);
        ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi);

    }
}
