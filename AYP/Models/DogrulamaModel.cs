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

        public ConnectorViewModel Connector { get; set; }

        public XElement ToXElement()
        {
            XElement element = new XElement("Dogrulama");
            element.Add(new XAttribute("ConnectorUniqueId", Connector.UniqueId));
            element.Add(new XAttribute("Mesaj", Mesaj));

            return element;
        }

        public static DogrulamaModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            DogrulamaModel viewModelDogrulama = null;

            Guid fromConnectorUniqueId = new Guid(node.Attribute("ConnectorUniqueId")?.Value);
            string mesaj = node.Attribute("Mesaj")?.Value;

            foreach(var item in nodesCanvas.Nodes.Items)
            {
                var connector = item.Transitions.Items.Where(x => x.UniqueId == fromConnectorUniqueId).FirstOrDefault();
                viewModelDogrulama = new DogrulamaModel();
                viewModelDogrulama.Mesaj = mesaj;
                viewModelDogrulama.Connector = connector;

                break;
            }

            return viewModelDogrulama;
        }
    }
}
