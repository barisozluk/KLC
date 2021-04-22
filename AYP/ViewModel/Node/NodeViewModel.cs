﻿using System;
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
using System.Text;

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
        [Reactive] public decimal? VerimlilikOrani { get; set; }
        [Reactive] public decimal? DahiliGucTuketimDegeri { get; set; }
        [Reactive] public byte[] Sembol { get; set; }
        [Reactive] public int Id { get; set; }
        [Reactive] public string StokNo { get; set; }
        [Reactive] public string Tanim { get; set; }
        [Reactive] public string UreticiAdi { get; set; }
        [Reactive] public string UreticiParcaNo { get; set; }
        [Reactive] public string TurAd { get; set; }
        [Reactive] public int TypeId { get; set; }
        [Reactive] public Guid UniqueId { get; set; }
        [Reactive] public List<ConnectorViewModel> InputList { get; set; }
        [Reactive] public List<ConnectorViewModel> OutputList { get; set; }
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
                               List<AgArayuzu> agArayuzuList = default, List<GucArayuzu> gucArayuzuList = default, List<ConnectorViewModel> inputList = default, List<ConnectorViewModel> outputList = default,
                               decimal? verimlilikOrani = default, decimal? dahiliGucTuketimDegeri = default, byte[] sembol = default, string stokNo = default, string tanim = default, string ureticiAdi = default,
                               string ureticiParcaNo = default, string turAd = default)
        {
            NodesCanvas = nodesCanvas;
            Name = name;
            Zindex = nodesCanvas.Nodes.Count;
            Point1 = point;
            Id = id;
            TypeId = typeId;
            UniqueId = uniqueId;
            VerimlilikOrani = verimlilikOrani;
            DahiliGucTuketimDegeri = dahiliGucTuketimDegeri;
            Sembol = sembol;
            StokNo = stokNo;
            Tanim = tanim;
            UreticiAdi = ureticiAdi;
            UreticiParcaNo = ureticiParcaNo;
            TurAd = turAd;

            AgArayuzuList = agArayuzuList;
            GucArayuzuList = gucArayuzuList;

            if (inputList != null && inputList.Count() > 0)
            {
                InputList = inputList;
            }
            else
            {
                InputList = new List<ConnectorViewModel>();
            }

            if (outputList != null && outputList.Count() > 0)
            {
                OutputList = outputList;
                AddEmptyConnector();
            }
            else
            {
                OutputList = new List<ConnectorViewModel>();
            }

            Transitions.Connect().ObserveOnDispatcher().Bind(TransitionsForView).Subscribe();

            if (InputList.Count() == 0)
            {
                SetupInputConnectors();
            }

            if (OutputList.Count() == 0)
            {
                SetupOutputConnectors();
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
            if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
            {
                if (AgArayuzuList != null)
                {
                    AgArayuzuList = AgArayuzuList.OrderBy(o => o.Port).ToList();
                    var inputList = AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).ToList();

                    for (int i = 0; i < AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count(); i++)
                    {
                        var adi = inputList[i].Adi;
                        var kapasiteId = inputList[i].KapasiteId;
                        var minKapasite = inputList[i].KL_Kapasite.MinKapasite;
                        var maxKapasite = inputList[i].KL_Kapasite.MaxKapasite;
                        var fizikselOrtamId = inputList[i].FizikselOrtamId;
                        var kullanimAmaciId = inputList[i].KullanimAmaciId;
                        var typeId = inputList[i].TipId.Value;

                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 30 + (i * 20)), Guid.NewGuid(), kapasiteId, minKapasite, maxKapasite, fizikselOrtamId, null, kullanimAmaciId, null, null, null, null, null, null, null, null, adi, typeId, null));
                    }
                }
            }

            if (GucArayuzuList != null)
            {
                if (GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count() > 0)
                {
                    int count = InputList.Count();
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


                        InputList.Add(new ConnectorViewModel(NodesCanvas, this, "Girdi", Point1.Addition(0, 30 + ((i + count) * 20)), Guid.NewGuid(), null, null, null, null, gerilimTipiId, kullanimAmaciId,
                                girdiDuraganGerilimDegeri1, girdiDuraganGerilimDegeri2, girdiDuraganGerilimDegeri3, girdiMinimumGerilimDegeri, girdiMaksimumGerilimDegeri, girdiTukettigiGucMiktari,
                                ciktiDuraganGerilimDegeri, ciktiUrettigiGucKapasitesi, adi, typeId, null));

                    }
                }
            }
        }

        private void SetupOutputConnectors()
        {
            if (TypeId == (int)TipEnum.UcBirim || TypeId == (int)TipEnum.AgAnahtari || TypeId == (int)TipEnum.Group)
            {
                if (AgArayuzuList != null)
                {
                    AgArayuzuList = AgArayuzuList.OrderBy(o => o.Port).ToList();
                    var outputList = AgArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).ToList();

                    for (int i = 0; i < outputList.Count(); i++)
                    {
                        var adi = outputList[i].Adi;
                        var kapasiteId = outputList[i].KapasiteId;
                        var minKapasite = outputList[i].KL_Kapasite.MinKapasite;
                        var maxKapasite = outputList[i].KL_Kapasite.MaxKapasite;
                        var fizikselOrtamId = outputList[i].FizikselOrtamId;
                        var kullanimAmaciId = outputList[i].KullanimAmaciId;
                        var typeId = outputList[i].TipId.Value;

                        var output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54 + (i * 20)), Guid.NewGuid(), kapasiteId, minKapasite, maxKapasite, fizikselOrtamId, null, kullanimAmaciId, null, null, null, null, null, null, null, null, adi, typeId)
                        {
                            Visible = null
                        };

                        if (output.TypeId == (int)TipEnum.GucUreticiGucArayuzu && output.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                        {
                            if (VerimlilikOrani == 0)
                            {
                                output.KalanKapasite = (output.CiktiUrettigiGucKapasitesi.Value - DahiliGucTuketimDegeri);
                            }
                            else
                            {
                                output.KalanKapasite = (VerimlilikOrani * output.CiktiUrettigiGucKapasitesi.Value);
                            }

                        }
                        else
                        {
                            output.KalanKapasite = null;
                        }

                        OutputList.Add(output);
                    }
                }
            }

            if (GucArayuzuList != null)
            {
                if (GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).Count() > 0)
                {
                    int count = OutputList.Count();
                    GucArayuzuList = GucArayuzuList.OrderBy(o => o.Port).ToList();
                    var outputList = GucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).ToList();

                    for (int i = 0; i < outputList.Count(); i++)
                    {
                        var adi = outputList[i].Adi;
                        var gerilimTipiId = outputList[i].GerilimTipiId;
                        var kullanimAmaciId = outputList[i].KullanimAmaciId;
                        var ciktiDuraganGerilimDegeri = outputList[i].CiktiDuraganGerilimDegeri;
                        var ciktiUrettigiGucKapasitesi = outputList[i].CiktiUrettigiGucKapasitesi;
                        var girdiDuraganGerilimDegeri1 = outputList[i].GirdiDuraganGerilimDegeri1;
                        var girdiDuraganGerilimDegeri2 = outputList[i].GirdiDuraganGerilimDegeri2;
                        var girdiDuraganGerilimDegeri3 = outputList[i].GirdiDuraganGerilimDegeri3;
                        var girdiMaksimumGerilimDegeri = outputList[i].GirdiMaksimumGerilimDegeri;
                        var girdiMinimumGerilimDegeri = outputList[i].GirdiMinimumGerilimDegeri;
                        var girdiTukettigiGucMiktari = outputList[i].GirdiTukettigiGucMiktari;
                        var typeId = outputList[i].TipId.Value;

                        var output = new ConnectorViewModel(NodesCanvas, this, "Çıktı", Point1.Addition(80, 54 + ((i + count) * 20)), Guid.NewGuid(), null, null, null, null, gerilimTipiId, kullanimAmaciId,
                                    girdiDuraganGerilimDegeri1, girdiDuraganGerilimDegeri2, girdiDuraganGerilimDegeri3, girdiMinimumGerilimDegeri, girdiMaksimumGerilimDegeri, girdiTukettigiGucMiktari,
                                    ciktiDuraganGerilimDegeri, ciktiUrettigiGucKapasitesi, adi, typeId)
                        {
                            Visible = null
                        };

                        if (output.TypeId == (int)TipEnum.GucUreticiGucArayuzu && output.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                        {
                            if (VerimlilikOrani == 0)
                            {
                                output.KalanKapasite = (output.CiktiUrettigiGucKapasitesi.Value - DahiliGucTuketimDegeri);
                            }
                            else
                            {
                                output.KalanKapasite = (VerimlilikOrani * output.CiktiUrettigiGucKapasitesi.Value);
                            }

                        }
                        OutputList.Add(output);
                    }
                }
            }

            if (GucArayuzuList != null || AgArayuzuList != null)
            {
                AddEmptyConnector();
            }
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
                //Output.Visible = null;
            }
            else
            {
                TransitionsVisible = null;
                //Output.Visible = true;
                UnSelectedAllConnectors();
            }
            NotSaved();
        }


        public XElement ToXElement()
        {
            XElement element = new XElement("State");
            element.Add(new XAttribute("UniqueId", UniqueId));
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Tanim", Tanim));
            element.Add(new XAttribute("StokNo", StokNo));
            element.Add(new XAttribute("UreticiAdi", UreticiAdi));
            element.Add(new XAttribute("UreticiParcaNo", UreticiParcaNo));
            element.Add(new XAttribute("Tur", TurAd));
            element.Add(new XAttribute("TypeId", TypeId));
            element.Add(new XAttribute("Position", PointExtensition.PointToString(Point1)));
            element.Add(!VerimlilikOrani.HasValue ? null : new XAttribute("VerimlilikOrani", VerimlilikOrani));
            element.Add(!DahiliGucTuketimDegeri.HasValue ? null : new XAttribute("DahiliGucTuketimDegeri", DahiliGucTuketimDegeri));
            element.Add(new XAttribute("Sembol", Convert.ToBase64String(Sembol)));

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

            Guid uniqueId = new Guid(node.Attribute("UniqueId")?.Value);
            int id = Convert.ToInt32(node.Attribute("Id")?.Value);
            string name = node.Attribute("Name")?.Value;
            string tanim = node.Attribute("Tanim")?.Value;
            string stokNo = node.Attribute("StokNo")?.Value;
            string ureticiAdi = node.Attribute("UreticiAdi")?.Value;
            string ureticiParcaNo = node.Attribute("UreticiParcaNo")?.Value;
            string turAd = node.Attribute("Tur")?.Value;
            int typeId = Convert.ToInt32(node.Attribute("TypeId")?.Value);
            Point position = new Point();
            PointExtensition.TryParseFromString(node.Attribute("Position")?.Value, out position);
            decimal? verimlilikOrani = 0;
            if (node.Attribute("VerimlilikOrani")?.Value == null) { verimlilikOrani = null; }
            else { verimlilikOrani = Convert.ToDecimal(node.Attribute("VerimlilikOrani")?.Value); }
            decimal? dahiliGucTuketimDegeri = 0;
            if (node.Attribute("DahiliGucTuketimDegeri")?.Value == null) { dahiliGucTuketimDegeri = null; }
            else { dahiliGucTuketimDegeri = Convert.ToDecimal(node.Attribute("DahiliGucTuketimDegeri")?.Value); }
            byte[] sembol = Convert.FromBase64String(node.Attribute("Sembol")?.Value);


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

            viewModelNode = new NodeViewModel(nodesCanvas, name, uniqueId, position, id, typeId, null, null, null, null, verimlilikOrani, dahiliGucTuketimDegeri, sembol, stokNo, tanim, ureticiAdi, ureticiParcaNo, turAd);
            var inputList = new List<ConnectorViewModel>();
            var outputList = new List<ConnectorViewModel>();

            var Inputs = stateMachineXElement.Element("Inputs")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var input in Inputs)
            {
                if (new Guid(input.Attribute("NodeUniqueId")?.Value) == uniqueId)
                {
                    string nameInput = input.Attribute("Name")?.Value;
                    string labelInput = input.Attribute("Label")?.Value;
                    Guid uniqueIdInput = new Guid(input.Attribute("UniqueId")?.Value);
                    Point positionInput = new Point();
                    PointExtensition.TryParseFromString(input.Attribute("Position")?.Value, out positionInput);

                    int? kapasiteIdInput = 0;
                    if (input.Attribute("KapasiteId")?.Value == null) { kapasiteIdInput = null; }
                    else { kapasiteIdInput = Convert.ToInt32(input.Attribute("KapasiteId")?.Value); }

                    int? minKapasiteInput = 0;
                    if (input.Attribute("MinKapasite")?.Value == null) { minKapasiteInput = null; }
                    else { minKapasiteInput = Convert.ToInt32(input.Attribute("MinKapasite")?.Value); }

                    int? maxKapasiteInput = 0;
                    if (input.Attribute("MaxKapasite")?.Value == null) { maxKapasiteInput = null; }
                    else { maxKapasiteInput = Convert.ToInt32(input.Attribute("MaxKapasite")?.Value); }

                    int? fizikselOrtamIdInput = 0;
                    if (input.Attribute("FizikselOrtamId")?.Value == null) { fizikselOrtamIdInput = null; }
                    else { fizikselOrtamIdInput = Convert.ToInt32(input.Attribute("FizikselOrtamId")?.Value); }

                    int? gerilimTipiIdInput = 0;
                    if (input.Attribute("GerilimTipiId")?.Value == null) { gerilimTipiIdInput = null; }
                    else { gerilimTipiIdInput = Convert.ToInt32(input.Attribute("GerilimTipiId")?.Value); }

                    int kullanimAmaciIdInput = Convert.ToInt32(input.Attribute("KullanimAmaciId")?.Value);
                    int typeIdInput = Convert.ToInt32(input.Attribute("TypeId")?.Value);

                    decimal? girdiDuraganGerilimDegeri1Input = 0;
                    if (input.Attribute("GirdiDuraganGerilimDegeri1")?.Value == null) { girdiDuraganGerilimDegeri1Input = null; }
                    else { girdiDuraganGerilimDegeri1Input = Convert.ToDecimal(input.Attribute("GirdiDuraganGerilimDegeri1")?.Value); }

                    decimal? girdiDuraganGerilimDegeri2Input = 0;
                    if (input.Attribute("GirdiDuraganGerilimDegeri2")?.Value == null) { girdiDuraganGerilimDegeri2Input = null; }
                    else { girdiDuraganGerilimDegeri2Input = Convert.ToDecimal(input.Attribute("GirdiDuraganGerilimDegeri2")?.Value); }

                    decimal? girdiDuraganGerilimDegeri3Input = 0;
                    if (input.Attribute("GirdiDuraganGerilimDegeri3")?.Value == null) { girdiDuraganGerilimDegeri3Input = null; }
                    else { girdiDuraganGerilimDegeri3Input = Convert.ToDecimal(input.Attribute("GirdiDuraganGerilimDegeri3")?.Value); }

                    decimal? girdiMinimumGerilimDegeriInput = 0;
                    if (input.Attribute("GirdiMinimumGerilimDegeri")?.Value == null) { girdiMinimumGerilimDegeriInput = null; }
                    else { girdiMinimumGerilimDegeriInput = Convert.ToDecimal(input.Attribute("GirdiMinimumGerilimDegeri")?.Value); }

                    decimal? girdiMaksimumGerilimDegeriInput = 0;
                    if (input.Attribute("GirdiMaksimumGerilimDegeri")?.Value == null) { girdiMaksimumGerilimDegeriInput = null; }
                    else { girdiMaksimumGerilimDegeriInput = Convert.ToDecimal(input.Attribute("GirdiMaksimumGerilimDegeri")?.Value); }

                    decimal? girdiTukettigiGucMiktariInput = 0;
                    if (input.Attribute("GirdiTukettigiGucMiktari")?.Value == null) { girdiTukettigiGucMiktariInput = null; }
                    else { girdiTukettigiGucMiktariInput = Convert.ToDecimal(input.Attribute("GirdiTukettigiGucMiktari")?.Value); }

                    string ciktiDuraganGerilimDegeriInput;
                    if (input.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeriInput = null; }
                    else { ciktiDuraganGerilimDegeriInput = input.Attribute("CiktiDuraganGerilimDegeri")?.Value; }

                    decimal? ciktiUrettigiGucKapasitesiInput = 0;
                    if (input.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesiInput = null; }
                    else { ciktiUrettigiGucKapasitesiInput = Convert.ToDecimal(input.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                    decimal? kalanKapasiteInput = 0;
                    if (input.Attribute("KalanKapasite")?.Value == null) { kalanKapasiteInput = null; }
                    else { kalanKapasiteInput = Convert.ToDecimal(input.Attribute("KalanKapasite")?.Value); }

                    var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameInput, positionInput, uniqueIdInput, kapasiteIdInput, minKapasiteInput, maxKapasiteInput,
                        fizikselOrtamIdInput, gerilimTipiIdInput, kullanimAmaciIdInput, girdiDuraganGerilimDegeri1Input, girdiDuraganGerilimDegeri2Input, girdiDuraganGerilimDegeri3Input,
                        girdiMinimumGerilimDegeriInput, girdiMaksimumGerilimDegeriInput, girdiTukettigiGucMiktariInput, ciktiDuraganGerilimDegeriInput, ciktiUrettigiGucKapasitesiInput, labelInput,
                        typeIdInput, kalanKapasiteInput);

                    inputList.Add(newConnector);
                }
            }

            var Outputs = stateMachineXElement.Element("Outputs")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var output in Outputs)
            {
                if (new Guid(output.Attribute("NodeUniqueId")?.Value) == uniqueId)
                {
                    string nameOutput = output.Attribute("Name")?.Value;
                    string labelOutput = output.Attribute("Label")?.Value;
                    Guid uniqueIdOutput = new Guid(output.Attribute("UniqueId")?.Value);
                    Point positionOutput = new Point();
                    PointExtensition.TryParseFromString(output.Attribute("Position")?.Value, out positionOutput);

                    int? kapasiteIdOutput = 0;
                    if (output.Attribute("KapasiteId")?.Value == null) { kapasiteIdOutput = null; }
                    else { kapasiteIdOutput = Convert.ToInt32(output.Attribute("KapasiteId")?.Value); }

                    int? minKapasiteOutput = 0;
                    if (output.Attribute("MinKapasite")?.Value == null) { minKapasiteOutput = null; }
                    else { minKapasiteOutput = Convert.ToInt32(output.Attribute("MinKapasite")?.Value); }

                    int? maxKapasiteOutput = 0;
                    if (output.Attribute("MaxKapasite")?.Value == null) { maxKapasiteOutput = null; }
                    else { maxKapasiteOutput = Convert.ToInt32(output.Attribute("MaxKapasite")?.Value); }

                    int? fizikselOrtamIdOutput = 0;
                    if (output.Attribute("FizikselOrtamId")?.Value == null) { fizikselOrtamIdOutput = null; }
                    else { fizikselOrtamIdOutput = Convert.ToInt32(output.Attribute("FizikselOrtamId")?.Value); }

                    int? gerilimTipiIdOutput = 0;
                    if (output.Attribute("GerilimTipiId")?.Value == null) { gerilimTipiIdOutput = null; }
                    else { gerilimTipiIdOutput = Convert.ToInt32(output.Attribute("GerilimTipiId")?.Value); }

                    int kullanimAmaciIdOutput = Convert.ToInt32(output.Attribute("KullanimAmaciId")?.Value);
                    int typeIdOutput = Convert.ToInt32(output.Attribute("TypeId")?.Value);

                    decimal? girdiDuraganGerilimDegeri1Output = 0;
                    if (output.Attribute("GirdiDuraganGerilimDegeri1")?.Value == null) { girdiDuraganGerilimDegeri1Output = null; }
                    else { girdiDuraganGerilimDegeri1Output = Convert.ToDecimal(output.Attribute("GirdiDuraganGerilimDegeri1")?.Value); }

                    decimal? girdiDuraganGerilimDegeri2Output = 0;
                    if (output.Attribute("GirdiDuraganGerilimDegeri2")?.Value == null) { girdiDuraganGerilimDegeri2Output = null; }
                    else { girdiDuraganGerilimDegeri2Output = Convert.ToDecimal(output.Attribute("GirdiDuraganGerilimDegeri2")?.Value); }

                    decimal? girdiDuraganGerilimDegeri3Output = 0;
                    if (output.Attribute("GirdiDuraganGerilimDegeri3")?.Value == null) { girdiDuraganGerilimDegeri3Output = null; }
                    else { girdiDuraganGerilimDegeri3Output = Convert.ToDecimal(output.Attribute("GirdiDuraganGerilimDegeri3")?.Value); }

                    decimal? girdiMinimumGerilimDegeriOutput = 0;
                    if (output.Attribute("GirdiMinimumGerilimDegeri")?.Value == null) { girdiMinimumGerilimDegeriOutput = null; }
                    else { girdiMinimumGerilimDegeriOutput = Convert.ToDecimal(output.Attribute("GirdiMinimumGerilimDegeri")?.Value); }

                    decimal? girdiMaksimumGerilimDegeriOutput = 0;
                    if (output.Attribute("GirdiMaksimumGerilimDegeri")?.Value == null) { girdiMaksimumGerilimDegeriOutput = null; }
                    else { girdiMaksimumGerilimDegeriOutput = Convert.ToDecimal(output.Attribute("GirdiMaksimumGerilimDegeri")?.Value); }

                    decimal? girdiTukettigiGucMiktariOutput = 0;
                    if (output.Attribute("GirdiTukettigiGucMiktari")?.Value == null) { girdiTukettigiGucMiktariOutput = null; }
                    else { girdiTukettigiGucMiktariOutput = Convert.ToDecimal(output.Attribute("GirdiTukettigiGucMiktari")?.Value); }

                    string ciktiDuraganGerilimDegeriOutput;
                    if (output.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeriOutput = null; }
                    else { ciktiDuraganGerilimDegeriOutput = output.Attribute("CiktiDuraganGerilimDegeri")?.Value; }

                    decimal? ciktiUrettigiGucKapasitesiOutput = 0;
                    if (output.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesiOutput = null; }
                    else { ciktiUrettigiGucKapasitesiOutput = Convert.ToDecimal(output.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                    decimal? kalanKapasiteOutput = 0;
                    if (output.Attribute("KalanKapasite")?.Value == null) { kalanKapasiteOutput = null; }
                    else { kalanKapasiteOutput = Convert.ToDecimal(output.Attribute("KalanKapasite")?.Value); }

                    var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameOutput, positionOutput, uniqueIdOutput, kapasiteIdOutput, minKapasiteOutput, maxKapasiteOutput,
                        fizikselOrtamIdOutput, gerilimTipiIdOutput, kullanimAmaciIdOutput, girdiDuraganGerilimDegeri1Output, girdiDuraganGerilimDegeri2Output, girdiDuraganGerilimDegeri3Output,
                        girdiMinimumGerilimDegeriOutput, girdiMaksimumGerilimDegeriOutput, girdiTukettigiGucMiktariOutput, ciktiDuraganGerilimDegeriOutput, ciktiUrettigiGucKapasitesiOutput, labelOutput,
                        typeIdOutput, kalanKapasiteOutput);

                    newConnector.Visible = null;
                    outputList.Add(newConnector);
                }
            }

            viewModelNode.InputList = inputList;
            viewModelNode.OutputList = outputList;
            viewModelNode.AddEmptyConnector();

            return viewModelNode;
        }

        public NodeViewModel Clone()
        {
            return new NodeViewModel
            {
                AgArayuzuList = AgArayuzuList,
                CanBeDelete = CanBeDelete,
                CurrentConnector = CurrentConnector,
                GucArayuzuList = GucArayuzuList,
                HeaderWidth = HeaderWidth,
                Id = Id,
                IndexStartSelectConnectors = IndexStartSelectConnectors,
                InputList = InputList,
                IsCollapse = IsCollapse,
                IsVisible = IsVisible,
                Name = NodesCanvas.GetNameForNewNode(TypeId),
                NameEnable = NameEnable,
                NodesCanvas = NodesCanvas,
                Output = Output,
                OutputList = OutputList,
                Point1 = Point1.Addition(50,50),
                Point2 = Point2,
                RollUpVisible = RollUpVisible,
                Selected = Selected,
                Size = Size,
                Transitions = new SourceList<ConnectorViewModel>(),
                TransitionsVisible = TransitionsVisible,
                TypeId = TypeId,
                UniqueId = new Guid(),
                Sembol = Sembol,
                Zindex = Zindex
            };
        }
    }
}
