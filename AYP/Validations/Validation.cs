using AYP.Enums;
using AYP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AYP.Validations
{
    public class Validation
    {
        public bool ValidateDuringDrawStart(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector)
        {
            bool response = true;

            if (fromConnector.Node.TypeId == (int)TipEnum.UcBirim)
            {
                foreach (var connect in fromConnector.NodesCanvas.Connects)
                {
                    if (connect.FromConnector == fromConnector)
                    {
                        OpenModal("Bir uç birim çıktı ağ arayüzünden sadece bir bağlantı olmalıdır.", NodesCanvas);
                        response = false;
                        break;
                    }
                }
            }
            else if (fromConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
            {
                foreach (var connect in fromConnector.NodesCanvas.Connects)
                {
                    if (connect.FromConnector == fromConnector)
                    {
                        OpenModal("Bir ağ anahtarı çıktı ağ arayüzünden sadece bir bağlantı olmalıdır.", NodesCanvas);
                        response = false;
                        break;
                    }
                }
            }
            else if (fromConnector.Node.TypeId == (int)TipEnum.Group)
            {
                if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || fromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                {
                    foreach (var connect in fromConnector.NodesCanvas.Connects)
                    {
                        if (connect.FromConnector == fromConnector)
                        {
                            OpenModal("Bir grup çıktı ağ arayüzünden sadece bir bağlantı olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }
                }
            }

            return response;
        }
        public bool ValidateDuringDrawEnd(NodesCanvasViewModel NodesCanvas, ConnectorViewModel toConnector)
        {
            var fromConnector = NodesCanvas.DraggedConnect.FromConnector;
            bool response = true;

            if (fromConnector.Node.TypeId == (int)TipEnum.UcBirim)
            {
                if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                {
                    OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        if (fromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                        else if (fromConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                foreach (var connect in toConnector.NodesCanvas.Connects)
                                {
                                    if (connect.ToConnector == toConnector)
                                    {
                                        OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                        response = false;
                                        break;
                                    }
                                }

                                if (response)
                                {
                                    response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                {
                    if (fromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                        if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    if (fromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                        if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
            }
            else if (fromConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
            {
                if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                        else if (fromConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                            {
                                foreach (var connect in toConnector.NodesCanvas.Connects)
                                {
                                    if (connect.ToConnector == toConnector)
                                    {
                                        OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                        response = false;
                                        break;
                                    }
                                }

                                if (response)
                                {
                                    response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                        else if (fromConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                foreach (var connect in toConnector.NodesCanvas.Connects)
                                {
                                    if (connect.ToConnector == toConnector)
                                    {
                                        OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                        response = false;
                                        break;
                                    }
                                }

                                if (response)
                                {
                                    response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                {
                    if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
                else if(toConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
            }
            else if (fromConnector.Node.TypeId == (int)TipEnum.GucUretici)
            {
                if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        if (fromConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                OpenModal("Güç Üretici güç arayüzü ile Uç Birim ağ arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        if (fromConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                OpenModal("Güç Üretici güç arayüzü ile Ağ Anahtarı ağ arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                {
                    foreach (var connect in toConnector.NodesCanvas.Connects)
                    {
                        if (connect.ToConnector == toConnector)
                        {
                            OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                            response = false;
                            break;
                        }
                    }

                    if (response)
                    {
                        response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);

                        if (response)
                        {
                            response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                            if (!response)
                            {
                                OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                response = false;
                            }
                            else
                            {
                                response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (!response)
                                {
                                    OpenModal("Yetersiz Güç", NodesCanvas);
                                    response = false;
                                }
                            }
                        }
                    }
                }
                else if (toConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                        OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                        response = false;
                    }
                    else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                            if (response)
                            {
                                response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (!response)
                                {
                                    OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                    response = false;
                                }
                                else
                                {
                                    response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Yetersiz Güç", NodesCanvas);
                                        response = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                        response = false;
                    }
                    else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                            if (response)
                            {
                                response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (!response)
                                {
                                    OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                    response = false;
                                }
                                else
                                {
                                    response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Yetersiz Güç", NodesCanvas);
                                        response = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                            if (response)
                            {
                                response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (!response)
                                {
                                    OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                    response = false;
                                }
                                else
                                {
                                    response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Yetersiz Güç", NodesCanvas);
                                        response = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (fromConnector.Node.TypeId == (int)TipEnum.Group)
            {
                if (fromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                {
                    if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                    {
                        OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                        response = false;
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                    {
                        OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                        response = false;
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            OpenModal("Uç Birim ile Uç Birim birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
                else if (fromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                {
                    if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                    {
                        OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                        response = false;
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bağlantı birebir olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = FizikselOrtamValidasyon(NodesCanvas, fromConnector, toConnector);

                                if (response)
                                {
                                    response = KapasiteValidasyon(NodesCanvas, fromConnector, toConnector);
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                    }
                }
                else if (fromConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                {
                    if (toConnector.Node.TypeId == (int)TipEnum.UcBirim)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                OpenModal("Güç Üretici güç arayüzü ile Uç Birim ağ arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                OpenModal("Güç Üretici güç arayüzü ile Ağ Anahtarı ağ arayüzü birbirine bağlanamaz.", NodesCanvas);
                                response = false;
                            }
                            else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.GucUretici)
                    {
                        foreach (var connect in toConnector.NodesCanvas.Connects)
                        {
                            if (connect.ToConnector == toConnector)
                            {
                                OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                response = false;
                                break;
                            }
                        }

                        if (response)
                        {
                            response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);

                            if (response)
                            {
                                response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (!response)
                                {
                                    OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                    response = false;
                                }
                                else
                                {
                                    response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Yetersiz Güç", NodesCanvas);
                                        response = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (toConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        if (toConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.UcBirimGucArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            OpenModal("Ağ arayüzü ile güç arayüzü birbirine bağlanamaz.", NodesCanvas);
                            response = false;
                        }
                        else if (toConnector.TypeId == (int)TipEnum.AgAnahtariGucArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (toConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                        {
                            foreach (var connect in toConnector.NodesCanvas.Connects)
                            {
                                if (connect.ToConnector == toConnector)
                                {
                                    OpenModal("Bir güç arayüzü girdisi için bir bağlantı olmalıdır.", NodesCanvas);
                                    response = false;
                                    break;
                                }
                            }

                            if (response)
                            {
                                response = GerilimTipiValidasyon(NodesCanvas, fromConnector, toConnector);
                                if (response)
                                {
                                    response = GerilimValidasyon(NodesCanvas, fromConnector, toConnector);
                                    if (!response)
                                    {
                                        OpenModal("Güç üretici ve güç tüketici arasında gerilim uyumsuzluğu vardır.", NodesCanvas);
                                        response = false;
                                    }
                                    else
                                    {
                                        response = GucValidasyon(NodesCanvas, fromConnector, toConnector);
                                        if (!response)
                                        {
                                            OpenModal("Yetersiz Güç", NodesCanvas);
                                            response = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return response;
        }

        public bool GerilimValidasyon(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (toConnector.GirdiDuraganGerilimDegeri1.Value != Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri))
            {
                if (toConnector.GirdiDuraganGerilimDegeri2.HasValue)
                {
                    if (toConnector.GirdiDuraganGerilimDegeri2.Value != Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri))
                    {
                        if (toConnector.GirdiDuraganGerilimDegeri3.HasValue)
                        {
                            if (toConnector.GirdiDuraganGerilimDegeri3.Value != Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri))
                            {
                                if (toConnector.GirdiMinimumGerilimDegeri.HasValue && toConnector.GirdiMaksimumGerilimDegeri.HasValue)
                                {
                                    if (!(toConnector.GirdiMinimumGerilimDegeri.Value <= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri) &&
                                          toConnector.GirdiMaksimumGerilimDegeri.Value >= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri)))
                                    {
                                        response = false;
                                    }
                                }
                                else
                                {
                                    response = false;
                                }
                            }
                        }
                        else
                        {
                            if (toConnector.GirdiMinimumGerilimDegeri.HasValue && toConnector.GirdiMaksimumGerilimDegeri.HasValue)
                            {
                                if (!(toConnector.GirdiMinimumGerilimDegeri.Value <= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri) &&
                                      toConnector.GirdiMaksimumGerilimDegeri.Value >= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri)))
                                {
                                    response = false;
                                }
                            }
                            else
                            {
                                response = false;
                            }
                        }
                    }
                }
                else
                {
                    if (toConnector.GirdiDuraganGerilimDegeri3.HasValue)
                    {
                        if (toConnector.GirdiDuraganGerilimDegeri3.Value != Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri))
                        {
                            if (toConnector.GirdiMinimumGerilimDegeri.HasValue && toConnector.GirdiMaksimumGerilimDegeri.HasValue)
                            {
                                if (!(toConnector.GirdiMinimumGerilimDegeri.Value <= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri) &&
                                      toConnector.GirdiMaksimumGerilimDegeri.Value >= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri)))
                                {
                                    response = false;
                                }
                            }
                            else
                            {
                                response = false;
                            }
                        }
                    }
                    else
                    {
                        if (toConnector.GirdiMinimumGerilimDegeri.HasValue && toConnector.GirdiMaksimumGerilimDegeri.HasValue)
                        {
                            if (!(toConnector.GirdiMinimumGerilimDegeri.Value <= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri) &&
                                  toConnector.GirdiMaksimumGerilimDegeri.Value >= Convert.ToDecimal(fromConnector.CiktiDuraganGerilimDegeri)))
                            {
                                response = false;
                            }
                        }
                        else
                        {
                            response = false;
                        }
                    }
                }
            }

            return response;
        }


        public bool GucValidasyon(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.GirdiTukettigiGucMiktari != -1)
            {
                if (fromConnector.KalanKapasite - toConnector.GirdiTukettigiGucMiktari.Value < 0)
                {
                    response = false;
                }
                else
                {
                    fromConnector.KalanKapasite = fromConnector.KalanKapasite - toConnector.GirdiTukettigiGucMiktari.Value;
                }
            }

            return response;
        }

        public bool FizikselOrtamValidasyon(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.Bakir)
            {
                if (toConnector.FizikselOrtamId == (int)FizikselOrtamEnum.FiberOptik)
                {
                    OpenModal("Bakır - Fiber Optik bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }
            else if (fromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.FiberOptik)
            {
                if (toConnector.FizikselOrtamId == (int)FizikselOrtamEnum.Bakir)
                {
                    OpenModal("Fiber Optik - Bakır bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }

            return response;
        }

        public bool FizikselOrtamValidasyonForTopoloji(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.Bakir)
            {
                if (toConnector.FizikselOrtamId == (int)FizikselOrtamEnum.FiberOptik)
                {
                    response = false;
                }
            }
            else if (fromConnector.FizikselOrtamId == (int)FizikselOrtamEnum.FiberOptik)
            {
                if (toConnector.FizikselOrtamId == (int)FizikselOrtamEnum.Bakir)
                {
                    response = false;
                }
            }

            return response;
        }

        public bool ToConnectorValidasyonForTopoloji(ConnectorViewModel toConnector)
        {
            bool response = true;

            foreach (var connect in toConnector.NodesCanvas.Connects)
            {
                if (connect.ToConnector == toConnector)
                {
                    response = false;
                    break;
                }
            }

            return response;
        }

        private bool GerilimTipiValidasyon(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
            {
                if (toConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
                {
                    OpenModal("AC - DC bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }
            else if (fromConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
            {
                if (toConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
                {
                    OpenModal("DC - AC bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }

            return response;
        }

        public bool GerilimTipiValidasyonForAutoConnect(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
            {
                if (toConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
                {
                    response = false;
                }
            }
            else if (fromConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
            {
                if (toConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
                {
                    response = false;
                }
            }

            return response;
        }

        public bool KapasiteValidasyon(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    OpenModal("Fast Ethernet - Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    OpenModal("Gigabit Ethernet - Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    OpenModal("Gigabit Ethernet - Fast Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    OpenModal("10-Gigabit Ethernet - Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    OpenModal("10-Gigabit Ethernet - Fast Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                {
                    OpenModal("10-Gigabit Ethernet - Gigabit Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    OpenModal("40-Gigabit Ethernet - Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    OpenModal("40-Gigabit Ethernet - Fast Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                {
                    OpenModal("40-Gigabit Ethernet - Gigabit Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                {
                    OpenModal("40-Gigabit Ethernet - 10-Gigabit Ethernet bağlantısı yapılamaz.", NodesCanvas);
                    response = false;
                }
            }

            return response;
        }

        public bool KapasiteValidasyonForTopoloji(NodesCanvasViewModel NodesCanvas, ConnectorViewModel fromConnector, ConnectorViewModel toConnector)
        {
            var response = true;

            if (fromConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                {
                    response = false;
                }
            }
            else if (fromConnector.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
            {
                if (toConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                {
                    response = false;
                }
                else if (toConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                {
                    response = false;
                }
            }

            return response;
        }

        private void OpenModal(string message, NodesCanvasViewModel NodesCanvas)
        {
            NotifyWarningPopup nfp = new NotifyWarningPopup();
            nfp.msg.Text = message;
            nfp.Owner = NodesCanvas.MainWindow;
            nfp.Show();
        }
    }
}
