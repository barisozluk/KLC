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
        ResponseModel SaveUcBirim(UcBirim ucBirim, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList);
        ResponseModel DeleteUcBirim(UcBirim ucBirim);

        ResponseModel UpdateUcBirim(UcBirim ucBirim, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList);
        List<UcBirimTur> ListUcBirimTur();
        List<UcBirim> ListUcBirim();
        List<AgArayuzu> ListUcBirimAgArayuzu(int ucBirimId);
        List<GucArayuzu> ListUcBirimGucArayuzu(int ucBirimId);
        UcBirim GetUcBirimById(int ucBirimId);
        ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi, string ureticiParcaNo, string dosyaAdi, byte[] sembol);
        UcBirimTur GetUcBirimTurById(int ucBirimTurId);
        void ImportUcBirimLibrary(UcBirimTur ucBirimTur, List<UcBirim> ucBirimList, List<UcBirimAgArayuzu> ubAgArayuzuList, List<AgArayuzu> agArayuzuList, List<UcBirimGucArayuzu> ubGucArayuzuList, List<GucArayuzu> gucArayuzuList);
    }
}
