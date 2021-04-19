using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers;
using AYP.Helpers.Commands;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using AYP.ViewModel.Node;
using DynamicData;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Matrix = System.Windows.Media.Matrix;

namespace AYP.ViewModel
{
    public class GroupModel
    {
        public Guid UniqueId { get; set; }
        public List<NodeViewModel> NodeList { get; set; }
        public List<ConnectorViewModel> TransitionList { get; set; }
        public List<AgArayuzu> AgArayuzuList { get; set; }
        public List<GucArayuzu> GucArayuzuList { get; set; }

    }

    public partial class NodesCanvasViewModel
    {
        private List<NodeViewModel> NodeClipboard;
        private List<ConnectorViewModel> TransitionClipboard;

        private List<GroupModel> GroupList;

        #region commands without parameter
        public ReactiveCommand<Unit, Unit> CommandNew { get; set; }
        public ReactiveCommand<Unit, Unit> CommandRedo { get; set; }
        public ReactiveCommand<Unit, Unit> CommandUndo { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSelectAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandUnSelectAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSelectorIntersect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCutterIntersect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandDeleteDraggedConnect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomIn { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomOut { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomOriginalSize { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCollapseUpAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExpandDownAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCollapseUpSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExpandDownSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandErrorListUpdate { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExportToPNG { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExportToJPEG { get; set; }
        public ReactiveCommand<Unit, Unit> CommandOpen { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSave { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSaveAs { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExit { get; set; }

        public ReactiveCommand<Unit, Unit> CommandChangeTheme { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCopy { get; set; }
        public ReactiveCommand<Unit, Unit> CommandPaste { get; set; }
        public ReactiveCommand<Unit, Unit> CommandEditSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCopyMultiple { get; set; }
        public ReactiveCommand<Unit, Unit> CommandGroup { get; set; }
        public ReactiveCommand<Unit, Unit> CommandUngroup { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZincirTopolojiOlustur { get; set; }
        public ReactiveCommand<Unit, Unit> CommandHalkaTopolojiOlustur { get; set; }
        public ReactiveCommand<Unit, Unit> CommandYildizTopolojiOlustur { get; set; }


        #endregion commands without parameter

        #region commands with parameter

        public ReactiveCommand<ConnectViewModel, Unit> CommandAddConnect { get; set; }
        public ReactiveCommand<ConnectViewModel, Unit> CommandDeleteConnect { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandAddDraggedConnect { get; set; }
        public ReactiveCommand<(NodeViewModel objectForValidate, string newValue), Unit> CommandValidateNodeName { get; set; }
        public ReactiveCommand<(ConnectorViewModel objectForValidate, string newValue), Unit> CommandValidateConnectName { get; set; }
        public ReactiveCommand<(Point point, double delta), Unit> CommandZoom { get; set; }
        public ReactiveCommand<Point, Unit> CommandSelect { get; set; }
        public ReactiveCommand<Point, Unit> CommandCut { get; set; }
        public ReactiveCommand<Point, Unit> CommandPartMoveAllNode { get; set; }
        public ReactiveCommand<Point, Unit> CommandPartMoveAllSelectedNode { get; set; }
        public ReactiveCommand<Point, Unit> CommandAlignLeft { get; set; }
        public ReactiveCommand<Point, Unit> CommandAlignRight { get; set; }
        public ReactiveCommand<Point, Unit> CommandAlignCenter { get; set; }

        public ReactiveCommand<string, Unit> CommandLogDebug { get; set; }
        public ReactiveCommand<string, Unit> CommandLogError { get; set; }
        public ReactiveCommand<string, Unit> CommandLogInformation { get; set; }
        public ReactiveCommand<string, Unit> CommandLogWarning { get; set; }

        #endregion commands with parameter

        #region commands with undo-redo

        public Command<ConnectorViewModel, ConnectorViewModel> CommandAddConnectorWithConnect { get; set; }
        public Command<Point, List<NodeViewModel>> CommandFullMoveAllNode { get; set; }
        public Command<Point, List<NodeViewModel>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<ExternalNode, NodeViewModel> CommandAddNodeWithUndoRedo { get; set; }
        public Command<List<NodeViewModel>, ElementsForDelete> CommandDeleteSelectedNodes { get; set; }
        public Command<List<ConnectorViewModel>, List<(int index, ConnectorViewModel element)>> CommandDeleteSelectedConnectors { get; set; }
        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }


        public Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)> CommandChangeNodeName { get; set; }
        public Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)> CommandChangeConnectName { get; set; }

        #endregion commands with undo-redo


        private void SetupCommands()
        {

            CommandRedo = ReactiveCommand.Create(ICommandWithUndoRedo.Redo);
            CommandUndo = ReactiveCommand.Create(ICommandWithUndoRedo.Undo);
            CommandSelectAll = ReactiveCommand.Create(SelectedAll);
            CommandUnSelectAll = ReactiveCommand.Create(UnSelectedAll);
            CommandSelectorIntersect = ReactiveCommand.Create(SelectNodes);
            CommandCutterIntersect = ReactiveCommand.Create(SelectConnects);
            CommandZoomIn = ReactiveCommand.Create(() => { ZoomIn(); });
            CommandZoomOut = ReactiveCommand.Create(() => { ZoomOut(); });
            CommandZoomOriginalSize = ReactiveCommand.Create(() => { ZoomOriginalSize(); });
            CommandCollapseUpAll = ReactiveCommand.Create(CollapseUpAll);
            CommandExpandDownAll = ReactiveCommand.Create(ExpandDownAll);
            CommandCollapseUpSelected = ReactiveCommand.Create(CollapseUpSelected);
            CommandExpandDownSelected = ReactiveCommand.Create(ExpandDownSelected);
            CommandErrorListUpdate = ReactiveCommand.Create(ErrosUpdaate);
            CommandExportToPNG = ReactiveCommand.Create(ExportToPNG);
            CommandExportToJPEG = ReactiveCommand.Create(ExportToJPEG);

            CommandNew = ReactiveCommand.Create(New);
            CommandOpen = ReactiveCommand.Create(Open);
            CommandSave = ReactiveCommand.Create(Save);
            CommandSaveAs = ReactiveCommand.Create(SaveAs);
            CommandExit = ReactiveCommand.Create(Exit);
            CommandChangeTheme = ReactiveCommand.Create(ChangeTheme);


            CommandValidateNodeName = ReactiveCommand.Create<(NodeViewModel objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = ReactiveCommand.Create<(ConnectorViewModel objectForValidate, string newValue)>(ValidateConnectName);
            CommandAddConnect = ReactiveCommand.Create<ConnectViewModel>(AddConnect);
            CommandDeleteConnect = ReactiveCommand.Create<ConnectViewModel>(DeleteConnect);
            CommandZoom = ReactiveCommand.Create<(Point point, double delta)>(Zoom);
            CommandLogDebug = ReactiveCommand.Create<string>((message) => LogDebug(message));
            CommandLogError = ReactiveCommand.Create<string>((message) => LogError(message));
            CommandLogInformation = ReactiveCommand.Create<string>((message) => LogInformation(message));
            CommandLogWarning = ReactiveCommand.Create<string>((message) => LogWarning(message));
            CommandSelect = ReactiveCommand.Create<Point>(StartSelect);
            CommandCut = ReactiveCommand.Create<Point>(StartCut);
            CommandAddDraggedConnect = ReactiveCommand.Create<ConnectorViewModel>(AddDraggedConnect);
            CommandDeleteDraggedConnect = ReactiveCommand.Create(DeleteDraggedConnect);


            CommandPartMoveAllNode = ReactiveCommand.Create<Point>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = ReactiveCommand.Create<Point>(PartMoveAllSelectedNode);
            CommandAlignLeft = ReactiveCommand.Create<Point>(AlignLeft);
            CommandAlignRight = ReactiveCommand.Create<Point>(AlignRight);
            CommandAlignCenter = ReactiveCommand.Create<Point>(AlignCenter);


            CommandFullMoveAllNode = new Command<Point, List<NodeViewModel>>(FullMoveAllNode, UnFullMoveAllNode, NotSaved);
            CommandFullMoveAllSelectedNode = new Command<Point, List<NodeViewModel>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddConnectorWithConnect = new Command<ConnectorViewModel, ConnectorViewModel>(AddConnectorWithConnect, DeleteConnectorWithConnect, NotSaved);
            CommandAddNodeWithUndoRedo = new Command<ExternalNode, NodeViewModel>(AddNodeWithUndoRedo, DeleteNodetWithUndoRedo, NotSaved);
            CommandDeleteSelectedNodes = new Command<List<NodeViewModel>, ElementsForDelete>(DeleteSelectedNodes, UnDeleteSelectedNodes, NotSaved);
            CommandDeleteSelectedConnectors = new Command<List<ConnectorViewModel>, List<(int index, ConnectorViewModel connector)>>(DeleteSelectedConnectors, UnDeleteSelectedConnectors, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);
            CommandChangeNodeName = new Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)>(ChangeNodeName, UnChangeNodeName);
            CommandChangeConnectName = new Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)>(ChangeConnectName, UnChangeConnectName);

            NodeClipboard = new List<NodeViewModel>();
            TransitionClipboard = new List<ConnectorViewModel>();
            GroupList = new List<GroupModel>();

            CommandCopy = ReactiveCommand.Create(CopyToClipboard);
            CommandPaste = ReactiveCommand.Create(Paste);
            CommandEditSelected = ReactiveCommand.Create(EditSelected);
            CommandCopyMultiple = ReactiveCommand.Create(CopyMultiple);
            CommandGroup = ReactiveCommand.Create(Group);
            CommandUngroup = ReactiveCommand.Create(Ungroup);
            CommandZincirTopolojiOlustur = ReactiveCommand.Create(ZincirTopolojiOlustur);
            CommandHalkaTopolojiOlustur = ReactiveCommand.Create(HalkaTopolojiOlustur);
            CommandYildizTopolojiOlustur = ReactiveCommand.Create(YildizTopolojiOlustur);

            NotSavedSubscrube();
        }

        private void NotSaved()
        {
            ItSaved = false;
        }
        private void NotSavedSubscrube()
        {
            CommandRedo.Subscribe(_ => NotSaved());
            CommandUndo.Subscribe(_ => NotSaved());
            CommandAddConnect.Subscribe(_ => NotSaved());
            CommandDeleteConnect.Subscribe(_ => NotSaved());
        }
        private void SelectedAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.Selected = true;
            }
        }
        private void CollapseUpAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.IsCollapse = false;
            }
        }
        private void ErrosUpdaate()
        {
            Messages.RemoveMany(Messages.Where(x => x.TypeMessage == DisplayMessageType || DisplayMessageType == TypeMessage.All));
        }
        private void ChangeTheme()
        {
            //if (Theme == Themes.Dark)
            //{
            //    SetTheme(Themes.Light);
            //}
            //else if (Theme == Themes.Light)
            //{
            //    SetTheme(Themes.Dark);
            //}

        }
        //private void SetTheme(Themes theme)
        //{
        //    var configuration = Locator.Current.GetService<IConfiguration>();
        //    configuration.GetSection("Appearance:Theme").Set(theme);
        //    Application.Current.Resources.Clear();
        //    var uri = new Uri(themesPaths[theme], UriKind.RelativeOrAbsolute);
        //    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
        //    Application.Current.Resources.MergedDictionaries.Add(resourceDict);       
        //    LoadIcons();
        //    Theme = theme;
        //}

        private void ZincirTopolojiOlustur()
        {
            List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            bool hepsiAgAnahtariMi = true;
            int omurgaVeyaToplamaSayisi = 0;
            Guid omurgaToplamaUniqueId = Guid.Empty;

            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                if (node.TypeId != (int)TipEnum.AgAnahtari)
                {
                    hepsiAgAnahtariMi = false;
                    break;
                }
                else
                {
                    using (AYPContext context = new AYPContext())
                    {
                        IAgAnahtariService service = new AgAnahtariService(context);
                        var agAnahtari = service.GetAgAnahtariById(node.Id);

                        if (agAnahtari.AgAnahtariTur.Ad == "Omurga" || agAnahtari.AgAnahtariTur.Ad == "Toplama")
                        {
                            omurgaVeyaToplamaSayisi++;
                            omurgaToplamaUniqueId = node.UniqueId;
                        }
                    }
                }
            }

            if (!hepsiAgAnahtariMi)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Zincir topoloji sadece ağ anahtarları arasında oluşturulabilir.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                if (omurgaVeyaToplamaSayisi != 1)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen sadece 1 omurga veya toplama türünde ağ anahtarı seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    AYP.Validations.Validation validation = new AYP.Validations.Validation();
                    var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();

                    foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != omurgaToplamaUniqueId))
                    {
                        var temp = new List<ConnectViewModel>();
                        foreach (var selectedNodeInner in selectedNodes)
                        {
                            bool connectOlusturulabilirMi = false;
                            if (selectedNode != selectedNodeInner)
                            {
                                foreach (var output in selectedNode.Transitions.Items)
                                {
                                    if (output.Connect == null)
                                    {
                                        foreach (var input in selectedNodeInner.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
                                        {
                                            if (validation.ToConnectorValidasyonForTopoloji(input))
                                            {
                                                bool isValid = validation.FizikselOrtamValidasyonForTopoloji(this, output, input);

                                                if (isValid)
                                                {
                                                    isValid = validation.KapasiteValidasyonForTopoloji(this, output, input);
                                                    if (isValid)
                                                    {
                                                        ConnectViewModel connect = new ConnectViewModel(this, output);
                                                        connect.ToConnector = input;
                                                        temp.Add(connect);
                                                        output.Connect = null;
                                                        input.Connect = null;

                                                        connectOlusturulabilirMi = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (connectOlusturulabilirMi)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        list.Add(new KeyValuePair<NodeViewModel, List<ConnectViewModel>>(selectedNode, temp));
                    }

                    var zincir = new List<NodeViewModel>();
                    foreach (var selectedNode in selectedNodes)
                    {
                        zincir = ZincirOlusturRecursive(selectedNode, list, zincir, omurgaToplamaUniqueId);

                        if (zincir.Count() == selectedNodes.Count() && zincir.Last().UniqueId == omurgaToplamaUniqueId)
                        {
                            break;
                        }
                        else
                        {
                            zincir.Clear();
                        }
                    }

                    if (zincir.Count > 0)
                    {
                        int count = 1;
                        Point position;
                        foreach (var item in zincir)
                        {
                            if (count > 1)
                            {
                                item.Point1 = position.Addition(250, 0);
                            }
                            position = item.Point1;

                            if (count != zincir.Count)
                            {
                                var nextNode = zincir[count];

                                var connects = list.Where(x => x.Key == item).Select(s => s.Value).FirstOrDefault();
                                var connect = connects.Where(k => k.ToConnector.Node == nextNode).FirstOrDefault();
                                connect.FromConnector.Connect = connect;
                                CommandAddConnect.ExecuteWithSubscribe(connect);
                            }
                            count++;
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Seçtiğiniz ağ anahtarları için ağ arayüzü uyumsuzluğundan dolayı topoloji oluşturulamamıştır.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
            }
        }

        private void HalkaTopolojiOlustur()
        {
            List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            bool hepsiAgAnahtariMi = true;
            int omurgaVeyaToplamaSayisi = 0;
            Guid omurgaToplamaUniqueId = Guid.Empty;

            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                if (node.TypeId != (int)TipEnum.AgAnahtari)
                {
                    hepsiAgAnahtariMi = false;
                    break;
                }
                else
                {
                    using (AYPContext context = new AYPContext())
                    {
                        IAgAnahtariService service = new AgAnahtariService(context);
                        var agAnahtari = service.GetAgAnahtariById(node.Id);

                        if (agAnahtari.AgAnahtariTur.Ad == "Omurga" || agAnahtari.AgAnahtariTur.Ad == "Toplama")
                        {
                            omurgaVeyaToplamaSayisi++;
                            omurgaToplamaUniqueId = node.UniqueId;
                        }
                    }
                }
            }

            if (!hepsiAgAnahtariMi)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Zincir topoloji sadece ağ anahtarları arasında oluşturulabilir.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                if (omurgaVeyaToplamaSayisi != 1)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen sadece 1 omurga veya toplama türünde ağ anahtarı seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    AYP.Validations.Validation validation = new AYP.Validations.Validation();
                    var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();

                    foreach (var selectedNode in selectedNodes)
                    {
                        var temp = new List<ConnectViewModel>();
                        foreach (var selectedNodeInner in selectedNodes)
                        {
                            bool connectOlusturulabilirMi = false;
                            if (selectedNode != selectedNodeInner)
                            {
                                foreach (var output in selectedNode.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
                                {
                                    if (output.Connect == null)
                                    {
                                        foreach (var input in selectedNodeInner.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
                                        {
                                            if (validation.ToConnectorValidasyonForTopoloji(input))
                                            {
                                                bool isValid = validation.FizikselOrtamValidasyonForTopoloji(this, output, input);

                                                if (isValid)
                                                {
                                                    isValid = validation.KapasiteValidasyonForTopoloji(this, output, input);
                                                    if (isValid)
                                                    {
                                                        ConnectViewModel connect = new ConnectViewModel(this, output);
                                                        connect.ToConnector = input;
                                                        temp.Add(connect);
                                                        output.Connect = null;
                                                        input.Connect = null;

                                                        connectOlusturulabilirMi = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (connectOlusturulabilirMi)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        list.Add(new KeyValuePair<NodeViewModel, List<ConnectViewModel>>(selectedNode, temp));
                    }

                    var zincir = new List<NodeViewModel>();
                    foreach (var selectedNode in selectedNodes)
                    {
                        zincir = ZincirOlusturRecursive(selectedNode, list, zincir, omurgaToplamaUniqueId);

                        if (zincir.Count() == selectedNodes.Count() && zincir.Last().UniqueId == omurgaToplamaUniqueId)
                        {
                            var sonElemaninBaglanabildikleri = list.Where(x => x.Key == zincir.Last()).Select(s => s.Value).FirstOrDefault();

                            if (sonElemaninBaglanabildikleri != null && sonElemaninBaglanabildikleri.Count() > 0)
                            {
                                var sonElemanIlkElemanaBaglanabilirMi = sonElemaninBaglanabildikleri.Where(x => x.ToConnector.Node == zincir.First()).Any();

                                if (sonElemanIlkElemanaBaglanabilirMi)
                                {
                                    break;
                                }
                                else
                                {
                                    zincir.Clear();
                                }
                            }
                            else
                            {
                                zincir.Clear();
                            }
                        }
                        else
                        {
                            zincir.Clear();
                        }
                    }

                    if (zincir.Count > 0)
                    {
                        int count = 1;
                        Point position;
                        foreach (var item in zincir)
                        {
                            if (count > 1)
                            {
                                item.Point1 = position.Addition(250, 0);
                            }
                            position = item.Point1;

                            if (count != zincir.Count)
                            {
                                var nextNode = zincir[count];

                                var connects = list.Where(x => x.Key == item).Select(s => s.Value).FirstOrDefault();
                                var connect = connects.Where(k => k.ToConnector.Node == nextNode).FirstOrDefault();
                                connect.FromConnector.Connect = connect;
                                CommandAddConnect.ExecuteWithSubscribe(connect);
                            }
                            else
                            {
                                var nextNode = zincir[0];

                                var connects = list.Where(x => x.Key == item).Select(s => s.Value).FirstOrDefault();
                                var connect = connects.Where(k => k.ToConnector.Node == nextNode).FirstOrDefault();
                                connect.FromConnector.Connect = connect;
                                CommandAddConnect.ExecuteWithSubscribe(connect);
                            }

                            count++;
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Seçtiğiniz ağ anahtarları için ağ arayüzü uyumsuzluğundan dolayı topoloji oluşturulamamıştır.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
            }
        }

        private List<NodeViewModel> ZincirOlusturRecursive(NodeViewModel node, List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>> list, List<NodeViewModel> zincir, Guid omurgaToplamaUniqueId)
        {
            if (node.UniqueId != omurgaToplamaUniqueId)
            {
                var temp = list.Where(x => x.Key == node).Select(s => s.Value).FirstOrDefault();
                zincir.Add(node);

                if (temp != null)
                {
                    foreach (var connect in temp)
                    {
                        if (!zincir.Contains(connect.ToConnector.Node))
                        {
                            zincir = ZincirOlusturRecursive(connect.ToConnector.Node, list, zincir, omurgaToplamaUniqueId);

                            if (zincir.Count() == Nodes.Items.Where(x => x.Selected).Count() && zincir.Last().UniqueId == omurgaToplamaUniqueId)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (zincir.Count() + 1 == Nodes.Items.Where(x => x.Selected).Count())
                {
                    zincir.Add(node);
                }
            }

            return zincir;
        }

        private void YildizTopolojiOlustur()
        {
            List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            bool hepsiAgAnahtariMi = true;
            int omurgaVeyaToplamaSayisi = 0;
            Guid omurgaToplamaUniqueId = Guid.Empty;

            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                if (node.TypeId != (int)TipEnum.AgAnahtari)
                {
                    hepsiAgAnahtariMi = false;
                    break;
                }
                else
                {
                    using (AYPContext context = new AYPContext())
                    {
                        IAgAnahtariService service = new AgAnahtariService(context);
                        var agAnahtari = service.GetAgAnahtariById(node.Id);

                        if (agAnahtari.AgAnahtariTur.Ad == "Omurga" || agAnahtari.AgAnahtariTur.Ad == "Toplama")
                        {
                            omurgaVeyaToplamaSayisi++;
                            omurgaToplamaUniqueId = node.UniqueId;
                        }
                    }
                }
            }

            if (!hepsiAgAnahtariMi)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Zincir topoloji sadece ağ anahtarları arasında oluşturulabilir.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                if (omurgaVeyaToplamaSayisi != 1)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen sadece 1 omurga veya toplama türünde ağ anahtarı seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    AYP.Validations.Validation validation = new AYP.Validations.Validation();
                    var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();
                    var list2 = new List<KeyValuePair<NodeViewModel, List<ConnectorViewModel>>>();

                    foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != omurgaToplamaUniqueId))
                    {
                        var temp = new List<ConnectViewModel>();
                        var temp2 = new List<ConnectorViewModel>();

                        foreach (var output in selectedNode.Transitions.Items)
                        {
                            if (output.Connect == null)
                            {
                                foreach (var input in selectedNodes.Where(x => x.UniqueId == omurgaToplamaUniqueId).First().InputList)
                                {
                                    if (validation.ToConnectorValidasyonForTopoloji(input))
                                    {
                                        bool isValid = validation.FizikselOrtamValidasyonForTopoloji(this, output, input);

                                        if (isValid)
                                        {
                                            isValid = validation.KapasiteValidasyonForTopoloji(this, output, input);
                                            if (isValid)
                                            {
                                                ConnectViewModel connect = new ConnectViewModel(this, output);
                                                connect.ToConnector = input;
                                                temp.Add(connect);
                                                temp2.Add(connect.ToConnector);
                                                output.Connect = null;
                                                input.Connect = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (temp != null && temp.Count() > 0)
                        {
                            list.Add(new KeyValuePair<NodeViewModel, List<ConnectViewModel>>(selectedNode, temp));
                            list2.Add(new KeyValuePair<NodeViewModel, List<ConnectorViewModel>>(selectedNode, temp2));
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Seçtiğiniz ağ anahtarları için ağ arayüzü uyumsuzluğundan dolayı topoloji oluşturulamamıştır.";
                            nfp.Owner = this.MainWindow;
                            nfp.Show();

                            list.Clear();
                            list2.Clear();
                            break;
                        }
                    }

                    if (list.Count() > 0)
                    {
                        List<ConnectorViewModel> result = new List<ConnectorViewModel>();
                        result = YildizRecursive(list2, 0, result);

                        if (result.Count > 0)
                        {
                            int count = 0;
                            foreach (var item in list)
                            {
                                var connect = item.Value.Where(x => x.ToConnector == result[count]).FirstOrDefault();
                                connect.FromConnector.Connect = connect;
                                CommandAddConnect.ExecuteWithSubscribe(connect);

                                count++;
                            }
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Seçtiğiniz ağ anahtarları için ağ arayüzü uyumsuzluğundan dolayı topoloji oluşturulamamıştır.";
                            nfp.Owner = this.MainWindow;
                            nfp.Show();
                        }
                    }
                }
            }
        }

        private List<ConnectorViewModel> YildizRecursive(List<KeyValuePair<NodeViewModel, List<ConnectorViewModel>>> list, int iterationCount, List<ConnectorViewModel> result)
        {
            var temp = list;

            int i = 0;
            foreach (var item in temp)
            {
                if (i == 0)
                {
                    var input = item.Value[iterationCount];
                    result.Add(input);
                    foreach (var item2 in temp)
                    {
                        if (item.Key != item2.Key)
                        {
                            item2.Value.Remove(input);
                        }
                    }
                }
                else
                {
                    foreach (var input in item.Value)
                    {
                        if (!result.Contains(input))
                        {
                            result.Add(input);
                            foreach (var item2 in temp)
                            {
                                if (item.Key != item2.Key)
                                {
                                    item2.Value.Remove(input);
                                }
                            }

                            break;
                        }
                    }
                }

                i++;
            }

            if (result.Count != Nodes.Items.Where(x => x.Selected).Count() - 1)
            {
                result.Clear();

                if (iterationCount != Nodes.Items.Where(x => x.Selected).Count() - 2)
                {
                    iterationCount++;
                    result = YildizRecursive(list, iterationCount, result);
                }
            }


            return result;
        }

        private void CopyToClipboard()
        {
            //NodeClipboard.Clear();
            //TransitionClipboard.Clear();

            //var copiedNodeList = new List<NodeViewModel>();
            //var copiedTransitionList = new List<ConnectorViewModel>();

            //foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            //{
            //    node.Point1.Addition(15, 15);

            //    foreach(var input in node.InputList)
            //    {
            //        input.PositionConnectPoint.Addition(15, 15);
            //    }

            //    foreach (var output in node.OutputList)
            //    {
            //        output.PositionConnectPoint.Addition(15, 15);
            //    }

            //    foreach (var transition in node.Transitions.Items)
            //    {
            //        transition.PositionConnectPoint.Addition(15, 15);
            //    }

            //    copiedNodeList.Add(node);
            //}

            //foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            //{
            //    foreach(var transition in node.Transitions.Items.Where(x => x.Selected))
            //    {
            //        copiedTransitionList.Add(transition);
            //    }
            //}

            //NodeClipboard.AddRange(copiedNodeList);
            //TransitionClipboard.AddRange(copiedTransitionList);
        }

        private void Paste()
        {
            //var temp = new List<NodeViewModel>();

            //foreach (var node in NodeClipboard)
            //{
            //    var nodePoint = new Point(node.Point1.X + 15, node.Point1.Y + 15);
            //    if (node.TypeId == (int)TipEnum.UcBirim)
            //    {
            //        UcBirimCount++;
            //    }
            //    else if (node.TypeId == (int)TipEnum.AgAnahtari)
            //    {
            //        AgAnahtariCount++;
            //    }
            //    else if (node.TypeId == (int)TipEnum.GucUretici)
            //    {
            //        GucUreticiCount++;
            //    }

            //    var newNode = new NodeViewModel(this, GetNameForNewNode(node.TypeId), Guid.NewGuid(), nodePoint, node.Id, node.TypeId, node.AgArayuzuList, node.GucArayuzuList, node.InputList, node.GucInputList, node.OutputList);
            //    newNode.SetPositionsForCopyPaste();
            //    Nodes.Add(newNode);
            //    LogDebug("Node with name \"{0}\" was copied", GetNameForNewNode(node.TypeId));
            //    AddToProjectHierarchy(newNode);
            //    temp.Add(newNode);
            //}


            //foreach (var transition in TransitionClipboard)
            //{
            //    if (transition.Connect != null)
            //    {
            //        ConnectorViewModel fromConnector = null;
            //        ConnectorViewModel toConnector = null;

            //        foreach (var node in temp)
            //        {
            //            fromConnector = node.Transitions.Items.Where(x => x.UniqueId == transition.Connect.FromConnector.UniqueId).FirstOrDefault();
            //            if (fromConnector != null)
            //            {
            //                break;
            //            }
            //        }

            //        foreach (var node in temp)
            //        {
            //            toConnector = node.InputList.Where(x => x.UniqueId == transition.Connect.ToConnector.UniqueId).FirstOrDefault();
            //            if (toConnector != null)
            //            {
            //                break;
            //            }
            //        }

            //        if (toConnector == null)
            //        {
            //            foreach (var node in temp)
            //            {
            //                toConnector = node.GucInputList.Where(x => x.UniqueId == transition.Connect.ToConnector.UniqueId).FirstOrDefault();
            //                if (toConnector != null)
            //                {
            //                    break;
            //                }
            //            }
            //        }

            //        var newConnect = new ConnectViewModel(this, fromConnector);
            //        newConnect.ToConnector = toConnector;
            //        newConnect.Uzunluk = transition.Connect.Uzunluk;
            //        CommandAddConnect.ExecuteWithSubscribe(newConnect);
            //    }
            //}
        }

        private void Group()
        {
            var nodeList = new List<NodeViewModel>();
            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                nodeList.Add(node);
            }

            var transitionList = new List<ConnectorViewModel>();
            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                foreach (var transition in node.Transitions.Items.Where(x => x.Selected))
                {
                    transitionList.Add(transition);
                }
            }

            if (nodeList.Count() > 1)
            {
                GroupModel model = new GroupModel();
                model.UniqueId = Guid.NewGuid();
                model.NodeList = nodeList;
                model.TransitionList = transitionList;

                model.AgArayuzuList = new List<AgArayuzu>();
                AgArayuzu temp = new AgArayuzu();
                temp.Adi = "Grup Girdi";
                temp.KullanimAmaciId = (int)KullanimAmaciEnum.Girdi;
                temp.Port = "Port 1";
                temp.TipId = (int)TipEnum.AgAnahtariAgArayuzu;
                model.AgArayuzuList.Add(temp);

                temp = new AgArayuzu();
                temp.Adi = "Grup Çıktı";
                temp.KullanimAmaciId = (int)KullanimAmaciEnum.Cikti;
                temp.Port = "Port 1";
                temp.TipId = (int)TipEnum.AgAnahtariAgArayuzu;
                model.AgArayuzuList.Add(temp);

                GroupList.Add(model);

                CommandDeleteSelectedNodes.Execute();
                CommandDeleteSelectedConnectors.Execute();

                NodeViewModel newNode = new NodeViewModel(this, "Grup " + GroupList.Count, model.UniqueId, new Point(), 0, 9, model.AgArayuzuList, new List<GucArayuzu>());
                Nodes.Add(newNode);
            }
        }

        private void Ungroup()
        {
            var nodeList = new List<NodeViewModel>();
            foreach (var node in this.Nodes.Items.Where(x => x.Selected))
            {
                nodeList.Add(node);
            }

            foreach (var node in nodeList)
            {
                var group = GroupList.Where(g => g.UniqueId == node.UniqueId).FirstOrDefault();

                if (group != null)
                {
                    Nodes.Remove(node);

                    foreach (var groupedNode in group.NodeList)
                    {
                        var temp = groupedNode.Transitions.Items;
                        groupedNode.Transitions.Clear();

                        foreach (var transition in temp)
                        {
                            groupedNode.Transitions.Add(transition);

                            if (transition.Connect != null)
                            {
                                groupedNode.NodesCanvas.CommandAddConnect.ExecuteWithSubscribe(transition.Connect);
                            }
                        }
                        Nodes.Add(groupedNode);

                    }
                }
            }
        }

        private void EditSelected()
        {
            NotificationManager notificationManager = new NotificationManager();
            bool sameId = true;
            int typeId;

            if (Nodes.Items.Where(x => x.Selected).Count() < 1)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, En Az 1 Cihaz Seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    foreach (var nodeNext in this.Nodes.Items.Where(x => x.Selected))
                    {
                        if (!node.TypeId.Equals(nodeNext.TypeId))
                        {
                            sameId = false;
                        }
                    }
                }
                if (sameId)
                {
                    typeId = Nodes.Items.Where(x => x.Selected).FirstOrDefault().TypeId;
                    var list = Nodes.Items.Where(x => x.Selected).Select(s => s.Id).Distinct().ToList();
                    EditSelectedPopupWindow es = new EditSelectedPopupWindow(typeId, list);
                    es.Owner = this.MainWindow;
                    es.ShowDialog();
                }
                else if (!sameId)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen, aynı tür cihazlar seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
        }

        private void LoadIcons()
        {
            string path = @"Icons\Icons.xaml";
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
        private void CollapseUpSelected()
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownSelected()
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
            {
                node.IsCollapse = false;
            }
        }
        private void UnSelectedAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.Selected = false;
                node.CommandUnSelectedAllConnectors.ExecuteWithSubscribe();
            }
        }
        private string GetNameForNewNode(int typeId)
        {
            string name = "";
            if (typeId == (int)TipEnum.UcBirim)
            {
                name = "Uç Birim #" + UcBirimCount;
            }
            else if (typeId == (int)TipEnum.AgAnahtari)
            {
                name = "Ağ Anahtarı #" + AgAnahtariCount;
            }
            if (typeId == (int)TipEnum.GucUretici)
            {
                name = "Güç Üretici #" + GucUreticiCount;
            }

            return name;
        }
        private void SelectConnects()
        {
            //Point cutterStartPoint = Cutter.StartPoint.Division(Scale.Value);
            //Point cutterEndPoint = Cutter.EndPoint.Division(Scale.Value);

            Point cutterStartPoint = Cutter.StartPoint;
            Point cutterEndPoint = Cutter.EndPoint;

            var connects = Connects.Where(x => MyUtils.CheckIntersectTwoRectangles(MyUtils.GetStartPointDiagonal(x.StartPoint, x.EndPoint), MyUtils.GetEndPointDiagonal(x.StartPoint, x.EndPoint),
                                               MyUtils.GetStartPointDiagonal(cutterStartPoint, cutterEndPoint), MyUtils.GetEndPointDiagonal(cutterStartPoint, cutterEndPoint)));
            foreach (var connect in Connects)
            {
                connect.FromConnector.Selected = false;
            }

            foreach (var connect in connects)
            {
                connect.FromConnector.Selected = MyUtils.CheckIntersectCubicBezierCurveAndLine(connect.StartPoint, connect.Point1, connect.Point2, connect.EndPoint, cutterStartPoint, cutterEndPoint);
            }

        }
        private void SelectNodes()
        {
            //Point selectorPoint1 = Selector.Point1WithScale.Division(Scale.Value);
            //Point selectorPoint2 = Selector.Point2WithScale.Division(Scale.Value);
            Point selectorPoint1 = Selector.Point1WithScale;
            Point selectorPoint2 = Selector.Point2WithScale;
            foreach (NodeViewModel node in Nodes.Items)
            {
                node.Selected = MyUtils.CheckIntersectTwoRectangles(node.Point1, node.Point2, selectorPoint1, selectorPoint2);
            }
        }
        private void ExportToPNG()
        {
            Dialog.ShowSaveFileDialog("PNG Image (.png)|*.png", SchemeName(), "Export scheme to PNG");
            if (Dialog.Result != DialogResult.Ok)
                return;
            ImageFormat = ImageFormats.PNG;
            ImagePath = Dialog.FileName;
        }
        private void ExportToJPEG()
        {
            Dialog.ShowSaveFileDialog("JPEG Image (.jpeg)|*.jpeg", SchemeName(), "Export scheme to JPEG");
            if (Dialog.Result != DialogResult.Ok)
                return;
            ImageFormat = ImageFormats.JPEG;
            ImagePath = Dialog.FileName;
        }
        private void New()
        {
            if (!WithoutSaving())
                return;
            ClearScheme();
            TransitionClipboard.Clear();
            NodeClipboard.Clear();
            GroupList.Clear();
            this.SetupStartState();
        }
        private void ClearScheme()
        {
            this.Nodes.Clear();
            this.Connects.Clear();
            this.NodesCount = 0;
            this.TransitionsCount = 0;
            this.SchemePath = "";
            this.ImagePath = "";
            WithoutMessages = false;
            this.Messages.Clear();
            ItSaved = true;
            UcBirimCount = 0;
            AgAnahtariCount = 0;
            GucUreticiCount = 0;
            StartState = null;
            DeleteProjectHierarchy();
        }
        private void Open()
        {
            if (!WithoutSaving())
                return;

            Dialog.ShowOpenFileDialog("XML-File | *.xml", SchemeName(), "Import scheme from xml file");
            if (Dialog.Result != DialogResult.Ok)
                return;
            Mouse.OverrideCursor = Cursors.Wait;
            string fileName = Dialog.FileName;
            ClearScheme();
            TransitionClipboard.Clear();
            NodeClipboard.Clear();
            GroupList.Clear();

            WithoutMessages = true;
            XDocument xDocument = XDocument.Load(fileName);
            XElement stateMachineXElement = xDocument.Element("StateMachine");
            if (stateMachineXElement == null)
            {
                Error("not contanins StateMachine");
                return;
            }
            #region setup states/nodes/connectors

            var States = stateMachineXElement.Element("States")?.Elements()?.ToList() ?? new List<XElement>();
            NodeViewModel viewModelNode = null;
            foreach (var state in States)
            {
                viewModelNode = NodeViewModel.FromXElement(this, state, out string errorMesage, NodesExist, stateMachineXElement);
                if (WithError(errorMesage, x => Nodes.Add(x), viewModelNode))
                    return;
            }

            #region setup start state

            var startStateElement = stateMachineXElement.Element("StartState");
            if (startStateElement == null)
            {
                this.SetupStartState();
            }
            else
            {
                var startStateAttribute = startStateElement.Attribute("Name");
                if (startStateAttribute == null)
                {
                    Error("Start state element don't has name attribute");
                    return;
                }
                else
                {
                    string startStateName = startStateAttribute.Value;
                    if (string.IsNullOrEmpty(startStateName))
                    {
                        Error(string.Format("Name attribute of start state is empty.", startStateName));
                        return;
                    }

                    var startNode = this.Nodes.Items.SingleOrDefault(x => x.Name == startStateName);
                    if (startNode == null)
                    {
                        Error(string.Format("Unable to set start state. Node with name \"{0}\" don't exists", startStateName));
                        return;
                    }
                    else
                    {
                        this.SetAsStart(startNode);
                    }
                }

            }

            #endregion  setup start state

            #endregion  setup states/nodes/connectors

            #region setup connects

            var Connects = stateMachineXElement.Element("Connects")?.Elements()?.ToList() ?? new List<XElement>();
            ConnectViewModel viewModelConnect = null;
            foreach (var connect in Connects)
            {
                viewModelConnect = ConnectViewModel.FromXElement(this, connect, out string errorMesage, ConnectsExist, stateMachineXElement);
                if (WithError(errorMesage, x => this.Connects.Add(x), viewModelConnect))
                    return;
            }
            SchemePath = fileName;

            #endregion  setup Transitions/connects

            #region setup Visualization
            XElement Visualization = stateMachineXElement.Element("Visualization");


            if (Visualization != null)
            {
                var visualizationStates = Visualization.Elements()?.ToList();
                if (visualizationStates != null)
                {
                    var nodes = this.Nodes.Items.ToDictionary(x => x.Name, x => x);
                    Point point;
                    bool isCollapse;
                    string name;
                    string pointAttribute;
                    string isCollapseAttribute;
                    foreach (var visualization in visualizationStates)
                    {
                        name = visualization.Attribute("Name")?.Value;
                        if (nodes.TryGetValue(name, out NodeViewModel node))
                        {
                            pointAttribute = visualization.Attribute("Position")?.Value;
                            if (!PointExtensition.TryParseFromString(pointAttribute, out point))
                            {
                                Error(String.Format("Error parse attribute \'position\' for state with name \'{0}\'", name));
                                return;
                            }
                            isCollapseAttribute = visualization.Attribute("IsCollapse")?.Value;
                            if (!bool.TryParse(isCollapseAttribute, out isCollapse))
                            {
                                Error(String.Format("Error parse attribute \'isCollapse\' for state with name \'{0}\'", name));
                                return;
                            }
                            node.Point1 = point;
                            node.IsCollapse = isCollapse;
                        }
                        else
                        {
                            Error(String.Format("Visualization for state with name \'{0}\' that not exist", name));
                            return;
                        }
                    }
                }


                //NodeViewModel nodeViewModel = Nodes.w 
                //var position = node.Attribute("Position")?.Value;
                //Point point = string.IsNullOrEmpty(position) ? new Point() : PointExtensition.StringToPoint(position);
                //var isCollapse = node.Attribute("IsCollapse")?.Value;
                //if (isCollapse != null)
                //    viewModelNode.IsCollapse = bool.Parse(isCollapse);

            }
            #endregion  setup Visualization

            Mouse.OverrideCursor = null;
            WithoutMessages = false;
            LogDebug("Scheme was loaded from file \"{0}\"", SchemePath);

            bool WithError<T>(string errorMessage, Action<T> action, T obj)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (!object.Equals(obj, default(T)))
                        action.Invoke(obj);
                }
                else
                {
                    Error(errorMessage);
                    return true;
                }
                return false;
            }
            void Error(string errorMessage)
            {
                ClearScheme();
                TransitionClipboard.Clear();
                NodeClipboard.Clear();
                GroupList.Clear();

                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Dosya formatı hatalıdır.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
                LogError("File is not valid. " + errorMessage);
                this.SetupStartState();
                Mouse.OverrideCursor = null;
            }
        }
        private void Save()
        {
            if (string.IsNullOrEmpty(SchemePath))
            {
                SaveAs();
            }
            else
            {
                Save(SchemePath);
            }
        }
        private void Exit()
        {
            if (!WithoutSaving())
                return;
            this.NeedExit = true;
        }
        private void SaveAs()
        {
            if (StartState == null)
            {
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Lütfen, en az 1 cihaz ekleyin.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                Dialog.ShowSaveFileDialog("XML-File | *.xml", SchemeName(), "Save scheme as...");
                if (Dialog.Result != DialogResult.Ok)
                    return;

                Save(Dialog.FileName);
            }

        }
        private void Save(string fileName)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            XDocument xDocument = new XDocument();
            XElement stateMachineXElement = new XElement("StateMachine");
            xDocument.Add(stateMachineXElement);
            XElement states = new XElement("States");
            stateMachineXElement.Add(states);
            foreach (var state in Nodes.Items)
            {
                states.Add(state.ToXElement());
            }

            XElement startState = new XElement("StartState");
            stateMachineXElement.Add(startState);
            if (StartState == null)
            {
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Lütfen, en az 1 cihaz ekleyin.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                startState.Add(new XAttribute("Name", StartState.Name));

                XElement inputs = new XElement("Inputs");
                stateMachineXElement.Add(inputs);
                foreach (var input in Nodes.Items.SelectMany(x => x.InputList))
                {
                    inputs.Add(input.ToInputXElement());
                }

                XElement outputs = new XElement("Outputs");
                stateMachineXElement.Add(outputs);
                foreach (var output in Nodes.Items.SelectMany(x => x.OutputList))
                {
                    outputs.Add(output.ToOutputXElement());
                }

                XElement connects = new XElement("Connects");
                stateMachineXElement.Add(connects);
                foreach (var connect in Connects)
                {
                    connects.Add(connect.ToXElement());
                }

                XElement agAkislari = new XElement("AgAkislari");
                stateMachineXElement.Add(agAkislari);
                foreach (var input in Nodes.Items.SelectMany(x => x.InputList))
                {
                    foreach (var agAkis in input.AgAkisList)
                    {
                        agAkislari.Add(agAkis.ToXElement());
                    }
                }

                foreach (var output in Nodes.Items.SelectMany(x => x.Transitions.Items))
                {
                    foreach (var agAkis in output.AgAkisList)
                    {
                        agAkislari.Add(agAkis.ToXElement());
                    }
                }

                XElement visualizationXElement = new XElement("Visualization");
                stateMachineXElement.Add(visualizationXElement);
                foreach (var state in Nodes.Items)
                {
                    visualizationXElement.Add(state.ToVisualizationXElement());
                }

                xDocument.Save(fileName);
                ItSaved = true;
                SchemePath = fileName;
                Mouse.OverrideCursor = null;

                NotificationManager nm = new NotificationManager();
                NotifySuccessPopup nfp = new NotifySuccessPopup();
                nfp.msg.Text = "Proje başarıyla kaydedildi.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
                LogDebug("Scheme was saved as \"{0}\"", SchemePath);
            }
        }

        private void StartSelect(Point point)
        {
            Selector.CommandStartSelect.ExecuteWithSubscribe(point);
        }
        private void StartCut(Point point)
        {
            Cutter.CommandStartCut.ExecuteWithSubscribe(point);
        }
        private void PartMoveAllNode(Point delta)
        {
            foreach (var node in Nodes.Items)
            { node.CommandMove.ExecuteWithSubscribe(delta); }
        }
        private void PartMoveAllSelectedNode(Point delta)
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
            { node.CommandMove.ExecuteWithSubscribe(delta); }
        }

        public void AlignLeft(Point delta)
        {
            if (Nodes.Items.Where(x => x.Selected).Count() > 0)
            {
                var nodeArr = Nodes.Items.Where(x => x.Selected);
                double kucuk = nodeArr.ToList()[0].Point1.X;

                for (int i = 0; i < nodeArr.Count(); i++)
                {
                    if (kucuk > nodeArr.ToList()[i].Point1.X)
                    {
                        kucuk = nodeArr.ToList()[i].Point1.X;
                    }
                }

                foreach (var node in Nodes.Items.Where(x => x.Selected))
                {
                    Point current = node.Point1;
                    node.Point1 = new Point(current.X - kucuk, current.Y);
                }
            }
        }

        public void AlignRight(Point delta)
        {
            if (Nodes.Items.Where(x => x.Selected).Count() > 0)
            {
                var nodeArr = Nodes.Items.Where(x => x.Selected);
                double buyuk = nodeArr.ToList()[0].Point1.X;

                for (int i = 0; i < nodeArr.Count(); i++)
                {
                    if (buyuk < nodeArr.ToList()[i].Point1.X)
                    {
                        buyuk = nodeArr.ToList()[i].Point1.X;
                    }
                }

                foreach (var node in Nodes.Items.Where(x => x.Selected))
                {
                    int midColSize = 0;
                    MainWindow mainWindow = node.NodesCanvas.MainWindow;

                    Point current = node.Point1;

                    if (mainWindow.toggleRight && mainWindow.toggleLeft)
                    {
                        midColSize = 976;

                    }
                    else if (!mainWindow.toggleRight && mainWindow.toggleLeft)
                    {
                        midColSize = 1336;
                    }
                    else if (mainWindow.toggleRight && !mainWindow.toggleLeft)
                    {
                        midColSize = 1336;
                    }

                    else if (!mainWindow.toggleRight && !mainWindow.toggleLeft)
                    {
                        midColSize = 1696;
                    }

                    node.Point1 = new Point(current.X + (midColSize - buyuk), current.Y);

                }
            }
        }

        public void AlignCenter(Point delta)
        {
            if (Nodes.Items.Where(x => x.Selected).Count() > 0)
            {
                foreach (var node in Nodes.Items.Where(x => x.Selected))
                {
                    int midColSize = 0;
                    MainWindow mainWindow = node.NodesCanvas.MainWindow;

                    Point currentY = node.Point1;

                    if (mainWindow.toggleRight && mainWindow.toggleLeft)
                    {
                        midColSize = 510;

                    }
                    else if (!mainWindow.toggleRight && mainWindow.toggleLeft)
                    {
                        midColSize = 740;
                    }
                    else if (mainWindow.toggleRight && !mainWindow.toggleLeft)
                    {
                        midColSize = 740;
                    }

                    else if (!mainWindow.toggleRight && !mainWindow.toggleLeft)
                    {
                        midColSize = 870;
                    }

                    node.Point1 = new Point(midColSize, currentY.Y);

                }
            }
        }

        private void Zoom((Point point, double delta) element)
        {
            ScaleCenter = element.point;
            double scaleValue = RenderTransformMatrix.M11;
            bool DeltaIsZero = (element.delta == 0);
            bool DeltaMax = ((element.delta > 0) && (scaleValue > ScaleMax));
            bool DeltaMin = ((element.delta < 0) && (scaleValue < ScaleMin));
            if (DeltaIsZero || DeltaMax || DeltaMin)
                return;

            double zoom = element.delta > 0 ? ScaleStep : 1 / ScaleStep;
            RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom, element.point.X, element.point.Y);
        }

        //private void Zoom((Point point, double delta) element)
        //{
        //    Zoom(element.delta, element.point);
        //}

        //private void Zoom(double delta, Point? point=null)
        //{
        //    double scaleValue = RenderTransformMatrix.M11;
        //    bool DeltaIsZero = (delta == 0);
        //    bool DeltaMax = ((delta > 0) && (scaleValue > ScaleMax));
        //    bool DeltaMin = ((delta < 0) && (scaleValue < ScaleMin));
        //    if (DeltaIsZero || DeltaMax || DeltaMin)
        //        return;

        //    double zoom = delta > 0 ? ScaleStep : 1 / ScaleStep;
        //    if(point.HasValue)
        //        RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom, point.Value.X, point.Value.Y);
        //    else
        //        RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom);


        //}

        private void ZoomIn()
        {
            //Point point = new Point(RenderTransformMatrix.OffsetX, RenderTransformMatrix.OffsetY);
            Zoom((ScaleCenter, 1));
        }
        private void ZoomOut()
        {
            //Point point = new Point(RenderTransformMatrix.OffsetX, RenderTransformMatrix.OffsetY);
            Zoom((ScaleCenter, -1));
        }
        private void ZoomOriginalSize()
        {
            RenderTransformMatrix = Matrix.Identity;
        }
        private void AddDraggedConnect(ConnectorViewModel fromConnector)
        {
            DraggedConnect = new ConnectViewModel(this, fromConnector);

            AddConnect(DraggedConnect);
        }
        private void DeleteDraggedConnect()
        {
            Connects.Remove(DraggedConnect);
            DraggedConnect.FromConnector.Connect = null;

        }
        private void AddConnect(ConnectViewModel ViewModelConnect)
        {
            Connects.Add(ViewModelConnect);
        }
        private void DeleteConnect(ConnectViewModel ViewModelConnect)
        {
            Connects.Remove(ViewModelConnect);

            ViewModelConnect.FromConnector.AgAkisList = new List<AgAkis>();
            ViewModelConnect.ToConnector.AgAkisList = new List<AgAkis>();
            if (ViewModelConnect.FromConnector.Node.TypeId == (int)TipEnum.GucUretici)
            {
                ViewModelConnect.FromConnector.KalanKapasite += ViewModelConnect.ToConnector.GirdiTukettigiGucMiktari.Value;
            }
            //ViewModelConnect.FromConnector.Node.Transitions.Remove(ViewModelConnect.FromConnector);
            //ViewModelConnect.FromConnector.Node.Transitions.Remove(ViewModelConnect.FromConnector.Node.CurrentConnector);
            ViewModelConnect.FromConnector.Node.CurrentConnector = null;
            //ViewModelConnect.FromConnector.Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
        }
        private void ValidateNodeName((NodeViewModel objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!NodesExist(obj.newValue))
                {
                    LogDebug("Node \"{0}\"  has been renamed . New name is \"{1}\"", obj.objectForValidate.Name, obj.newValue);

                    CommandChangeNodeName.Execute((obj.objectForValidate, obj.newValue));
                }
                else
                {
                    LogError("Name for node doesn't set, because node with name \"{0}\" already exist", obj.newValue);
                }
            }
            else
            {
                LogError("Name for node doesn't set, name off node should not be empty", obj.newValue);
            }
        }
        private void ValidateConnectName((ConnectorViewModel objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!ConnectsExist(obj.newValue))
                {
                    LogDebug("Transition \"{0}\"  has been renamed . New name is \"{1}\"", obj.objectForValidate.Name, obj.newValue);

                    CommandChangeConnectName.Execute((obj.objectForValidate, obj.newValue));
                }
                else
                {
                    LogError("Name for transition doesn't set, because transition with name \"{0}\" already exist", obj.newValue);
                }
            }
            else
            {
                LogError("Name for transition doesn't set, name off transition should not be empty", obj.newValue);
            }
        }


        private List<NodeViewModel> FullMoveAllNode(Point delta, List<NodeViewModel> nodes = null)
        {
            if (nodes == null)
            {
                nodes = Nodes.Items.ToList();
                delta = new Point();
            }
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(delta));
            return nodes;
        }
        private List<NodeViewModel> UnFullMoveAllNode(Point delta, List<NodeViewModel> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<NodeViewModel> FullMoveAllSelectedNode(Point delta, List<NodeViewModel> nodes = null)
        {
            Point myPoint = delta.Copy();
            if (nodes == null)
            {
                nodes = Nodes.Items.Where(x => x.Selected).ToList();
                myPoint = new Point();
            }
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<NodeViewModel> UnFullMoveAllSelectedNode(Point delta, List<NodeViewModel> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }

        private NodeViewModel AddNodeWithUndoRedo(ExternalNode parameter, NodeViewModel result)
        {
            NodeViewModel newNode = result;
            if (result == null)
            {
                if (parameter.Node.TypeId == (int)TipEnum.UcBirim)
                {
                    UcBirimCount++;
                }
                else if (parameter.Node.TypeId == (int)TipEnum.AgAnahtari)
                {
                    AgAnahtariCount++;
                }
                else if (parameter.Node.TypeId == (int)TipEnum.GucUretici)
                {
                    GucUreticiCount++;
                }

                newNode = new NodeViewModel(this, GetNameForNewNode(parameter.Node.TypeId), Guid.NewGuid(), parameter.Point, parameter.Node.Id, parameter.Node.TypeId,
                                parameter.Node.AgArayuzuList, parameter.Node.GucArayuzuList, new List<ConnectorViewModel>(), new List<ConnectorViewModel>(), parameter.Node.VerimlilikOrani,
                                parameter.Node.DahiliGucTuketimDegeri, parameter.Node.Sembol, parameter.Node.StokNo, parameter.Node.Tanim, parameter.Node.UreticiAdi, parameter.Node.UreticiParcaNo,
                                parameter.Node.TurAd);

                if (NodesCount == 0)
                {
                    newNode.CanBeDelete = true;
                    StartState = newNode;
                }
            }
            else
            {
                NodesCount--;
            }
            Nodes.Add(newNode);
            AddToProjectHierarchy(newNode);

            LogDebug("Node with name \"{0}\" was added", newNode.Name);
            return newNode;
        }
        private NodeViewModel DeleteNodetWithUndoRedo(ExternalNode parameter, NodeViewModel result)
        {
            Nodes.Remove(result);
            DeleteWithRedoFromProjectHierarchy(result);
            LogDebug("Node with name \"{0}\" was removed", result.Name);
            return result;
        }

        private ConnectorViewModel AddConnectorWithConnect(ConnectorViewModel parameter, ConnectorViewModel result)
        {
            if (result == null)
            {
                AddChildToProjectHierarchyWithTransitions(parameter);
                return parameter;
            }
            else
                TransitionsCount--;

            result.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((1, result));
            LogDebug("Transition with name \"{0}\" was added", result.Name);
            AddChildToProjectHierarchyWithTransitions(result);
            return result;

            //parameter.Connect.ToConnector.Node.Name
        }
        private ConnectorViewModel DeleteConnectorWithConnect(ConnectorViewModel parameter, ConnectorViewModel result)
        {
            result.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(result);
            LogDebug("Transition with name \"{0}\" was removed", result.Name);
            return parameter;
        }
        private DeleteMode DeleteSelectedElements(DeleteMode parameter, DeleteMode result)
        {
            if (result == DeleteMode.noCorrect)
            {
                bool keyN = Keyboard.IsKeyDown(Key.N);
                bool keyC = Keyboard.IsKeyDown(Key.C);

                if (keyN == keyC)
                {
                    result = DeleteMode.DeleteAllSelected;
                }
                else if (keyN)
                {
                    result = DeleteMode.DeleteNodes;
                }
                else if (keyC)
                {
                    result = DeleteMode.DeleteConnects;
                }

            }

            if ((result == DeleteMode.DeleteConnects) || (result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedConnectors.Execute();
            }
            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedNodes.Execute();
            }

            return result;
        }
        private DeleteMode UnDeleteSelectedElements(DeleteMode parameter, DeleteMode result)
        {
            int count = 0;

            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteConnects))
            {
                count = 1;
            }
            else if (result == DeleteMode.DeleteAllSelected)
            {
                count = 2;
            }

            for (int i = 0; i < count; i++)
            {
                CommandUndo.ExecuteWithSubscribe();
            }
            return result;
        }
        private List<(int index, ConnectorViewModel connector)> DeleteSelectedConnectors(List<ConnectorViewModel> parameter, List<(int index, ConnectorViewModel connector)> result)
        {
            if (result == null)
            {

                result = new List<(int index, ConnectorViewModel element)>();
                foreach (var connector in parameter ?? GetAllConnectors().Where(x => x.Selected))
                {
                    result.Add((connector.Node.GetConnectorIndex(connector), connector));
                }
            }
            foreach (var element in result)
            {
                element.connector.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(element.connector);
                LogDebug("Transition with name \"{0}\" was removed", element.connector.Name);
            }
            return result;
        }
        private List<(int index, ConnectorViewModel connector)> UnDeleteSelectedConnectors(List<ConnectorViewModel> parameter, List<(int index, ConnectorViewModel connector)> result)
        {
            foreach (var element in result)
            {
                TransitionsCount--;
                element.connector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.index, element.connector));
                LogDebug("Transition with name \"{0}\" was added", element.connector.Name);
            }

            return result;
        }
        private (ConnectorViewModel connector, string oldName) ChangeConnectName((ConnectorViewModel connector, string newName) parameter, (ConnectorViewModel connector, string oldName) result)
        {
            string oldName = parameter.connector.Name;
            parameter.connector.Name = parameter.newName;
            return (parameter.connector, oldName);
        }
        private (ConnectorViewModel connector, string oldName) UnChangeConnectName((ConnectorViewModel connector, string newName) parameter, (ConnectorViewModel connector, string oldName) result)
        {
            result.connector.Name = result.oldName;
            return result;
        }


        private (NodeViewModel node, string oldName) ChangeNodeName((NodeViewModel node, string newName) parameter, (NodeViewModel node, string oldName) result)
        {
            string oldName = parameter.node.Name;
            parameter.node.Name = parameter.newName;
            return (parameter.node, oldName);
        }
        private (NodeViewModel node, string oldName) UnChangeNodeName((NodeViewModel node, string newName) parameter, (NodeViewModel node, string oldName) result)
        {
            result.node.Name = result.oldName;
            return result;
        }
        private ElementsForDelete DeleteSelectedNodes(List<NodeViewModel> parameter, ElementsForDelete result)
        {
            if (result == null)
            {
                result = new ElementsForDelete();

                result.NodesToDelete = (parameter?.Where(x => x.CanBeDelete) ?? Nodes.Items.Where(x => x.Selected && x.CanBeDelete)).ToList();
                result.ConnectsToDelete = new List<ConnectViewModel>();
                result.ConnectsToDeleteWithConnectors = new List<(int connectorIndex, ConnectViewModel connect)>();

                foreach (var connect in Connects)
                {
                    if (result.NodesToDelete.Contains(connect.FromConnector.Node))
                    {
                        result.ConnectsToDelete.Add(connect);
                    }
                    else if (result.NodesToDelete.Contains(connect.ToConnector.Node))
                    {
                        result.ConnectsToDeleteWithConnectors.Add((connect.FromConnector.Node.GetConnectorIndex(connect.FromConnector), connect));
                    }
                }
            }
            foreach (var element in result.ConnectsToDeleteWithConnectors)
            {
                element.connect.FromConnector.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(element.connect.FromConnector);
                LogDebug("Transition with name \"{0}\" was removed", element.connect.FromConnector.Name);
            }

            Connects.RemoveMany(result.ConnectsToDelete);
            Nodes.RemoveMany(result.NodesToDelete);
            DeleteFromProjectHierarchy(result.NodesToDelete);
            foreach (var node in result.NodesToDelete)
            {
                LogDebug("Node with name \"{0}\" was removed", node.Name);
            }

            return result;
        }
        private ElementsForDelete UnDeleteSelectedNodes(List<NodeViewModel> parameter, ElementsForDelete result)
        {
            NodesCount -= result.NodesToDelete.Count;
            Nodes.AddRange(result.NodesToDelete);
            foreach (var node in result.NodesToDelete)
            {
                LogDebug("Node with name \"{0}\" was added", node.Name);
            }
            Connects.AddRange(result.ConnectsToDelete);
            result.ConnectsToDeleteWithConnectors.Sort(ElementsForDelete.Sort);
            foreach (var element in result.ConnectsToDeleteWithConnectors)
            {
                TransitionsCount--;
                element.connect.FromConnector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.connectorIndex, element.connect.FromConnector));
                LogDebug("Transition with name \"{0}\" was added", element.connect.FromConnector.Name);
            }

            return result;
        }
        private IEnumerable<ConnectorViewModel> GetAllConnectors()
        {
            return this.Nodes.Items.SelectMany(x => x.Transitions.Items);
        }

        private bool ConnectsExist(string nameConnect)
        {
            return GetAllConnectors().Any(x => x.Name == nameConnect);
        }
        private bool NodesExist(string nameNode)
        {
            return Nodes.Items.Any(x => x.Name == nameNode);
        }
        public class ElementsForDelete
        {
            public List<NodeViewModel> NodesToDelete;
            public List<ConnectViewModel> ConnectsToDelete;
            public List<(int connectorIndex, ConnectViewModel connect)> ConnectsToDeleteWithConnectors;

            public static int Sort((int connectorIndex, ConnectViewModel connect) A, (int connectorIndex, ConnectViewModel connect) B)
            {
                return A.connectorIndex.CompareTo(B.connectorIndex);
            }
        }

        bool WithoutSaving()
        {
            if (!ItSaved)
            {
                NewAppPopupWindow newapp = new NewAppPopupWindow();
                newapp.Owner = MainWindow;
                newapp.ShowDialog();
                //Dialog.ShowMessageBox("Kaydetmeden ��kmak istedi�inize emin misiniz?", "Uyar�", MessageBoxButton.YesNo);
                return newapp.returnvalue;// Dialog.Result == DialogResult.Yes;
            }

            return true;
        }

        #region Project Hierarchy Creations
        private void AddToProjectHierarchy(NodeViewModel hierarchyNode)
        {
            System.Windows.Controls.TreeViewItem newChild = new System.Windows.Controls.TreeViewItem();
            newChild.Header = hierarchyNode.Name;
            MainWindow.ProjectHierarchyAdd(newChild);
        }

        private void DeleteWithRedoFromProjectHierarchy(NodeViewModel hierarchyNode)
        {
            MainWindow.ProjectHierarchyDeleteWithRedo(hierarchyNode);
        }
        private void DeleteFromProjectHierarchy(List<NodeViewModel> NodesToDelete)
        {
            MainWindow.ProjectHierarchyDelete(NodesToDelete);
        }
        private void AddChildToProjectHierarchyWithTransitions(ConnectorViewModel connectorViewModel)
        {
            MainWindow.ProjectHierarchyAddChild(connectorViewModel);
        }
        private void DeleteProjectHierarchy()
        {
            MainWindow.ProjeHiyerarsi.Items.Clear();
        }

        #endregion

        private void CopyMultiple()
        {

            NotificationManager notificationManager = new NotificationManager();


            if (Nodes.Items.Where(x => x.Selected).Count() < 1)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, cihaz seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else if (Nodes.Items.Where(x => x.Selected).Count() > 1)
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, sadece 1 cihaz seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
            else
            {
                MultipleCopyPopupWindow mc = new MultipleCopyPopupWindow(Nodes.Items.Where(x => x.Selected).FirstOrDefault());
                mc.Owner = this.MainWindow;
                mc.ShowDialog();
            }

        }
    }
}
