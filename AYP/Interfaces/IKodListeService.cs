using AYP.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Interfaces
{
    public interface IKodListeService
    {
        List<KL_Kapasite> ListKapasite();
        List<KL_GerilimTipi> ListGerilimTipi();
        List<KL_KullanimAmaci> ListKullanimAmaci();
        List<KL_FizikselOrtam> ListFizikselOrtam();
    }
}
