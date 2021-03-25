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
        [Reactive] public int InputSayisi { get; set; }
        [Reactive] public int OutputSayisi { get; set; }
        [Reactive] public int GucArayuzuSayisi { get; set; }

        [Reactive] public List<ConnectorViewModel> InputList { get; set; }
        [Reactive] public List<ConnectorViewModel> GucInputList { get; set; }

        public SourceList<ConnectorViewModel> Transitions { get; set; } = new SourceList<ConnectorViewModel>();
        public ObservableCollectionExtended<ConnectorViewModel> TransitionsForView = new ObservableCollectionExtended<ConnectorViewModel>();
        public int Zindex { get; private set; }

        public NodeViewModel()
        {
            SetupCommands();
            SetupBinding();
        }


        public NodeViewModel(NodesCanvasViewModel nodesCanvas, string name, Guid uniqueId = default(Guid), Point point = default(Point), int id = default(int), int typeId = default(int), int inputSayisi = default(int), int outputSayisi = default(int), int gucArayuzuSayisi = 0)
        {
            NodesCanvas = nodesCanvas;
            Name = name;
            Zindex = nodesCanvas.Nodes.Count;
            Point1 = point;
            Id = id;
            TypeId = typeId;
            UniqueId = uniqueId;
            InputList = new List<ConnectorViewModel>();
            GucInputList = new List<ConnectorViewModel>();

            InputSayisi = inputSayisi;
            OutputSayisi = outputSayisi;
            GucArayuzuSayisi = gucArayuzuSayisi;

            Transitions.Connect().ObserveOnDispatcher().Bind(TransitionsForView).Subscribe();
            SetupConnectors();

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
        private void SetupConnectors()
        {
            for (int i = 0; i < InputSayisi; i++)
            {
                if (i == 0)
                {
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 23))));
                }
                else if (i == 1)
                {
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 34 + (i * 20))));
                }
                else if (i == 2)
                {
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 31 + (i * 19))));
                }
                else if (i == 3)
                {
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 33 + (i * 18))));
                }
                else if (i == 4)
                {
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 36 + (i * 17))));
                }
            }

            for (int i = 0; i < GucArayuzuSayisi; i++)
            {
                GucInputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 160 + (i * 23))));
            }

            Output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54))
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
            return element;
        }
        public XElement ToVisualizationXElement()
        {
            XElement element = ToXElement();
            element.Add(new XAttribute("Position", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("IsCollapse", IsCollapse.ToString()));
            return element;
        }
        public static NodeViewModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            NodeViewModel viewModelNode = null;
            string name = node.Attribute("Name")?.Value;

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

            viewModelNode = new NodeViewModel(nodesCanvas, name);

            return viewModelNode;
        }
    }
}
