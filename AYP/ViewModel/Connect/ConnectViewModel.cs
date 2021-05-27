using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using AYP.Helpers;
using AYP.Helpers.Extensions;
using AYP.Enums;
using System.Xml.Linq;
using System.Collections.Generic;
using AYP.Entities;
using AYP.Interfaces;
using AYP.Services;
using AYP.DbContext.AYP.DbContexts;

namespace AYP.ViewModel
{
    public class ConnectViewModel : ReactiveObject
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }

        [Reactive] public Brush Stroke { get; set; } = Application.Current.Resources["ColorConnect"] as SolidColorBrush;

        [Reactive] public ConnectorViewModel FromConnector { get; set; }

        [Reactive] public ConnectorViewModel ToConnector { get; set; }

        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }

        [Reactive] public DoubleCollection StrokeDashArrayFiber { get; set; } = new DoubleCollection() { 1, 2 };
        [Reactive] public DoubleCollection StrokeDashArrayBakir { get; set; } = new DoubleCollection() { 10, 3 };
        [Reactive] public DoubleCollection StrokeDashArrayGuc { get; set; } = new DoubleCollection() { 20, 6 };

        [Reactive] public double StrokeThickness { get; set; } = 1;
        [Reactive] public bool IsVisible { get; set; } = true;
        [Reactive] public bool Selected { get; set; } = false;
        [Reactive] public decimal Uzunluk { get; set; }
        [Reactive] public decimal AgYuku { get; set; }
        [Reactive] public decimal GucMiktari { get; set; }
        [Reactive] public int KabloKesitOnerisi { get; set; }

        private IDisposable subscriptionOnConnectorPositionChange;
        private IDisposable subscriptionOnOutputPositionChange;

        public ConnectViewModel(NodesCanvasViewModel viewModelNodesCanvas, ConnectorViewModel fromConnector)
        {
            Initial(viewModelNodesCanvas, fromConnector);
            SetupSubscriptions();
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.StartPoint, x => x.EndPoint).Subscribe(_ => UpdateMedium());
            this.WhenAnyValue(x => x.FromConnector.Node.IsCollapse).Subscribe(value => UpdateSubscriptionForPosition(value));
            this.WhenAnyValue(x => x.ToConnector.PositionConnectPoint).Subscribe(value => EndPointUpdate(value));
            this.WhenAnyValue(x => x.FromConnector.Selected).Subscribe(value => Select(value));
            this.WhenAnyValue(x => x.NodesCanvas.Theme).Subscribe(_ => Select(this.FromConnector.Selected));
        }
        private void UpdateSubscriptionForPosition(bool nodeIsCollapse)
        {
            if (!nodeIsCollapse)
            {
                subscriptionOnOutputPositionChange?.Dispose();
                subscriptionOnConnectorPositionChange = this.WhenAnyValue(x => x.FromConnector.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));

            }
            else
            {
                subscriptionOnConnectorPositionChange?.Dispose();
                subscriptionOnOutputPositionChange = this.WhenAnyValue(x => x.FromConnector.Node.Output.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));
            }
        }
        private void Initial(NodesCanvasViewModel viewModelNodesCanvas, ConnectorViewModel fromConnector)
        {
            NodesCanvas = viewModelNodesCanvas;
            FromConnector = fromConnector;
            FromConnector.Connect = this;
            this.EndPoint = fromConnector.PositionConnectPoint;
        }
        private void Select(bool value)
        {

            if (this.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu || this.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
            {
                if (this.FromConnector.KapasiteId == (int)KapasiteEnum.Ethernet)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorEthernet"] as SolidColorBrush;
                }
                else if (this.FromConnector.KapasiteId == (int)KapasiteEnum.FastEthernet)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorFastEthernet"] as SolidColorBrush;
                }
                else if (this.FromConnector.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorGigabitEthernet"] as SolidColorBrush;
                }
                else if (this.FromConnector.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector10GigabitEthernet"] as SolidColorBrush;
                }
                else if (this.FromConnector.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnector40GigabitEthernet"] as SolidColorBrush;
                }
            }
            else
            {
                if (this.FromConnector.GerilimTipiId == (int)GerilimTipiEnum.AC)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorAC"] as SolidColorBrush;
                }
                else if (this.FromConnector.GerilimTipiId == (int)GerilimTipiEnum.DC)
                {
                    this.Stroke = Application.Current.Resources[value ? "ColorSelectedElement" : "ColorConnectorDC"] as SolidColorBrush;
                }
            }
        }
        private void StartPointUpdate(Point point)
        {
            if (FromConnector.Node.InputList.Count() == 0)
            {
                StartPoint = point.Addition(0, 10);
            }
            else if (FromConnector.Node.InputList.Count() - 1 != 0)
            {
                int index = FromConnector.Node.Transitions.Items.ToList().FindIndex(x => x.UniqueId == FromConnector.UniqueId);

                int offset1 = ((FromConnector.Node.InputList.Count() - 1) * 20);
                StartPoint = point.Addition(0, (offset1 - index - 5));
            }
            else
            {
                StartPoint = point.Addition(0, 10);
            }

        }
        private void EndPointUpdate(Point point)
        {
            EndPoint = point;
        }
        private void UpdateMedium()
        {
            Point different = EndPoint.Subtraction(StartPoint);
            Point1 = new Point(StartPoint.X + 3 * different.X / 8, StartPoint.Y + 1 * different.Y / 8);
            Point2 = new Point(StartPoint.X + 5 * different.X / 8, StartPoint.Y + 7 * different.Y / 8);
        }

        #endregion Setup Subscriptions

        public XElement ToXElement()
        {
            XElement element = new XElement("Connect");
            element.Add(new XAttribute("FromConnectorNodeUniqueId", FromConnector.Node.UniqueId));
            element.Add(new XAttribute("FromConnectorUniqueId", FromConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorUniqueId", ToConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorNodeUniqueId", ToConnector.Node.UniqueId));
            element.Add(new XAttribute("KabloUzunlugu", Uzunluk));
            element.Add(new XAttribute("AgYuku", AgYuku));
            element.Add(new XAttribute("GucMiktari", GucMiktari));
            element.Add(new XAttribute("Point1", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("Point2", PointExtensition.PointToString(Point2)));
            element.Add(new XAttribute("StartPoint", PointExtensition.PointToString(StartPoint)));
            element.Add(new XAttribute("EndPoint", PointExtensition.PointToString(EndPoint)));

            return element;
        }

        public XElement ToInternalXElement(Guid GroupId)
        {
            XElement element = new XElement("GroupInternalConnect");
            element.Add(new XAttribute("FromConnectorNodeUniqueId", FromConnector.Node.UniqueId));
            element.Add(new XAttribute("FromConnectorUniqueId", FromConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorUniqueId", ToConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorNodeUniqueId", ToConnector.Node.UniqueId));
            element.Add(new XAttribute("KabloUzunlugu", Uzunluk));
            element.Add(new XAttribute("AgYuku", AgYuku));
            element.Add(new XAttribute("GucMiktari", GucMiktari));
            element.Add(new XAttribute("Point1", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("Point2", PointExtensition.PointToString(Point2)));
            element.Add(new XAttribute("StartPoint", PointExtensition.PointToString(StartPoint)));
            element.Add(new XAttribute("EndPoint", PointExtensition.PointToString(EndPoint)));
            element.Add(new XAttribute("GroupId", GroupId));

            return element;
        }

        public XElement ToExternalXElement(Guid GroupId)
        {
            XElement element = new XElement("GroupExternalConnect");
            element.Add(new XAttribute("FromConnectorNodeUniqueId", FromConnector.Node.UniqueId));
            element.Add(new XAttribute("FromConnectorUniqueId", FromConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorUniqueId", ToConnector.UniqueId));
            element.Add(new XAttribute("ToConnectorNodeUniqueId", ToConnector.Node.UniqueId));
            element.Add(new XAttribute("KabloUzunlugu", Uzunluk));
            element.Add(new XAttribute("AgYuku", AgYuku));
            element.Add(new XAttribute("GucMiktari", GucMiktari));
            element.Add(new XAttribute("Point1", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("Point2", PointExtensition.PointToString(Point2)));
            element.Add(new XAttribute("StartPoint", PointExtensition.PointToString(StartPoint)));
            element.Add(new XAttribute("EndPoint", PointExtensition.PointToString(EndPoint)));
            element.Add(new XAttribute("GroupId", GroupId));

            return element;
        }

        public static ConnectViewModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck, XElement stateMachineXElement)
        {
            errorMessage = null;
            ConnectViewModel viewModelConnect = null;

            Guid fromConnectorUniqueId = new Guid(node.Attribute("FromConnectorUniqueId")?.Value);
            Guid fromConnectorNodeUniqueId = new Guid(node.Attribute("FromConnectorNodeUniqueId")?.Value);
            Guid toConnectorUniqueId = new Guid(node.Attribute("ToConnectorUniqueId")?.Value);
            Guid toConnectorNodeUniqueId = new Guid(node.Attribute("ToConnectorNodeUniqueId")?.Value);
            decimal kabloUzunlugu = Convert.ToDecimal(node.Attribute("KabloUzunlugu")?.Value);
            decimal agYuku = Convert.ToDecimal(node.Attribute("AgYuku")?.Value);
            decimal gucMiktari = Convert.ToDecimal(node.Attribute("GucMiktari")?.Value);
            Point point1 = new Point();
            PointExtensition.TryParseFromString(node.Attribute("Point1")?.Value, out point1);

            Point point2 = new Point();
            PointExtensition.TryParseFromString(node.Attribute("Point2")?.Value, out point2);

            Point startPoint = new Point();
            PointExtensition.TryParseFromString(node.Attribute("StartPoint")?.Value, out startPoint);

            Point endPoint = new Point();
            PointExtensition.TryParseFromString(node.Attribute("EndPoint")?.Value, out endPoint);

            var fromConnectorNodeOutputList = nodesCanvas.Nodes.Items.Where(x => x.UniqueId == fromConnectorNodeUniqueId).Select(s => s.Transitions).FirstOrDefault();
            var fromConnector = fromConnectorNodeOutputList.Items.Where(x => x.UniqueId == fromConnectorUniqueId).FirstOrDefault();

            var toConnectorNodeInputList = nodesCanvas.Nodes.Items.Where(x => x.UniqueId == toConnectorNodeUniqueId).Select(s => s.InputList).FirstOrDefault();
            var toConnector = toConnectorNodeInputList.Where(x => x.UniqueId == toConnectorUniqueId).FirstOrDefault();

            viewModelConnect = new ConnectViewModel(nodesCanvas, fromConnector);
            viewModelConnect.ToConnector = toConnector;
            viewModelConnect.Uzunluk = kabloUzunlugu;
            viewModelConnect.AgYuku = agYuku;
            viewModelConnect.GucMiktari = gucMiktari;
            viewModelConnect.StartPoint = startPoint;
            viewModelConnect.EndPoint = endPoint;
            viewModelConnect.Point1 = point1;
            viewModelConnect.Point2 = point2;
            fromConnector.Connect = viewModelConnect;


            IKodListeService service = new KodListeService();

            var AgAkislari = stateMachineXElement.Element("AgAkislari")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var agAkis in AgAkislari)
            {
                if (new Guid(agAkis.Attribute("AgArayuzuUniqueId")?.Value) == fromConnector.UniqueId)
                {
                    var agAkisItem = new AgAkis();
                    agAkisItem.Id = new Guid(agAkis.Attribute("UniqueId")?.Value);
                    agAkisItem.AgAkisProtokoluId = Convert.ToInt32(agAkis.Attribute("AgAkisProtokoluId")?.Value);
                    agAkisItem.AgAkisTipiId = Convert.ToInt32(agAkis.Attribute("AgAkisTipiId")?.Value);
                    agAkisItem.AgAkisTipiAdi = service.ListAgAkisTipi().Where(x => x.Id == agAkisItem.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();
                    agAkisItem.AgAkisProtokoluAdi = service.ListAgAkisProtokolu().Where(x => x.Id == agAkisItem.AgAkisProtokoluId).Select(s => s.Ad).FirstOrDefault();
                    agAkisItem.AgArayuzuId = new Guid(agAkis.Attribute("AgArayuzuUniqueId")?.Value);
                    if (agAkis.Attribute("IliskiliAgArayuzuUniqueId")?.Value == null)
                    {
                        agAkisItem.IliskiliAgArayuzuId = null;
                    }
                    else
                    {
                        agAkisItem.IliskiliAgArayuzuId = new Guid(agAkis.Attribute("IliskiliAgArayuzuUniqueId")?.Value);
                        agAkisItem.IliskiliAgArayuzuAdi = fromConnector.Node.InputList.Where(x => x.UniqueId == agAkisItem.IliskiliAgArayuzuId).Select(s => s.Label).FirstOrDefault();
                    }
                    agAkisItem.Yuk = Convert.ToDecimal(agAkis.Attribute("Yuk")?.Value);
                    fromConnector.AgAkisList.Add(agAkisItem);
                }
                else if (new Guid(agAkis.Attribute("AgArayuzuUniqueId")?.Value) == toConnector.UniqueId)
                {
                    var agAkisItem = new AgAkis();
                    agAkisItem.Id = new Guid(agAkis.Attribute("UniqueId")?.Value);
                    agAkisItem.AgAkisProtokoluId = Convert.ToInt32(agAkis.Attribute("AgAkisProtokoluId")?.Value);
                    agAkisItem.AgAkisTipiId = Convert.ToInt32(agAkis.Attribute("AgAkisTipiId")?.Value);
                    agAkisItem.AgAkisTipiAdi = service.ListAgAkisTipi().Where(x => x.Id == agAkisItem.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();
                    agAkisItem.AgAkisProtokoluAdi = service.ListAgAkisProtokolu().Where(x => x.Id == agAkisItem.AgAkisProtokoluId).Select(s => s.Ad).FirstOrDefault();
                    agAkisItem.AgArayuzuId = new Guid(agAkis.Attribute("AgArayuzuUniqueId")?.Value);
                    agAkisItem.IliskiliAgArayuzuId = null;

                    agAkisItem.Yuk = Convert.ToDecimal(agAkis.Attribute("Yuk")?.Value);
                    toConnector.AgAkisList.Add(agAkisItem);
                }
            }

            return viewModelConnect;
        }
    }
}
