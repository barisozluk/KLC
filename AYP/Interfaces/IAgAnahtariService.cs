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
        ResponseModel DeleteAgAnahtari(AgAnahtari agAnahtari);

        ResponseModel UpdateAgAnahtari(AgAnahtari agAnahtari, List<AgArayuzu> agArayuzuList, List<GucArayuzu> gucArayuzuList);
        List<AgAnahtariTur> ListAgAnahtariTur();
        List<AgAnahtari> ListAgAnahtari();
        List<AgArayuzu> ListAgAnahtariAgArayuzu(int agAnahtariId);
        List<GucArayuzu> ListAgAnahtariGucArayuzu(int agAnahtariId);
        AgAnahtari GetAgAnahtariById(int agAnahtariId);
        ResponseModel SaveTopluEdit(List<int> selectedIdList, string ureticiAdi);
        AgAnahtariTur GetAgAnahtariTurById(int agAnahtariTurId);
        void ImportAgAnahtariLibrary(AgAnahtariTur agAnahtariTur, List<AgAnahtari> agAnahtariList, List<AgAnahtariAgArayuzu> aaAgArayuzuList, List<AgArayuzu> agArayuzuList, List<AgAnahtariGucArayuzu> aaGucArayuzuList, List<GucArayuzu> gucArayuzuList);
    }
}
