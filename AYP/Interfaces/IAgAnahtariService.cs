using AYP.Entities;
using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AYP.Interfaces
{
    public interface IAgAnahtariService
    {
        ResponseModel SaveAgAnahtariTur(AgAnahtariTur agAnahtariTur);
        ResponseModel SaveAgAnahtari(AgAnahtari agAnahtari, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList);
        ResponseModel UpdateAgAnahtari(AgAnahtari agAnahtari);
        List<AgAnahtariTur> ListAgAnahtariTur();
        List<AgAnahtari> ListAgAnahtari();
        AgAnahtari GetAgAnahtariById(int agAnahtariId);
        ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi);

    }
}
