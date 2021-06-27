using AYP.Entities;
using AYP.Enums;
using AYP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AYP.Helpers.AgAkisUpdater
{
    public class AgAkisUpdater
    {
        private void UpdateAgAkisFromArayuz(ConnectorViewModel connector)
        {

        }

        public void UpdateAgAkisFromConnect(ConnectViewModel connect, bool isDelete)
        {
            if (isDelete)
            {
                connect.ToConnector.AgAkisList.Clear();
            }
            else
            {
                if (connect.FromConnector.AgAkisList != null && connect.FromConnector.AgAkisList.Count() != 0)
                {
                    connect.ToConnector.AgAkisList = new List<AgAkis>();
                    connect.ToConnector.AgAkisList.Clear();
                    foreach (var agAkis in connect.FromConnector.AgAkisList)
                    {
                        var agAkisTemp = new AgAkis();
                        agAkisTemp.Id = Guid.NewGuid();
                        agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                        agAkisTemp.Yuk = agAkis.Yuk;
                        agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                        agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                        agAkisTemp.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                        agAkisTemp.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;
                        agAkisTemp.VarisNoktasiIdNameList = agAkis.VarisNoktasiIdNameList;
                        agAkisTemp.FromNodeUniqueId = agAkis.FromNodeUniqueId;

                        connect.ToConnector.AgAkisList.Add(agAkisTemp);
                        connect.AgYuku = connect.ToConnector.AgAkisList.Select(x => x.Yuk).Sum();
                    }
                }
            }

            if (connect.ToConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
            {
                foreach (var output in connect.ToConnector.Node.Transitions.Items)
                {
                    var list = output.AgAkisList.Where(x => x.IliskiliAgArayuzuId == connect.ToConnector.UniqueId).ToList();

                    foreach (var item in list)
                    {
                        output.AgAkisList.Remove(item);
                    }

                    if (output.Connect != null)
                    {
                        output.Connect.AgYuku = output.AgAkisList.Select(s => s.Yuk).Sum();
                        UpdateAgAkisFromConnect(output.Connect, false);
                    }
                }
            }
        }

        private void UpdateAgAkisFromNode(NodeViewModel node)
        {

        }
    }
}
