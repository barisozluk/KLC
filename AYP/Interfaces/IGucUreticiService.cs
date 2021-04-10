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
        ResponseModel SaveGucUretici(GucUretici gucUretici, List<GucArayuzu> gucArayuzuList);
        ResponseModel UpdateGucUretici(GucUretici gucUretici, List<GucArayuzu> gucArayuzuList);
        List<GucUreticiTur> ListGucUreticiTur();
        List<GucUretici> ListGucUretici();
        List<GucArayuzu> ListGucUreticiGucArayuzu(int gucUreticiId);
        GucUretici GetGucUreticiById(int gucUreticiId);
        ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi);

    }
}
