using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

using DynamicData.Binding;
using System.Linq;
using System.Xml.Linq;
using AYP.Helpers.Extensions;
using DynamicData;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AYP.Entities;
using AYP.Enums;

namespace AYP.ViewModel
{
    public partial class NodeViewModel : ReactiveValidationObject<NodeViewModel>
    {
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public bool NameEnable { get; set; } = true;
        [Reactive] public bool Selected { get; set; }
        [Reactive] public Brush BorderBrush { get; set; } = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;
        [Reactive] public bool? TransitionsVisible { get; set; } = true;
        [Reactive] public bool? RollUpVisible { get; set; } = true;
        [Reactive] public bool CanBeDelete { get; set; } = true;
        [Reactive] public bool IsCollapse { get; set; }
        [Reactive] public bool IsVisible { get; set; } = true;

        [Reactive] public ConnectorViewModel Input { get; set; }
        [Reactive] public ConnectorViewModel Output { get; set; }
        [Reactive] public ConnectorViewModel CurrentConnector { get; set; }
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public int IndexStartSelectConnectors { get; set; } = 0;

        [Reactive] public double HeaderWidth { get; set; } = 80;
        [Reactive] public int Id { get; set; }
        [Reactive] public int TypeId { get; set; }
        [Reactive] public Guid UniqueId { get; set; }
        //[Reactive] public int InputSayisi { get; set; }
        //[Reactive] public int OutputSayisi { get; set; }
        //[Reactive] public int GucArayuzuSayisi { get; set; }

        [Reactive] public List<ConnectorViewModel> InputList { get; set; }
        [Reactive] public List<ConnectorViewModel> GucInputList { get; set; }

        [Reactive] public List<AgArayuzu> AgArayuzuList { get; set; }
        [Reactive] public List<GucArayuzu> GucArayuzuList { get; set; }

        public SourceList<ConnectorViewModel> Transitions { get; set; } = new SourceList<ConnectorViewModel>();
        public ObservableCollectionExtended<ConnectorViewModel> TransitionsForView = new ObservableCollectionExtended<ConnectorViewModel>();
        public int Zindex { get; private set; }

        public NodeViewModel()
        {
            SetupCommands();
            SetupBinding();
        }


        public NodeViewModel(NodesCanvasViewModel nodesCanvas, string name, Guid uniqueId = default(Guid), Point point = default(Point), int id = default(int), int typeId = default(int),
                               List<AgArayuzu> agArayuzuList = default, List<GucArayuzu> gucArayuzuList = default, List<ConnectorViewModel> inputList = default, List<ConnectorViewModel> gucInputList = default)
        {
            NodesCanvas = nodesCanvas;
            Name = name;
            Zindex = nodesCanvas.Nodes.Count;
            Point1 = point;
            Id = id;
            TypeId = typeId;
            UniqueId = uniqueId;

            if(inputList != null && inputList.Count() > 0)
            {
                InputList = inputList;
            }
            else
            {
                InputList = new List<ConnectorViewModel>();
            }

            if(gucInputList != null && gucInputList.Count() > 0)
            {
                GucInputList = gucInputList;
            }
            else
            {
                GucInputList = new List<ConnectorViewModel>();
            }

            AgArayuzuList = agArayuzuList;
            GucArayuzuList = gucArayuzuList;
            //InputSayisi = inputSayisi;
            //OutputSayisi = outputSayisi;
            //GucArayuzuSayisi = gucArayuzuSayisi;

            Transitions.Connect().ObserveOnDispatcher().Bind(TransitionsForView).Subscribe();

            if (InputList.Count() == 0)
            {
                SetupInputConnectors();
            }

            if(GucInputList.Count() == 0)
            {
                if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari)
                {
                    SetupGucInputConnectors();
                }
            }

            SetupOutputConnectors();
            SetupCommands();
            SetupBinding();
            SetupSubscriptions();
        }

        #region SetupBinding
        private void SetupBinding()
        {
        }
        #endregion SetupBinding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.BorderBrush = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Brushes.LightGray; });
            this.WhenAnyValue(x => x.TransitionsForView.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));
            this.WhenAnyValue(x => x.Point1, x => x.Size).Subscribe(_ => UpdatePoint2());
            this.WhenAnyValue(x => x.IsCollapse).Subscribe(value => Collapse(value));

            //this.WhenAnyValue(x => x.Transitions.Count).Subscribe(value => UpdateCount(value));
        }
        private void UpdateCount(int value)
        {
            NodesCanvas.TransitionsCount++;
        }
        #endregion Setup Subscriptions
        #region Connectors

        private void SetupInputConnectors()
        {
            if(TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari)
            {
                for (int i = 0; i < AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count(); i++)
                {
                    if (i == 0)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 23)), Guid.NewGuid()));
                    }
                    else if (i == 1)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 34 + (i * 20)), Guid.NewGuid()));
                    }
                    else if (i == 2)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 31 + (i * 19)), Guid.NewGuid()));
                    }
                    else if (i == 3)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 33 + (i * 18)), Guid.NewGuid()));
                    }
                    else if (i == 4)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 17)), Guid.NewGuid()));
                    }
                }
            }
            else
            {
                for (int i = 0; i < GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count(); i++)
                {
                    if (i == 0)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 23)), Guid.NewGuid()));
                    }
                    else if (i == 1)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 34 + (i * 20)), Guid.NewGuid()));
                    }
                    else if (i == 2)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 31 + (i * 19)), Guid.NewGuid()));
                    }
                    else if (i == 3)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 33 + (i * 18)), Guid.NewGuid()));
                    }
                    else if (i == 4)
                    {
                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 17)), Guid.NewGuid()));
                    }
                }
            }
            
        }

        private void SetupGucInputConnectors()
        {
            for (int i = 0; i < GucArayuzuList.Count(); i++)
            {
                GucInputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 160 + (i * 23)), Guid.NewGuid()));
            }
        }

        private void SetupOutputConnectors()
        {
            Output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54), Guid.NewGuid())
            {
                Visible = null
            };
            AddEmptyConnector();
        }
        #endregion Connectors
        private void UpdatePoint2()
        {
            Point2 = Point1.Addition(Size);
        }
        private void UpdateCount(int oldValue, int newValue)
        {
            if ((oldValue > 0) && (newValue > oldValue))
            {
                NodesCanvas.TransitionsCount++;
            }
        }
        private void Collapse(bool value)
        {
            if (!value)
            {

                TransitionsVisible = true;
                Output.Visible = null;
            }
            else
            {
                TransitionsVisible = null;
                Output.Visible = true;
                UnSelectedAllConnectors();
            }
            NotSaved();
        }


        public XElement ToXElement()
        {
            XElement element = new XElement("State");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("TypeId", TypeId));
            element.Add(new XAttribute("UniqueId", UniqueId));
            //element.Add(new XAttribute("InputSayisi", InputSayisi));
            //element.Add(new XAttribute("OutputSayisi", OutputSayisi));
            //element.Add(new XAttribute("GucArayuzuSayisi", GucArayuzuSayisi));
            element.Add(new XAttribute("Position", PointExtensition.PointToString(Point1)));

            return element;
        }
        public XElement ToVisualizationXElement()
        {
            XElement element = ToXElement();
            element.Add(new XAttribute("IsCollapse", IsCollapse.ToString()));
            return element;
        }
        public static NodeViewModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck, XElement stateMachineXElement)
        {
            errorMessage = null;
            NodeViewModel viewModelNode = null;
            string name = node.Attribute("Name")?.Value;
            int id = Convert.ToInt32(node.Attribute("Id")?.Value);
            int typeId = Convert.ToInt32(node.Attribute("TypeId")?.Value);
            Guid uniqueId = new Guid(node.Attribute("UniqueId")?.Value);
            int inputSayisi = Convert.ToInt32(node.Attribute("InputSayisi")?.Value);
            int outputSayisi = Convert.ToInt32(node.Attribute("OutputSayisi")?.Value);
            int gucArayuzuSayisi = Convert.ToInt32(node.Attribute("GucArayuzuSayisi")?.Value);

            Point position = new Point();
            PointExtensition.TryParseFromString(node.Attribute("Position")?.Value, out position); 
               
            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "Node without name";
                return viewModelNode;
            }

            if (actionForCheck(name))
            {
                errorMessage = String.Format("Contains more than one node with name \"{0}\"", name);
                return viewModelNode;
            }

            //viewModelNode = new NodeViewModel(nodesCanvas, name, uniqueId, position, id, typeId, inputSayisi, outputSayisi, gucArayuzuSayisi);

            var inputList = new List<ConnectorViewModel>();
            var gucInputList = new List<ConnectorViewModel>();

            var Inputs = stateMachineXElement.Element("Inputs")?.Elements()?.ToList() ?? new List<XElement>();
            Inputs?.Reverse();
            foreach (var input in Inputs)
            {
                if (new Guid(input.Attribute("NodeUniqueId")?.Value) == uniqueId)
                {
                    string nameInp = input.Attribute("Name")?.Value;
                    Guid uniqueIdInp = new Guid(node.Attribute("UniqueId")?.Value);
                    Point positionInp = new Point();
                    PointExtensition.TryParseFromString(node.Attribute("Position")?.Value, out positionInp);

                    var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameInp, positionInp, uniqueIdInp);
                    inputList.Add(newConnector);
                }
            }

            var GucInputs = stateMachineXElement.Element("GucInputs")?.Elements()?.ToList() ?? new List<XElement>();
            GucInputs?.Reverse();
            foreach (var gucInput in GucInputs)
            {
                if (new Guid(gucInput.Attribute("NodeUniqueId")?.Value) == uniqueId)
                {
                    string nameInp = gucInput.Attribute("Name")?.Value;
                    Guid uniqueIdInp = new Guid(node.Attribute("UniqueId")?.Value);
                    Point positionInp = new Point();
                    PointExtensition.TryParseFromString(node.Attribute("Position")?.Value, out positionInp);

                    var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameInp, positionInp, uniqueIdInp);
                    gucInputList.Add(newConnector);
                }
            }

            viewModelNode.InputList = inputList;
            viewModelNode.GucInputList = gucInputList;

            return viewModelNode;
        }
    }
}
