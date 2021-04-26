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
using AYP.Enums;
using System.Collections.Generic;
using AYP.Entities;

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
        [Reactive] public int? MinKapasite { get; set; }
        [Reactive] public int? MaxKapasite { get; set; }
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
        [Reactive] public List<AgAkis> AgAkisList { get; set; } = new List<AgAkis>();
        [Reactive] public decimal? KalanKapasite { get; set; }
        [Reactive] public double Artik { get; set; }
        [Reactive] public string Port { get; set; }
        [Reactive] public int Id { get; set; }


        public ConnectorViewModel(NodesCanvasViewModel nodesCanvas, NodeViewModel viewModelNode, string name, Point myPoint, Guid uniqueId, int? kapasiteId = default(int), int? minKapasite = default(int), int? maxKapasite = default(int), int? fizikselOrtamId = default(int),
            int? gerilimTipiId = default(int), int kullanimAmaciId = default(int), decimal? girdiDuraganGerilimDegeri1 = default(decimal), decimal? girdiDuraganGerilimDegeri2 = default(decimal), decimal? girdiDuraganGerilimDegeri3 = default(decimal),
            decimal? girdiMinimumGerilimDegeri = default(decimal), decimal? girdiMaksimumGerilimDegeri = default(decimal), decimal? girdiTukettigiGucMiktari = default(decimal),
            string ciktiDuraganGerilimDegeri = default(string), decimal? ciktiUrettigiGucKapasitesi = default(decimal), string label = default(string), int typeId = default(int), int id = default(int), string port = default, decimal? kalanKapasite = default)
        {
            Node = viewModelNode;
            NodesCanvas = nodesCanvas;
            Name = name;
            Label = label;
            PositionConnectPoint = myPoint;
            UniqueId = uniqueId;
            KapasiteId = kapasiteId;
            MinKapasite = minKapasite;
            MaxKapasite = maxKapasite;
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
            KalanKapasite = kalanKapasite;
            Id = id;
            Port = port;

            SetupCommands();
            SetupSubscriptions();
        }
        #region Setup Subscriptions
        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => Select(value));
            this.WhenAnyValue(x => x.NodesCanvas.Theme).Subscribe(_ => UpdateResources());

            if (this.Name != "Girdi")
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
                this.PositionConnectPoint = Node.CurrentConnector.PositionConnectPoint.Addition(0, index);
            }
        }
        private void UpdatePositionOnWidthChange(double value)
        {
            Artik = value;
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

        public XElement ToInputXElement()
        {
            XElement element = new XElement("Input");
            element.Add(new XAttribute("NodeUniqueId", Node.UniqueId));
            element.Add(new XAttribute("NodeName", Node.Name));
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Label", Label));
            element.Add(new XAttribute("UniqueId", UniqueId));
            element.Add(new XAttribute("Position", PointExtensition.PointToString(PositionConnectPoint)));
            element.Add(!KapasiteId.HasValue ? null : new XAttribute("KapasiteId", KapasiteId));
            element.Add(!MinKapasite.HasValue ? null : new XAttribute("MinKapasite", MinKapasite));
            element.Add(!MaxKapasite.HasValue ? null : new XAttribute("MaxKapasite", MaxKapasite));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(!FizikselOrtamId.HasValue ? null : new XAttribute("FizikselOrtamId", FizikselOrtamId));
            element.Add(!GerilimTipiId.HasValue ? null : new XAttribute("GerilimTipiId", GerilimTipiId));
            element.Add(!GirdiDuraganGerilimDegeri1.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri1", GirdiDuraganGerilimDegeri1));
            element.Add(!GirdiDuraganGerilimDegeri2.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri2", GirdiDuraganGerilimDegeri2));
            element.Add(!GirdiDuraganGerilimDegeri3.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri3", GirdiDuraganGerilimDegeri3));
            element.Add(!GirdiMinimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMinimumGerilimDegeri", GirdiMinimumGerilimDegeri));
            element.Add(!GirdiMaksimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMaksimumGerilimDegeri", GirdiMaksimumGerilimDegeri));
            element.Add(!GirdiTukettigiGucMiktari.HasValue ? null : new XAttribute("GirdiTukettigiGucMiktari", GirdiTukettigiGucMiktari));
            element.Add(CiktiDuraganGerilimDegeri == null ? null : new XAttribute("CiktiDuraganGerilimDegeri", CiktiDuraganGerilimDegeri));
            element.Add(!CiktiUrettigiGucKapasitesi.HasValue ? null : new XAttribute("CiktiUrettigiGucKapasitesi", CiktiUrettigiGucKapasitesi));
            element.Add(new XAttribute("TypeId", TypeId));
            element.Add(!KalanKapasite.HasValue ? null : new XAttribute("KalanKapasite", KalanKapasite));
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("Artik", Artik));

            return element;
        }

        public XElement ToOutputXElement()
        {
            XElement element = new XElement("Output");
            element.Add(new XAttribute("NodeUniqueId", Node.UniqueId));
            element.Add(new XAttribute("NodeName", Node.Name));
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Label", Label));
            element.Add(new XAttribute("UniqueId", UniqueId));
            element.Add(new XAttribute("Position", PointExtensition.PointToString(PositionConnectPoint.Addition(Artik * -1, 0))));
            element.Add(!KapasiteId.HasValue ? null : new XAttribute("KapasiteId", KapasiteId));
            element.Add(!MinKapasite.HasValue ? null : new XAttribute("MinKapasite", MinKapasite));
            element.Add(!MaxKapasite.HasValue ? null : new XAttribute("MaxKapasite", MaxKapasite));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(!FizikselOrtamId.HasValue ? null : new XAttribute("FizikselOrtamId", FizikselOrtamId));
            element.Add(!GerilimTipiId.HasValue ? null : new XAttribute("GerilimTipiId", GerilimTipiId));
            element.Add(!GirdiDuraganGerilimDegeri1.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri1", GirdiDuraganGerilimDegeri1));
            element.Add(!GirdiDuraganGerilimDegeri2.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri2", GirdiDuraganGerilimDegeri2));
            element.Add(!GirdiDuraganGerilimDegeri3.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri3", GirdiDuraganGerilimDegeri3));
            element.Add(!GirdiMinimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMinimumGerilimDegeri", GirdiMinimumGerilimDegeri));
            element.Add(!GirdiMaksimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMaksimumGerilimDegeri", GirdiMaksimumGerilimDegeri));
            element.Add(!GirdiTukettigiGucMiktari.HasValue ? null : new XAttribute("GirdiTukettigiGucMiktari", GirdiTukettigiGucMiktari));
            element.Add(CiktiDuraganGerilimDegeri == null ? null : new XAttribute("CiktiDuraganGerilimDegeri", CiktiDuraganGerilimDegeri));
            element.Add(!CiktiUrettigiGucKapasitesi.HasValue ? null : new XAttribute("CiktiUrettigiGucKapasitesi", CiktiUrettigiGucKapasitesi));
            element.Add(new XAttribute("TypeId", TypeId));
            element.Add(!KalanKapasite.HasValue ? null : new XAttribute("KalanKapasite", KalanKapasite));
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("Artik", Artik));


            return element;
        }
    }
}
