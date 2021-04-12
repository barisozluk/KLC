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
        [Reactive] public ConnectorViewModel Output { get; set; }
        [Reactive] public ConnectorViewModel CurrentConnector { get; set; }
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public int IndexStartSelectConnectors { get; set; } = 0;
        [Reactive] public double HeaderWidth { get; set; } = 80;
        [Reactive] public int Id { get; set; }
        [Reactive] public int TypeId { get; set; }
        [Reactive] public Guid UniqueId { get; set; }
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

            Transitions.Connect().ObserveOnDispatcher().Bind(TransitionsForView).Subscribe();

            if (InputList.Count() == 0)
            {
                SetupInputConnectors();
            }

            if(GucInputList.Count() == 0)
            {
                if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
                {
                    SetupGucInputConnectors();
                }
            }

            if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
            {
                if (AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).Count() > 0)
                {
                    SetupOutputConnectors();
                }
            }
            else
            {
                if (GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).Count() > 0)
                {
                    SetupOutputConnectors();
                }
            }

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
            if(TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
            {
                AgArayuzuList = AgArayuzuList.OrderBy(o => o.Port).ToList();
                var inputList = AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).ToList();

                for (int i = 0; i < AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count(); i++)
                {
                    var adi = inputList[i].Adi;
                    var kapasiteId = inputList[i].KapasiteId;
                    var fizikselOrtamId = inputList[i].FizikselOrtamId;
                    var kullanimAmaciId = inputList[i].KullanimAmaciId;
                    var typeId = inputList[i].TipId.Value;

                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 30 + (i * 20)), Guid.NewGuid(), kapasiteId, fizikselOrtamId, null, kullanimAmaciId, null, null, null, null, null, null, null, null, adi, typeId));
                }
            }
            else
            {
                GucArayuzuList = GucArayuzuList.OrderBy(o => o.Port).ToList();
                var inputList = GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).ToList();

                for (int i = 0; i < inputList.Count(); i++)
                {
                    var adi = inputList[i].Adi;
                    var gerilimTipiId = inputList[i].GerilimTipiId;
                    var kullanimAmaciId = inputList[i].KullanimAmaciId;
                    var ciktiDuraganGerilimDegeri = inputList[i].CiktiDuraganGerilimDegeri;
                    var ciktiUrettigiGucKapasitesi = inputList[i].CiktiUrettigiGucKapasitesi;
                    var girdiDuraganGerilimDegeri1 = inputList[i].GirdiDuraganGerilimDegeri1;
                    var girdiDuraganGerilimDegeri2 = inputList[i].GirdiDuraganGerilimDegeri2;
                    var girdiDuraganGerilimDegeri3 = inputList[i].GirdiDuraganGerilimDegeri3;
                    var girdiMaksimumGerilimDegeri = inputList[i].GirdiMaksimumGerilimDegeri;
                    var girdiMinimumGerilimDegeri = inputList[i].GirdiMinimumGerilimDegeri;
                    var girdiTukettigiGucMiktari = inputList[i].GirdiTukettigiGucMiktari;
                    var typeId = inputList[i].TipId.Value;

                    
                    InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 30 + (i * 20)), Guid.NewGuid(), null, null, gerilimTipiId, kullanimAmaciId, 
                            girdiDuraganGerilimDegeri1, girdiDuraganGerilimDegeri2, girdiDuraganGerilimDegeri3, girdiMinimumGerilimDegeri, girdiMaksimumGerilimDegeri, girdiTukettigiGucMiktari,
                            ciktiDuraganGerilimDegeri, ciktiUrettigiGucKapasitesi, adi, typeId));
                    
                }
            }
            
        }

        private void SetupGucInputConnectors()
        {
            GucArayuzuList = GucArayuzuList.OrderBy(o => o.Port).ToList();

            for (int i = 1; i <= GucArayuzuList.Count(); i++)
            {
                var adi = GucArayuzuList[i-1].Adi;
                var gerilimTipiId = GucArayuzuList[i-1].GerilimTipiId;
                var kullanimAmaciId = GucArayuzuList[i-1].KullanimAmaciId;
                var ciktiDuraganGerilimDegeri = GucArayuzuList[i-1].CiktiDuraganGerilimDegeri;
                var ciktiUrettigiGucKapasitesi = GucArayuzuList[i-1].CiktiUrettigiGucKapasitesi;
                var girdiDuraganGerilimDegeri1 = GucArayuzuList[i-1].GirdiDuraganGerilimDegeri1;
                var girdiDuraganGerilimDegeri2 = GucArayuzuList[i-1].GirdiDuraganGerilimDegeri2;
                var girdiDuraganGerilimDegeri3 = GucArayuzuList[i-1].GirdiDuraganGerilimDegeri3;
                var girdiMaksimumGerilimDegeri = GucArayuzuList[i-1].GirdiMaksimumGerilimDegeri;
                var girdiMinimumGerilimDegeri = GucArayuzuList[i-1].GirdiMinimumGerilimDegeri;
                var girdiTukettigiGucMiktari = GucArayuzuList[i-1].GirdiTukettigiGucMiktari;
                var typeId = GucArayuzuList[i-1].TipId.Value;

                GucInputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, (((InputList.Count-1) * 20) + 30) + (i * 20)), Guid.NewGuid(), null, null, gerilimTipiId, kullanimAmaciId,
                            girdiDuraganGerilimDegeri1, girdiDuraganGerilimDegeri2, girdiDuraganGerilimDegeri3, girdiMinimumGerilimDegeri, girdiMaksimumGerilimDegeri, girdiTukettigiGucMiktari,
                            ciktiDuraganGerilimDegeri, ciktiUrettigiGucKapasitesi, adi, typeId));
            }
        }

        private void SetupOutputConnectors()
        {
            if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
            {
                AgArayuzuList = AgArayuzuList.OrderBy(o => o.Port).ToList();
                var outputList = AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti);

                var adi = outputList.First().Adi;
                var kapasiteId = outputList.First().KapasiteId;
                var fizikselOrtamId = outputList.First().FizikselOrtamId;
                var kullanimAmaciId = outputList.First().KullanimAmaciId;
                var typeId = outputList.First().TipId.Value;

                Output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54), Guid.NewGuid(), kapasiteId, fizikselOrtamId, null, kullanimAmaciId, null, null, null, null, null, null, null, null, adi, typeId)
                {
                    Visible = null
                };

            }
            else
            {
                GucArayuzuList = GucArayuzuList.OrderBy(o => o.Port).ToList();
                var outputList = GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti);

                var adi = outputList.First().Adi;
                var gerilimTipiId = outputList.First().GerilimTipiId;
                var kullanimAmaciId = outputList.First().KullanimAmaciId;
                var ciktiDuraganGerilimDegeri = outputList.First().CiktiDuraganGerilimDegeri;
                var ciktiUrettigiGucKapasitesi = outputList.First().CiktiUrettigiGucKapasitesi;
                var girdiDuraganGerilimDegeri1 = outputList.First().GirdiDuraganGerilimDegeri1;
                var girdiDuraganGerilimDegeri2 = outputList.First().GirdiDuraganGerilimDegeri2;
                var girdiDuraganGerilimDegeri3 = outputList.First().GirdiDuraganGerilimDegeri3;
                var girdiMaksimumGerilimDegeri = outputList.First().GirdiMaksimumGerilimDegeri;
                var girdiMinimumGerilimDegeri = outputList.First().GirdiMinimumGerilimDegeri;
                var girdiTukettigiGucMiktari = outputList.First().GirdiTukettigiGucMiktari;
                var typeId = outputList.First().TipId.Value;

                Output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54), Guid.NewGuid(), null, null, gerilimTipiId, kullanimAmaciId,
                            girdiDuraganGerilimDegeri1, girdiDuraganGerilimDegeri2, girdiDuraganGerilimDegeri3, girdiMinimumGerilimDegeri, girdiMaksimumGerilimDegeri, girdiTukettigiGucMiktari,
                            ciktiDuraganGerilimDegeri, ciktiUrettigiGucKapasitesi, adi, typeId)
                {
                    Visible = null
                };
            }
                   
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
