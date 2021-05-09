using AYP.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AYP.Models
{
    public class DogrulamaModel : ReactiveObject
    {
        public string Mesaj { get; set; }

        public Guid ConnectorId { get; set; }
        public int MesajTipi { get; set; }

        public XElement ToXElement()
        {
            XElement element = new XElement("Dogrulama");
            element.Add(new XAttribute("Mesaj", Mesaj));
            element.Add(new XAttribute("ConnectorId", ConnectorId));
            element.Add(new XAttribute("MesajTipi", MesajTipi));

            return element;
        }

        public static DogrulamaModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            DogrulamaModel viewModelDogrulama = null;

            string mesaj = node.Attribute("Mesaj")?.Value;
            Guid connectorId = new Guid(node.Attribute("ConnectorId")?.Value);
            int mesajTipi = Convert.ToInt32(node.Attribute("MesajTipi")?.Value);


            foreach (var item in nodesCanvas.Nodes.Items)
            {
                viewModelDogrulama = new DogrulamaModel();
                viewModelDogrulama.Mesaj = mesaj;
                viewModelDogrulama.ConnectorId = connectorId;
                viewModelDogrulama.MesajTipi = mesajTipi;

                break;
            }

            return viewModelDogrulama;
        }
    }
}
