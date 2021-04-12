using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using AYP.Helpers;
using System;
using System.Xml.Linq;
using System.Linq;
using AYP.Helpers.Extensions;
using System.Reactive.Linq;
using DynamicData;

namespace AYP.ViewModel
{
    public partial class ConnectorViewModel : ReactiveObject
    {
        [Reactive] public Point PositionConnectPoint { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public bool TextEnable { get; set; } = false;
        [Reactive] public bool? Visible { get; set; } = true;
        [Reactive] public bool FormEnable { get; set; } = true;
        [Reactive] public Brush FormStroke { get; set; }
        [Reactive] public Brush FormFill { get; set; }
        [Reactive] public Brush Foreground { get; set; }
        [Reactive] public double FormStrokeThickness { get; set; } = 1;
        [Reactive] public NodeViewModel Node { get; set; }
        [Reactive] public ConnectViewModel Connect { get; set; }
        [Reactive] public bool ItsLoop { get; set; } = false;
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public bool Selected { get; set; }
        [Reactive] public Guid UniqueId { get; set; }
        [Reactive] public int? KapasiteId { get; set; }
        [Reactive] public int? FizikselOrtamId { get; set; }
        [Reactive] public int KullanimAmaciId { get; set; }
        [Reactive] public int? GerilimTipiId { get; set; }
        [Reactive] public decimal? GirdiDuraganGerilimDegeri1 { get; set; }
        [Reactive] public decimal? GirdiDuraganGerilimDegeri2 { get; set; }
        [Reactive] public decimal? GirdiDuraganGerilimDegeri3 { get; set; }
        [Reactive] public decimal? GirdiMinimumGerilimDegeri { get; set; }
        [Reactive] public decimal? GirdiMaksimumGerilimDegeri { get; set; }
        [Reactive] public decimal? GirdiTukettigiGucMiktari { get; set; }
        [Reactive] public string CiktiDuraganGerilimDegeri { get; set; }
        [Reactive] public decimal? CiktiUrettigiGucKapasitesi { get; set; }
        [Reactive] public string Label { get; set; }
        [Reactive] public int TypeId { get; set; }

        public ConnectorViewModel(NodesCanvasViewModel nodesCanvas, NodeViewModel viewModelNode, string name, Point myPoint, Guid uniqueId, int? kapasiteId = default(int), int? fizikselOrtamId = default(int),
            int? gerilimTipiId = default(int), int kullanimAmaciId = default(int), decimal? girdiDuraganGerilimDegeri1 = default(decimal), decimal? girdiDuraganGerilimDegeri2 = default(decimal), decimal? girdiDuraganGerilimDegeri3 = default(decimal),
            decimal? girdiMinimumGerilimDegeri = default(decimal), decimal? girdiMaksimumGerilimDegeri = default(decimal), decimal? girdiTukettigiGucMiktari = default(decimal),
            string ciktiDuraganGerilimDegeri = default(string), decimal? ciktiUrettigiGucKapasitesi = default(decimal), string label = default(string), int typeId = default(int))
        {
            Node = viewModelNode;
            NodesCanvas = nodesCanvas;
            Name = name;
            Label = label;
            PositionConnectPoint = myPoint;
            UniqueId = uniqueId;
            KapasiteId = kapasiteId;
            KullanimAmaciId = kullanimAmaciId;
            FizikselOrtamId = fizikselOrtamId;
            GerilimTipiId = gerilimTipiId;
            GirdiDuraganGerilimDegeri1 = girdiDuraganGerilimDegeri1;
            GirdiDuraganGerilimDegeri2 = girdiDuraganGerilimDegeri2;
            GirdiDuraganGerilimDegeri3 = girdiDuraganGerilimDegeri3;
            GirdiMinimumGerilimDegeri = girdiMinimumGerilimDegeri;
            GirdiMaksimumGerilimDegeri = girdiMaksimumGerilimDegeri;
            GirdiTukettigiGucMiktari = girdiTukettigiGucMiktari;
            CiktiDuraganGerilimDegeri = ciktiDuraganGerilimDegeri;
            CiktiUrettigiGucKapasitesi = ciktiUrettigiGucKapasitesi;
            TypeId = typeId;

            SetupCommands();
            SetupSubscriptions();
        }
        #region Setup Subscriptions
        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => Select(value));
            this.WhenAnyValue(x => x.NodesCanvas.Theme).Subscribe(_ => UpdateResources());

            if (this.Name!="Girdi")
            {
                this.WhenAnyValue(x => x.Node.HeaderWidth).Buffer(2, 1).Subscribe(x => UpdatePositionOnWidthChange(x[1] - x[0]));
                if (this.Name != "Çıktı")
                {
                    this.WhenAnyValue(x => x.Node.TransitionsForView.Count).Subscribe(x => UpdatePositionOnTransitionCountChange());                   
                }
                
            }

            this.WhenAnyValue(x => x.Node.Point1).Buffer(2, 1).Subscribe(value => PositionConnectPoint = PositionConnectPoint.Addition(value[1].Subtraction(value[0])));
        }

        private void UpdatePositionOnTransitionCountChange()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                int index = Node.Transitions.Items.IndexOf(this);
                this.PositionConnectPoint = Node.CurrentConnector.PositionConnectPoint.Addition(0, index * 20);
            }
        }
        private void UpdatePositionOnWidthChange(double value)
        {
            this.PositionConnectPoint = this.PositionConnectPoint.Addition(value, 0);
        }
        private void UpdateResources()
        {
           Select(this.Selected);
            if (this.ItsLoop)
            {
                ToLoop();
            }
        }           
            

        #endregion Setup Subscriptions
        public XElement ToXElement()
        {
            XElement element = new XElement("Transition");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("FromNode", Node.Name));
            element.Add(new XAttribute("FromNodeUniqueId", Node.UniqueId));
            var ToConnector = this.Connect?.ToConnector;
            element.Add(new XAttribute("To", ToConnector.Node.Name));
            element.Add(new XAttribute("ToNodeUniqueId", ToConnector.Node.UniqueId));
            element.Add(new XAttribute("ToInputUniqueId", ToConnector.UniqueId));
            element.Add(new XAttribute("ToInputName", ToConnector.Name));
            element.Add(new XAttribute("ToInputPosition", PointExtensition.PointToString(ToConnector.PositionConnectPoint)));

            return element;
        }

        public XElement ToInputXElement()
        {
            XElement element = new XElement("Input");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("UniqueId", UniqueId));
            element.Add(new XAttribute("Position", PointExtensition.PointToString(PositionConnectPoint)));
            element.Add(new XAttribute("NodeName", Node.Name));
            element.Add(new XAttribute("NodeUniqueId", Node.UniqueId));

            return element;
        }

        public static ConnectViewModel FromXElement(NodesCanvasViewModel nodesCanvas,XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            ConnectViewModel viewModelConnect = null;

            errorMessage = null;
            string name = node.Attribute("Name")?.Value;
            Guid fromNodeUniqueId = new Guid(node.Attribute("FromNodeUniqueId")?.Value);
            Guid toNodeUniqueId = new Guid(node.Attribute("ToNodeUniqueId")?.Value);
            string toInputName = node.Attribute("ToInputName")?.Value;
            Guid toInputUniqueId = new Guid(node.Attribute("ToInputUniqueId")?.Value);

            Point toInputPosition;
            PointExtensition.TryParseFromString(node.Attribute("ToInputPosition")?.Value, out toInputPosition);

            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "Connect without name";
                return viewModelConnect;
            }
            //if (string.IsNullOrEmpty(from))
            //{
            //    errorMessage = "Connect without from point";
            //    return viewModelConnect;
            //}
            //if (string.IsNullOrEmpty(to))
            //{
            //    errorMessage = "Connect without to point";
            //    return viewModelConnect;
            //}
            if (actionForCheck(name))
            {
                errorMessage = String.Format("Contains more than one connect with name \"{0}\"", name);
                return viewModelConnect;
            }

            NodeViewModel nodeFrom = nodesCanvas.Nodes.Items.Single(x => x.UniqueId == fromNodeUniqueId);
            NodeViewModel nodeTo = nodesCanvas.Nodes.Items.Single(x => x.UniqueId == toNodeUniqueId);
          
            nodeFrom.CurrentConnector.Name = name;


            if (nodeFrom == nodeTo)
            {
                nodeFrom.CurrentConnector.CommandSetAsLoop.ExecuteWithSubscribe();
            }
            else
            {
                viewModelConnect = new ConnectViewModel(nodeFrom.NodesCanvas, nodeFrom.CurrentConnector);
                viewModelConnect.ToConnector = new ConnectorViewModel(nodesCanvas, nodeTo, toInputName, toInputPosition, toInputUniqueId);
                nodeFrom.CommandAddEmptyConnector.ExecuteWithSubscribe();
            }     

            return viewModelConnect;
        }
    }

}
