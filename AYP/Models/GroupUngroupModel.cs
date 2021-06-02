using System;
using System.Collections.Generic;
using System.Text;
using AYP.Entities;
using AYP.ViewModel;
using System.Xml.Linq;
using ReactiveUI;
using System.Linq;
using System.Windows;
using AYP.Helpers.Extensions;
using AYP.DbContext.AYP.DbContexts;
using AYP.Interfaces;
using AYP.Services;

namespace AYP.Models
{
    public class GroupUngroupModel : ReactiveObject
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public Guid UniqueId { get; set; }
        public List<NodeViewModel> NodeList { get; set; }
        public List<ConnectViewModel> InternalConnectList { get; set; }
        public List<ConnectViewModel> ExternalConnectList { get; set; }
        public List<AgArayuzu> AgArayuzuList { get; set; }
        public List<GucArayuzu> GucArayuzuList { get; set; }

        public XElement ToXElement()
        {
            XElement element = new XElement("Group");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("UniqueId", UniqueId));

            return element;
        }

        public static GroupUngroupModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck, XElement stateMachineXElement)
        {
            errorMessage = null;
            GroupUngroupModel viewModelGroup = new GroupUngroupModel();

            Guid uniqueId = new Guid(node.Attribute("UniqueId")?.Value);
            string name = node.Attribute("Name")?.Value;

            viewModelGroup.UniqueId = uniqueId;
            viewModelGroup.Name = name;
            viewModelGroup.NodeList = new List<NodeViewModel>();
            viewModelGroup.InternalConnectList = new List<ConnectViewModel>();
            viewModelGroup.ExternalConnectList = new List<ConnectViewModel>();
            viewModelGroup.GucArayuzuList = new List<GucArayuzu>();
            viewModelGroup.AgArayuzuList = new List<AgArayuzu>();

            var GroupNodes = stateMachineXElement.Element("GroupNodes")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var groupNode in GroupNodes)
            {
                if (new Guid(groupNode.Attribute("GroupId")?.Value) == uniqueId)
                {
                    Guid nodeUniqueId = new Guid(groupNode.Attribute("UniqueId")?.Value);
                    int id = Convert.ToInt32(groupNode.Attribute("Id")?.Value);
                    string nodeName = groupNode.Attribute("Name")?.Value;
                    string tanim = groupNode.Attribute("Tanim")?.Value;
                    string stokNo = groupNode.Attribute("StokNo")?.Value;
                    string ureticiAdi = groupNode.Attribute("UreticiAdi")?.Value;
                    string ureticiParcaNo = groupNode.Attribute("UreticiParcaNo")?.Value;
                    string turAd = groupNode.Attribute("Tur")?.Value;
                    int typeId = Convert.ToInt32(groupNode.Attribute("TypeId")?.Value);
                    Point position = new Point();
                    PointExtensition.TryParseFromString(groupNode.Attribute("Position")?.Value, out position);
                    decimal? verimlilikOrani = 0;
                    if (groupNode.Attribute("VerimlilikOrani")?.Value == null) { verimlilikOrani = null; }
                    else { verimlilikOrani = Convert.ToDecimal(groupNode.Attribute("VerimlilikOrani")?.Value); }
                    decimal? dahiliGucTuketimDegeri = 0;
                    if (groupNode.Attribute("DahiliGucTuketimDegeri")?.Value == null) { dahiliGucTuketimDegeri = null; }
                    else { dahiliGucTuketimDegeri = Convert.ToDecimal(groupNode.Attribute("DahiliGucTuketimDegeri")?.Value); }
                    byte[] sembol = Convert.FromBase64String(groupNode.Attribute("Sembol")?.Value);

                    NodeViewModel viewModelNode = new NodeViewModel(nodesCanvas, nodeName, nodeUniqueId, position, id, typeId, null, null, null, null, verimlilikOrani, dahiliGucTuketimDegeri, sembol, stokNo, tanim, ureticiAdi, ureticiParcaNo, turAd);
                    var inputList = new List<ConnectorViewModel>();
                    var outputList = new List<ConnectorViewModel>();
                    var agArayuzuList = new List<AgArayuzu>();
                    var gucArayuzuList = new List<GucArayuzu>();
                    var GroupNodeInputs = stateMachineXElement.Element("GroupNodeInputs")?.Elements()?.ToList() ?? new List<XElement>();
                    foreach (var input in GroupNodeInputs)
                    {
                        if (new Guid(input.Attribute("NodeUniqueId")?.Value) == nodeUniqueId)
                        {
                            string portInput = input.Attribute("Port")?.Value;
                            int idInput = Convert.ToInt32(input.Attribute("Id")?.Value);

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

                            decimal? ciktiDuraganGerilimDegeriInput;
                            if (input.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeriInput = null; }
                            else { ciktiDuraganGerilimDegeriInput = Convert.ToDecimal(input.Attribute("CiktiDuraganGerilimDegeri")?.Value); }

                            decimal? ciktiUrettigiGucKapasitesiInput = 0;
                            if (input.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesiInput = null; }
                            else { ciktiUrettigiGucKapasitesiInput = Convert.ToDecimal(input.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                            decimal? kalanKapasiteInput = 0;
                            if (input.Attribute("KalanKapasite")?.Value == null) { kalanKapasiteInput = null; }
                            else { kalanKapasiteInput = Convert.ToDecimal(input.Attribute("KalanKapasite")?.Value); }

                            var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameInput, positionInput, uniqueIdInput, kapasiteIdInput, minKapasiteInput, maxKapasiteInput,
                                fizikselOrtamIdInput, gerilimTipiIdInput, kullanimAmaciIdInput, girdiDuraganGerilimDegeri1Input, girdiDuraganGerilimDegeri2Input, girdiDuraganGerilimDegeri3Input,
                                girdiMinimumGerilimDegeriInput, girdiMaksimumGerilimDegeriInput, girdiTukettigiGucMiktariInput, ciktiDuraganGerilimDegeriInput, ciktiUrettigiGucKapasitesiInput, labelInput,
                                typeIdInput, idInput, portInput, kalanKapasiteInput);

                            inputList.Add(newConnector);
                        }
                    }

                    var GroupNodeOutputs = stateMachineXElement.Element("GroupNodeOutputs")?.Elements()?.ToList() ?? new List<XElement>();
                    foreach (var output in GroupNodeOutputs)
                    {
                        if (new Guid(output.Attribute("NodeUniqueId")?.Value) == nodeUniqueId)
                        {
                            string portOutput = output.Attribute("Port")?.Value;
                            int idOutput = Convert.ToInt32(output.Attribute("Id")?.Value);
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

                            decimal? ciktiDuraganGerilimDegeriOutput;
                            if (output.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeriOutput = null; }
                            else { ciktiDuraganGerilimDegeriOutput = Convert.ToDecimal(output.Attribute("CiktiDuraganGerilimDegeri")?.Value); }

                            decimal? ciktiUrettigiGucKapasitesiOutput = 0;
                            if (output.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesiOutput = null; }
                            else { ciktiUrettigiGucKapasitesiOutput = Convert.ToDecimal(output.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                            decimal? kalanKapasiteOutput = 0;
                            if (output.Attribute("KalanKapasite")?.Value == null) { kalanKapasiteOutput = null; }
                            else { kalanKapasiteOutput = Convert.ToDecimal(output.Attribute("KalanKapasite")?.Value); }

                            var newConnector = new ConnectorViewModel(nodesCanvas, viewModelNode, nameOutput, positionOutput, uniqueIdOutput, kapasiteIdOutput, minKapasiteOutput, maxKapasiteOutput,
                                fizikselOrtamIdOutput, gerilimTipiIdOutput, kullanimAmaciIdOutput, girdiDuraganGerilimDegeri1Output, girdiDuraganGerilimDegeri2Output, girdiDuraganGerilimDegeri3Output,
                                girdiMinimumGerilimDegeriOutput, girdiMaksimumGerilimDegeriOutput, girdiTukettigiGucMiktariOutput, ciktiDuraganGerilimDegeriOutput, ciktiUrettigiGucKapasitesiOutput, labelOutput,
                                typeIdOutput, idOutput, portOutput, kalanKapasiteOutput);

                            newConnector.Visible = null;
                            outputList.Add(newConnector);
                        }
                    }

                    var GroupNodeGucArayuzus = stateMachineXElement.Element("GroupNodeGucArayuzus")?.Elements()?.ToList() ?? new List<XElement>();
                    foreach (var gucArayuzu in GroupNodeGucArayuzus)
                    {
                        if (new Guid(gucArayuzu.Attribute("NodeUniqueId")?.Value) == nodeUniqueId)
                        {
                            GucArayuzu item = new GucArayuzu();

                            int itemId = Convert.ToInt32(gucArayuzu.Attribute("Id")?.Value);
                            string adi = gucArayuzu.Attribute("Adi")?.Value;
                            string port = gucArayuzu.Attribute("Port")?.Value;
                            int kullanimAmaciId = Convert.ToInt32(gucArayuzu.Attribute("KullanimAmaciId")?.Value);
                            int gerilimTipiId = Convert.ToInt32(gucArayuzu.Attribute("GerilimTipiId")?.Value);
                            int tipId = Convert.ToInt32(gucArayuzu.Attribute("TipId")?.Value);

                            decimal? girdiDuraganGerilimDegeri1 = 0;
                            if (gucArayuzu.Attribute("GirdiDuraganGerilimDegeri1")?.Value == null) { girdiDuraganGerilimDegeri1 = null; }
                            else { girdiDuraganGerilimDegeri1 = Convert.ToDecimal(gucArayuzu.Attribute("GirdiDuraganGerilimDegeri1")?.Value); }

                            decimal? girdiDuraganGerilimDegeri2 = 0;
                            if (gucArayuzu.Attribute("GirdiDuraganGerilimDegeri2")?.Value == null) { girdiDuraganGerilimDegeri2 = null; }
                            else { girdiDuraganGerilimDegeri2 = Convert.ToDecimal(gucArayuzu.Attribute("GirdiDuraganGerilimDegeri2")?.Value); }

                            decimal? girdiDuraganGerilimDegeri3 = 0;
                            if (gucArayuzu.Attribute("GirdiDuraganGerilimDegeri3")?.Value == null) { girdiDuraganGerilimDegeri3 = null; }
                            else { girdiDuraganGerilimDegeri3 = Convert.ToDecimal(gucArayuzu.Attribute("GirdiDuraganGerilimDegeri3")?.Value); }

                            decimal? girdiMinimumGerilimDegeri = 0;
                            if (gucArayuzu.Attribute("GirdiMinimumGerilimDegeri")?.Value == null) { girdiMinimumGerilimDegeri = null; }
                            else { girdiMinimumGerilimDegeri = Convert.ToDecimal(gucArayuzu.Attribute("GirdiMinimumGerilimDegeri")?.Value); }

                            decimal? girdiMaksimumGerilimDegeri = 0;
                            if (gucArayuzu.Attribute("GirdiMaksimumGerilimDegeri")?.Value == null) { girdiMaksimumGerilimDegeri = null; }
                            else { girdiMaksimumGerilimDegeri = Convert.ToDecimal(gucArayuzu.Attribute("GirdiMaksimumGerilimDegeri")?.Value); }

                            decimal? girdiTukettigiGucMiktari = 0;
                            if (gucArayuzu.Attribute("GirdiTukettigiGucMiktari")?.Value == null) { girdiTukettigiGucMiktari = null; }
                            else { girdiTukettigiGucMiktari = Convert.ToDecimal(gucArayuzu.Attribute("GirdiTukettigiGucMiktari")?.Value); }

                            decimal? ciktiUrettigiGucKapasitesi = 0;
                            if (gucArayuzu.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesi = null; }
                            else { ciktiUrettigiGucKapasitesi = Convert.ToDecimal(gucArayuzu.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                            decimal? ciktiDuraganGerilimDegeri = 0;
                            if (gucArayuzu.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeri = null; }
                            else { ciktiDuraganGerilimDegeri = Convert.ToDecimal(gucArayuzu.Attribute("CiktiDuraganGerilimDegeri")?.Value); }

                            item.Adi = adi;
                            item.Id = itemId;
                            item.CiktiDuraganGerilimDegeri = ciktiDuraganGerilimDegeri;
                            item.CiktiUrettigiGucKapasitesi = ciktiUrettigiGucKapasitesi;
                            item.GerilimTipiId = gerilimTipiId;
                            item.GirdiDuraganGerilimDegeri1 = girdiDuraganGerilimDegeri1;
                            item.GirdiDuraganGerilimDegeri2 = girdiDuraganGerilimDegeri2;
                            item.GirdiDuraganGerilimDegeri3 = girdiDuraganGerilimDegeri3;
                            item.GirdiMaksimumGerilimDegeri = girdiMaksimumGerilimDegeri;
                            item.GirdiMinimumGerilimDegeri = girdiMinimumGerilimDegeri;
                            item.GirdiTukettigiGucMiktari = girdiTukettigiGucMiktari;
                            item.KullanimAmaciId = kullanimAmaciId;
                            item.Port = port;
                            item.TipId = tipId;

                            gucArayuzuList.Add(item);
                        }
                    }

                    var GroupNodeAgArayuzus = stateMachineXElement.Element("GroupNodeAgArayuzus")?.Elements()?.ToList() ?? new List<XElement>();
                    foreach (var agArayuzu in GroupNodeAgArayuzus)
                    {
                        if (new Guid(agArayuzu.Attribute("NodeUniqueId")?.Value) == nodeUniqueId)
                        {
                            AgArayuzu item = new AgArayuzu();

                            int itemId = Convert.ToInt32(agArayuzu.Attribute("Id")?.Value);
                            string adi = agArayuzu.Attribute("Adi")?.Value;
                            string port = agArayuzu.Attribute("Port")?.Value;
                            int kullanimAmaciId = Convert.ToInt32(agArayuzu.Attribute("KullanimAmaciId")?.Value);
                            int fizikselOrtamId = Convert.ToInt32(agArayuzu.Attribute("FizikselOrtamId")?.Value);
                            int kapasiteId = Convert.ToInt32(agArayuzu.Attribute("KapasiteId")?.Value);
                            int tipId = Convert.ToInt32(agArayuzu.Attribute("TipId")?.Value);


                            IKodListeService service = new KodListeService();
                            item.Adi = adi;
                            item.Id = itemId;
                            item.FizikselOrtamId = fizikselOrtamId;
                            item.KapasiteId = kapasiteId;
                            item.KL_Kapasite = service.GetKapasiteById(kapasiteId);
                            item.KullanimAmaciId = kullanimAmaciId;
                            item.Port = port;
                            item.TipId = tipId;


                            agArayuzuList.Add(item);
                        }
                    }

                    viewModelNode.InputList = inputList;
                    viewModelNode.OutputList = outputList;
                    viewModelNode.AgArayuzuList = agArayuzuList;
                    viewModelNode.GucArayuzuList = gucArayuzuList;
                    viewModelNode.AddEmptyConnector();
                    viewModelGroup.NodeList.Add(viewModelNode);
                }
            }

            var GroupGucArayuzus = stateMachineXElement.Element("GroupGucArayuzus")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var GroupGucArayuzu in GroupGucArayuzus)
            {
                if (new Guid(GroupGucArayuzu.Attribute("GroupId")?.Value) == uniqueId)
                {
                    GucArayuzu gucArayuzu = null;

                    int itemId = Convert.ToInt32(GroupGucArayuzu.Attribute("Id")?.Value);
                    string adi = GroupGucArayuzu.Attribute("Adi")?.Value;
                    string port = GroupGucArayuzu.Attribute("Port")?.Value;
                    int kullanimAmaciId = Convert.ToInt32(GroupGucArayuzu.Attribute("KullanimAmaciId")?.Value);
                    int gerilimTipiId = Convert.ToInt32(GroupGucArayuzu.Attribute("GerilimTipiId")?.Value);
                    int tipId = Convert.ToInt32(GroupGucArayuzu.Attribute("TipId")?.Value);

                    decimal? girdiDuraganGerilimDegeri1 = 0;
                    if (GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri1")?.Value == null) { girdiDuraganGerilimDegeri1 = null; }
                    else { girdiDuraganGerilimDegeri1 = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri1")?.Value); }

                    decimal? girdiDuraganGerilimDegeri2 = 0;
                    if (GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri2")?.Value == null) { girdiDuraganGerilimDegeri2 = null; }
                    else { girdiDuraganGerilimDegeri2 = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri2")?.Value); }

                    decimal? girdiDuraganGerilimDegeri3 = 0;
                    if (GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri3")?.Value == null) { girdiDuraganGerilimDegeri3 = null; }
                    else { girdiDuraganGerilimDegeri3 = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiDuraganGerilimDegeri3")?.Value); }

                    decimal? girdiMinimumGerilimDegeri = 0;
                    if (GroupGucArayuzu.Attribute("GirdiMinimumGerilimDegeri")?.Value == null) { girdiMinimumGerilimDegeri = null; }
                    else { girdiMinimumGerilimDegeri = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiMinimumGerilimDegeri")?.Value); }

                    decimal? girdiMaksimumGerilimDegeri = 0;
                    if (GroupGucArayuzu.Attribute("GirdiMaksimumGerilimDegeri")?.Value == null) { girdiMaksimumGerilimDegeri = null; }
                    else { girdiMaksimumGerilimDegeri = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiMaksimumGerilimDegeri")?.Value); }

                    decimal? girdiTukettigiGucMiktari = 0;
                    if (GroupGucArayuzu.Attribute("GirdiTukettigiGucMiktari")?.Value == null) { girdiTukettigiGucMiktari = null; }
                    else { girdiTukettigiGucMiktari = Convert.ToDecimal(GroupGucArayuzu.Attribute("GirdiTukettigiGucMiktari")?.Value); }

                    decimal? ciktiUrettigiGucKapasitesi = 0;
                    if (GroupGucArayuzu.Attribute("CiktiUrettigiGucKapasitesi")?.Value == null) { ciktiUrettigiGucKapasitesi = null; }
                    else { ciktiUrettigiGucKapasitesi = Convert.ToDecimal(GroupGucArayuzu.Attribute("CiktiUrettigiGucKapasitesi")?.Value); }

                    decimal? ciktiDuraganGerilimDegeri = 0;
                    if (GroupGucArayuzu.Attribute("CiktiDuraganGerilimDegeri")?.Value == null) { ciktiDuraganGerilimDegeri = null; }
                    else { ciktiDuraganGerilimDegeri = Convert.ToDecimal(GroupGucArayuzu.Attribute("CiktiDuraganGerilimDegeri")?.Value); }

                    gucArayuzu.Adi = adi;
                    gucArayuzu.Id = itemId;
                    gucArayuzu.CiktiDuraganGerilimDegeri = ciktiDuraganGerilimDegeri;
                    gucArayuzu.CiktiUrettigiGucKapasitesi = ciktiUrettigiGucKapasitesi;
                    gucArayuzu.GerilimTipiId = gerilimTipiId;
                    gucArayuzu.GirdiDuraganGerilimDegeri1 = girdiDuraganGerilimDegeri1;
                    gucArayuzu.GirdiDuraganGerilimDegeri2 = girdiDuraganGerilimDegeri2;
                    gucArayuzu.GirdiDuraganGerilimDegeri3 = girdiDuraganGerilimDegeri3;
                    gucArayuzu.GirdiMaksimumGerilimDegeri = girdiMaksimumGerilimDegeri;
                    gucArayuzu.GirdiMinimumGerilimDegeri = girdiMinimumGerilimDegeri;
                    gucArayuzu.GirdiTukettigiGucMiktari = girdiTukettigiGucMiktari;
                    gucArayuzu.KullanimAmaciId = kullanimAmaciId;
                    gucArayuzu.Port = port;
                    gucArayuzu.TipId = tipId;

                    viewModelGroup.GucArayuzuList.Add(gucArayuzu);
                }
            }

            var GroupAgArayuzus = stateMachineXElement.Element("GroupAgArayuzus")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var GroupAgArayuzu in GroupAgArayuzus)
            {
                if (new Guid(GroupAgArayuzu.Attribute("GroupId")?.Value) == uniqueId)
                {
                    AgArayuzu agArayuzu = new AgArayuzu();

                    int itemId = Convert.ToInt32(GroupAgArayuzu.Attribute("Id")?.Value);
                    string adi = GroupAgArayuzu.Attribute("Adi")?.Value;
                    string port = GroupAgArayuzu.Attribute("Port")?.Value;
                    int kullanimAmaciId = Convert.ToInt32(GroupAgArayuzu.Attribute("KullanimAmaciId")?.Value);
                    int fizikselOrtamId = Convert.ToInt32(GroupAgArayuzu.Attribute("FizikselOrtamId")?.Value);
                    int kapasiteId = Convert.ToInt32(GroupAgArayuzu.Attribute("KapasiteId")?.Value);
                    int tipId = Convert.ToInt32(GroupAgArayuzu.Attribute("TipId")?.Value);


                    IKodListeService service = new KodListeService();
                    agArayuzu.Adi = adi;
                    agArayuzu.Id = itemId;
                    agArayuzu.FizikselOrtamId = fizikselOrtamId;
                    agArayuzu.KapasiteId = kapasiteId;
                    agArayuzu.KL_Kapasite = service.GetKapasiteById(kapasiteId);
                    agArayuzu.KullanimAmaciId = kullanimAmaciId;
                    agArayuzu.Port = port;
                    agArayuzu.TipId = tipId;


                    viewModelGroup.AgArayuzuList.Add(agArayuzu);
                }
            }

            var GroupInternalConnects = stateMachineXElement.Element("GroupInternalConnects")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var groupInternalConnect in GroupInternalConnects)
            {
                if (new Guid(groupInternalConnect.Attribute("GroupId")?.Value) == uniqueId)
                {
                    ConnectViewModel viewModelConnect = null;

                    Guid fromConnectorUniqueId = new Guid(groupInternalConnect.Attribute("FromConnectorUniqueId")?.Value);
                    Guid fromConnectorNodeUniqueId = new Guid(groupInternalConnect.Attribute("FromConnectorNodeUniqueId")?.Value);
                    Guid toConnectorUniqueId = new Guid(groupInternalConnect.Attribute("ToConnectorUniqueId")?.Value);
                    Guid toConnectorNodeUniqueId = new Guid(groupInternalConnect.Attribute("ToConnectorNodeUniqueId")?.Value);
                    decimal kabloUzunlugu = Convert.ToDecimal(groupInternalConnect.Attribute("KabloUzunlugu")?.Value);
                    decimal agYuku = Convert.ToDecimal(groupInternalConnect.Attribute("AgYuku")?.Value);
                    decimal gucMiktari = Convert.ToDecimal(groupInternalConnect.Attribute("GucMiktari")?.Value);

                    Point point1 = new Point();
                    PointExtensition.TryParseFromString(groupInternalConnect.Attribute("Point1")?.Value, out point1);

                    Point point2 = new Point();
                    PointExtensition.TryParseFromString(groupInternalConnect.Attribute("Point2")?.Value, out point2);

                    Point startPoint = new Point();
                    PointExtensition.TryParseFromString(groupInternalConnect.Attribute("StartPoint")?.Value, out startPoint);

                    Point endPoint = new Point();
                    PointExtensition.TryParseFromString(groupInternalConnect.Attribute("EndPoint")?.Value, out endPoint);

                    var fromConnectorNodeOutputList = viewModelGroup.NodeList.Where(x => x.UniqueId == fromConnectorNodeUniqueId).Select(s => s.Transitions).FirstOrDefault();
                    var fromConnector = fromConnectorNodeOutputList.Items.Where(x => x.UniqueId == fromConnectorUniqueId).FirstOrDefault();

                    var toConnectorNodeInputList = viewModelGroup.NodeList.Where(x => x.UniqueId == toConnectorNodeUniqueId).Select(s => s.InputList).FirstOrDefault();
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

                    var AgAkislari = stateMachineXElement.Element("GroupNodeAgAkislari")?.Elements()?.ToList() ?? new List<XElement>();
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


                    viewModelGroup.InternalConnectList.Add(viewModelConnect);
                }
            }


            var GroupExternalConnects = stateMachineXElement.Element("GroupExternalConnects")?.Elements()?.ToList() ?? new List<XElement>();
            foreach (var groupExternalConnect in GroupExternalConnects)
            {
                if (new Guid(groupExternalConnect.Attribute("GroupId")?.Value) == uniqueId)
                {
                    ConnectViewModel viewModelConnect = null;

                    Guid fromConnectorUniqueId = new Guid(groupExternalConnect.Attribute("FromConnectorUniqueId")?.Value);
                    Guid fromConnectorNodeUniqueId = new Guid(groupExternalConnect.Attribute("FromConnectorNodeUniqueId")?.Value);
                    Guid toConnectorUniqueId = new Guid(groupExternalConnect.Attribute("ToConnectorUniqueId")?.Value);
                    Guid toConnectorNodeUniqueId = new Guid(groupExternalConnect.Attribute("ToConnectorNodeUniqueId")?.Value);
                    decimal kabloUzunlugu = Convert.ToDecimal(groupExternalConnect.Attribute("KabloUzunlugu")?.Value);
                    decimal agYuku = Convert.ToDecimal(groupExternalConnect.Attribute("AgYuku")?.Value);
                    decimal gucMiktari = Convert.ToDecimal(groupExternalConnect.Attribute("GucMiktari")?.Value);

                    Point point1 = new Point();
                    PointExtensition.TryParseFromString(groupExternalConnect.Attribute("Point1")?.Value, out point1);

                    Point point2 = new Point();
                    PointExtensition.TryParseFromString(groupExternalConnect.Attribute("Point2")?.Value, out point2);

                    Point startPoint = new Point();
                    PointExtensition.TryParseFromString(groupExternalConnect.Attribute("StartPoint")?.Value, out startPoint);

                    Point endPoint = new Point();
                    PointExtensition.TryParseFromString(groupExternalConnect.Attribute("EndPoint")?.Value, out endPoint);

                    var fromConnectorNodeOutputList = viewModelGroup.NodeList.Where(x => x.UniqueId == fromConnectorNodeUniqueId).Select(s => s.Transitions).FirstOrDefault();
                    var fromConnector = fromConnectorNodeOutputList.Items.Where(x => x.UniqueId == fromConnectorUniqueId).FirstOrDefault();
                    var toConnectorNodeInputList = new List<ConnectorViewModel>();
                    ConnectorViewModel toConnector = null;

                    if (fromConnectorNodeOutputList != null)
                    {
                        viewModelConnect = new ConnectViewModel(nodesCanvas, fromConnector);

                        toConnectorNodeInputList = nodesCanvas.Nodes.Items.Where(x => x.UniqueId == toConnectorNodeUniqueId).Select(s => s.InputList).FirstOrDefault();
                        toConnector = toConnectorNodeInputList.Where(x => x.UniqueId == toConnectorUniqueId).FirstOrDefault();

                        viewModelConnect.ToConnector = toConnector;
                        viewModelConnect.Uzunluk = kabloUzunlugu;
                        viewModelConnect.AgYuku = agYuku;
                        viewModelConnect.GucMiktari = gucMiktari;
                        viewModelConnect.StartPoint = startPoint;
                        viewModelConnect.EndPoint = endPoint;
                        viewModelConnect.Point1 = point1;
                        viewModelConnect.Point2 = point2;
                        fromConnector.Connect = viewModelConnect;
                    }
                    else
                    {
                        fromConnectorNodeOutputList = nodesCanvas.Nodes.Items.Where(x => x.UniqueId == fromConnectorNodeUniqueId).Select(s => s.Transitions).FirstOrDefault();
                        fromConnector = fromConnectorNodeOutputList.Items.Where(x => x.UniqueId == fromConnectorUniqueId).FirstOrDefault();
                        viewModelConnect = new ConnectViewModel(nodesCanvas, fromConnector);

                        toConnectorNodeInputList = viewModelGroup.NodeList.Where(x => x.UniqueId == toConnectorNodeUniqueId).Select(s => s.InputList).FirstOrDefault();
                        toConnector = toConnectorNodeInputList.Where(x => x.UniqueId == toConnectorUniqueId).FirstOrDefault();
                        viewModelConnect.ToConnector = toConnector;
                        viewModelConnect.Uzunluk = kabloUzunlugu;
                        viewModelConnect.AgYuku = agYuku;
                        viewModelConnect.GucMiktari = gucMiktari;
                        viewModelConnect.StartPoint = startPoint;
                        viewModelConnect.EndPoint = endPoint;
                        viewModelConnect.Point1 = point1;
                        viewModelConnect.Point2 = point2;
                        fromConnector.Connect = viewModelConnect;
                    }


                    IKodListeService service = new KodListeService();

                    var AgAkislari = stateMachineXElement.Element("GroupNodeAgAkislari")?.Elements()?.ToList() ?? new List<XElement>();
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


                    viewModelGroup.ExternalConnectList.Add(viewModelConnect);
                }
            }

            return viewModelGroup;
        }
    }
}
