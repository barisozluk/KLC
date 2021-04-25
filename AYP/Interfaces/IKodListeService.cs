using AYP.Entities;
using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Interfaces
{
    public interface IKodListeService
    {
        List<KL_Kapasite> ListKapasite();
        KL_Kapasite GetKapasiteById(int id);
        List<KL_GerilimTipi> ListGerilimTipi();
        List<KL_KullanimAmaci> ListKullanimAmaci();
        List<KL_FizikselOrtam> ListFizikselOrtam();
        List<KodListModel> ListAgAkisProtokolu();
        List<KodListModel> ListAgAkisTipi();
    }
}
