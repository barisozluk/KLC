using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers;
using AYP.Helpers.Commands;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
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
    public partial class NodesCanvasViewModel
    {
        public List<NodeViewModel> NodeClipboard;
        public List<ConnectViewModel> ConnectClipboard;

        public List<GroupUngroupModel> GroupList;

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
        public ReactiveCommand<Unit, Unit> CommandEditSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCopyMultiple { get; set; }
        public ReactiveCommand<Unit, Unit> CommandShowAgAkis { get; set; }
        public ReactiveCommand<Unit, Unit> CommandArayuzEkle { get; set; }
        public ReactiveCommand<Unit, Unit> CommandRename { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZincirTopolojiOlustur { get; set; }
        public ReactiveCommand<Unit, Unit> CommandHalkaTopolojiOlustur { get; set; }
        public ReactiveCommand<Unit, Unit> CommandYildizTopolojiOlustur { get; set; }
        public ReactiveCommand<Unit, Unit> CommandGucUreticiOtoConnect { get; set; }

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
        public Command<List<ConnectViewModel>, List<(int index, ConnectViewModel element)>> CommandDeleteSelectedConnects { get; set; }

        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }

        public Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)> CommandChangeNodeName { get; set; }
        public Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)> CommandChangeConnectName { get; set; }

        public Command<Unit, List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>>> CommandPaste { get; set; }
        public Command<Unit, KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>>> CommandCopy { get; set; }
        public Command<Unit, KeyValuePair<NodeViewModel, GroupUngroupModel>> CommandGroup { get; set; }
        public Command<Unit, List<KeyValuePair<NodeViewModel, GroupUngroupModel>>> CommandUngroup { get; set; }
        public Command<NodeViewModel, List<ConnectViewModel>> CommandYildizTopolojiOlusturLogic { get; set; }
        public Command<NodeViewModel, List<ConnectViewModel>> CommandHalkaTopolojiOlusturLogic { get; set; }
        public Command<NodeViewModel, List<ConnectViewModel>> CommandZincirTopolojiOlusturLogic { get; set; }
        public Command<Unit, List<ConnectViewModel>> CommandUcBirimOtoConnect { get; set; }
        public Command<NodeViewModel, List<ConnectViewModel>> CommandGucUreticiOtoConnectLogic { get; set; }

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
            CommandDeleteSelectedConnects = new Command<List<ConnectViewModel>, List<(int index, ConnectViewModel connect)>>(DeleteSelectedConnects, UnDeleteSelectedConnects, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);
            CommandChangeNodeName = new Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)>(ChangeNodeName, UnChangeNodeName);
            CommandChangeConnectName = new Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)>(ChangeConnectName, UnChangeConnectName);

            NodeClipboard = new List<NodeViewModel>();
            ConnectClipboard = new List<ConnectViewModel>();
            GroupList = new List<GroupUngroupModel>();

            CommandCopy = new Command<Unit, KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>>>(CopyToClipboard, UnCopyToClipboard, NotSaved);
            CommandPaste = new Command<Unit, List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>>>(Paste, UnPaste, NotSaved);
            CommandGroup = new Command<Unit, KeyValuePair<NodeViewModel, GroupUngroupModel>>(Group, RedoGroup, NotSaved);
            CommandUngroup = new Command<Unit, List<KeyValuePair<NodeViewModel, GroupUngroupModel>>>(Ungroup, RedoUngroup, NotSaved);
            CommandZincirTopolojiOlusturLogic = new Command<NodeViewModel, List<ConnectViewModel>>(ZincirTopolojiOlustur, RedoZincirTopolojiOlustur, NotSaved);
            CommandHalkaTopolojiOlusturLogic = new Command<NodeViewModel, List<ConnectViewModel>>(HalkaTopolojiOlustur, RedoHalkaTopolojiOlustur, NotSaved);
            CommandYildizTopolojiOlusturLogic = new Command<NodeViewModel, List<ConnectViewModel>>(YildizTopolojiOlustur, RedoYildizTopolojiOlustur, NotSaved);
            CommandUcBirimOtoConnect = new Command<Unit, List<ConnectViewModel>>(UcBirimOtoConnectOlustur, RedoUcBirimOtoConnectOlustur, NotSaved);
            CommandGucUreticiOtoConnectLogic = new Command<NodeViewModel, List<ConnectViewModel>>(GucUreticiOtoConnectOlustur, RedoGucUreticiOtoConnectOlustur, NotSaved);

            CommandRename = ReactiveCommand.Create(RenameSelectedNodes);
            CommandEditSelected = ReactiveCommand.Create(EditSelected);
            CommandCopyMultiple = ReactiveCommand.Create(CopyMultiple);
            CommandShowAgAkis = ReactiveCommand.Create(ShowAgAkis);
            CommandZincirTopolojiOlustur = ReactiveCommand.Create(OpenZincirTopolojiGateWayPanel);
            CommandHalkaTopolojiOlustur = ReactiveCommand.Create(OpenHalkaTopolojiGateWayPanel);
            CommandYildizTopolojiOlustur = ReactiveCommand.Create(OpenYildizTopolojiGateWayPanel);
            CommandGucUreticiOtoConnect = ReactiveCommand.Create(OpenGucUreticiSelectPanel);
            CommandArayuzEkle = ReactiveCommand.Create(ArayuzEkle);

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

        private void ArayuzEkle()
        {
            var selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            if (selectedNodes.Count == 1 && selectedNodes.First().TypeId == (int)TipEnum.Group)
            {
                this.MainWindow.IsEnabled = false;
                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                blur.Radius = 2;
                this.MainWindow.Effect = blur;
                GrupArayuzOlusturPopupWindow popup = new GrupArayuzOlusturPopupWindow(selectedNodes.First());
                popup.Owner = this.MainWindow;
                popup.ShowDialog();
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, sadece bir grup seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void ShowAgAkis()
        {
            var selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            if (selectedNodes.Count == 1 && selectedNodes.First().TypeId == (int)TipEnum.UcBirim)
            {
                bool baslangicNoktasiMi = true;
                foreach (var input in selectedNodes.First().InputList)
                {
                    foreach (var connect in Connects)
                    {
                        if (connect.ToConnector == input)
                        {
                            baslangicNoktasiMi = false;
                            break;
                        }
                    }

                    if (!baslangicNoktasiMi)
                    {
                        break;
                    }
                }

                if (baslangicNoktasiMi)
                {
                    baslangicNoktasiMi = false;
                    foreach (var output in selectedNodes.First().Transitions.Items)
                    {
                        if (output.Connect != null)
                        {
                            baslangicNoktasiMi = true;
                            break;
                        }
                    }

                    if (baslangicNoktasiMi)
                    {
                        this.MainWindow.IsEnabled = false;
                        System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                        blur.Radius = 2;
                        this.MainWindow.Effect = blur;
                        AgAkisGosterPopupWindow popup = new AgAkisGosterPopupWindow(selectedNodes.First());
                        popup.Owner = this.MainWindow;
                        popup.ShowDialog();
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Lütfen, akış başlangıç noktası olan bir uç birim seçiniz.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen, akış başlangıç noktası olan bir uç birim seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, sadece bir uç birim seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
        private void RenameSelectedNodes()
        {
            var selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

            if (selectedNodes.Count == 1)
            {
                this.MainWindow.IsEnabled = false;
                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                blur.Radius = 2;
                this.MainWindow.Effect = blur;
                EditSelectedNodeNamePopupWindow popup = new EditSelectedNodeNamePopupWindow(selectedNodes);
                popup.Owner = this.MainWindow;
                popup.ShowDialog();
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, sadece bir cihaz seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void OpenGucUreticiSelectPanel()
        {
            List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
            int gucUreticiSayisi = selectedNodes.Where(s => s.TypeId == (int)TipEnum.GucUretici).Count();
            int gucTuketiciSayisi = selectedNodes.Where(s => s.TypeId == (int)TipEnum.AgAnahtari || s.TypeId == (int)TipEnum.UcBirim).Count();

            if (gucUreticiSayisi > 0)
            {
                if (gucTuketiciSayisi > 0 || gucUreticiSayisi > 1)
                {
                    if (gucUreticiSayisi > 1)
                    {
                        this.MainWindow.IsEnabled = false;
                        System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                        blur.Radius = 2;
                        this.MainWindow.Effect = blur;
                        SelectGucUreticiForOtoConnect popup = new SelectGucUreticiForOtoConnect(selectedNodes.Where(s => s.TypeId == (int)TipEnum.GucUretici).ToList());
                        popup.Owner = this.MainWindow;
                        popup.ShowDialog();
                    }
                    else
                    {
                        CommandGucUreticiOtoConnectLogic.Execute(selectedNodes.Where(s => s.TypeId == (int)TipEnum.GucUretici).FirstOrDefault());
                    }
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen, en az bir güç tüketici seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir güç üretici seçiniz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private List<ConnectViewModel> RedoGucUreticiOtoConnectOlustur(NodeViewModel x, List<ConnectViewModel> res)
        {
            foreach (var item in res)
            {
                if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                        if (output != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.FromConnector == output && x.ToConnector == item.ToConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                        if (input != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.ToConnector == input && x.FromConnector == item.FromConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                item.FromConnector.Connect = null;
                Connects.Remove(item);
            }

            return res;
        }
        public List<ConnectViewModel> GucUreticiOtoConnectOlustur(NodeViewModel gucUretici, List<ConnectViewModel> res)
        {
            if (res == null)
            {
                res = new List<ConnectViewModel>();
                List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

                AYP.Validations.Validation validation = new AYP.Validations.Validation();
                List<ConnectViewModel> connects = new List<ConnectViewModel>();

                foreach (var output in gucUretici.Transitions.Items)
                {
                    foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != gucUretici.UniqueId))
                    {
                        foreach (var input in selectedNode.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu ||
                              x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu))
                        {
                            if (validation.ToConnectorValidasyonForTopoloji(input))
                            {
                                if (validation.GerilimTipiValidasyonForAutoConnect(this, output, input))
                                {
                                    if (validation.GerilimValidasyon(this, output, input))
                                    {
                                        if (validation.GucValidasyon(this, output, input))
                                        {
                                            ConnectViewModel connect = new ConnectViewModel(this, output);
                                            connect.ToConnector = input;
                                            connect.GucMiktari = input.GirdiTukettigiGucMiktari.Value;
                                            AddConnect(connect);
                                            connects.Add(connect);
                                            res.Add(connect);

                                            if (connect.FromConnector.Node.TypeId == (int)TipEnum.Group)
                                            {
                                                var groupNode = GroupList.Where(x => x.UniqueId == connect.FromConnector.Node.UniqueId).FirstOrDefault();

                                                foreach (var node in groupNode.NodeList)
                                                {
                                                    var outp = node.Transitions.Items.Where(x => x.UniqueId == connect.FromConnector.UniqueId).FirstOrDefault();
                                                    if (outp != null)
                                                    {
                                                        ConnectViewModel c = new ConnectViewModel(this, outp);
                                                        c.ToConnector = connect.ToConnector;
                                                        c.AgYuku = connect.AgYuku;
                                                        groupNode.ExternalConnectList.Add(c);
                                                        break;
                                                    }
                                                }
                                            }

                                            if (connect.ToConnector.Node.TypeId == (int)TipEnum.Group)
                                            {
                                                var groupNode = GroupList.Where(x => x.UniqueId == connect.ToConnector.Node.UniqueId).FirstOrDefault();

                                                foreach (var node in groupNode.NodeList)
                                                {
                                                    var inp = node.InputList.Where(x => x.UniqueId == connect.ToConnector.UniqueId).FirstOrDefault();
                                                    if (inp != null)
                                                    {
                                                        ConnectViewModel c = new ConnectViewModel(this, connect.FromConnector);
                                                        c.ToConnector = inp;
                                                        c.AgYuku = connect.AgYuku;
                                                        groupNode.ExternalConnectList.Add(c);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (connects.Count() == 0)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Seçtiğiniz çihazlar için uygun hiçbir güç arayüz bağlantısı yapılamamıştır.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var item in res)
                {
                    if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                            if (output != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, output);
                                c.ToConnector = item.ToConnector;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                            if (input != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, item.FromConnector);
                                c.ToConnector = input;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    CommandAddConnect.ExecuteWithSubscribe(item);
                }
            }

            return res;
        }

        private List<ConnectViewModel> RedoUcBirimOtoConnectOlustur(Unit x, List<ConnectViewModel> res)
        {
            foreach (var item in res)
            {
                if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                        if (output != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.FromConnector == output && x.ToConnector == item.ToConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                        if (input != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.ToConnector == input && x.FromConnector == item.FromConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                item.FromConnector.Connect = null;
                Connects.Remove(item);
            }

            return res;
        }
        private List<ConnectViewModel> UcBirimOtoConnectOlustur(Unit x, List<ConnectViewModel> res)
        {
            if (res == null)
            {
                res = new List<ConnectViewModel>();

                List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();

                int agAnahtariSayisi = selectedNodes.Where(s => s.TypeId == (int)TipEnum.AgAnahtari).Count();
                int ucBirimSayisi = selectedNodes.Where(s => s.TypeId == (int)TipEnum.UcBirim).Count();

                if (ucBirimSayisi + agAnahtariSayisi == selectedNodes.Count)
                {
                    if (agAnahtariSayisi == 1)
                    {
                        Guid gateWayUniqueId = selectedNodes.Where(s => s.TypeId == (int)TipEnum.AgAnahtari).Select(s => s.UniqueId).FirstOrDefault();
                        var gateWayInputs = selectedNodes.Where(x => x.UniqueId == gateWayUniqueId).Select(s => s.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)).FirstOrDefault();
                        var bosInputSayisi = gateWayInputs.Where(x => x.Connect == null).Count();

                        if (bosInputSayisi >= selectedNodes.Where(x => x.UniqueId != gateWayUniqueId).Count())
                        {
                            AYP.Validations.Validation validation = new AYP.Validations.Validation();
                            var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();
                            var list2 = new List<KeyValuePair<NodeViewModel, List<ConnectorViewModel>>>();

                            foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != gateWayUniqueId))
                            {
                                var temp = new List<ConnectViewModel>();
                                var temp2 = new List<ConnectorViewModel>();

                                foreach (var output in selectedNode.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu))
                                {
                                    if (output.Connect == null)
                                    {
                                        foreach (var input in selectedNodes.Where(x => x.UniqueId == gateWayUniqueId).First().InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
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
                                    var a = temp2.Distinct().ToList();
                                    list.Add(new KeyValuePair<NodeViewModel, List<ConnectViewModel>>(selectedNode, temp));
                                    list2.Add(new KeyValuePair<NodeViewModel, List<ConnectorViewModel>>(selectedNode, a));
                                }
                                else
                                {
                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Seçtiğiniz cihazlar için ağ arayüzü uyumsuzluğundan dolayı otomatik bağlantı oluşturulamamıştır.";
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

                                        if (connect.FromConnector.AgAkisList != null && connect.FromConnector.AgAkisList.Count() != 0)
                                        {
                                            connect.ToConnector.AgAkisList = new List<AgAkis>();
                                            connect.ToConnector.AgAkisList.Clear();
                                            foreach (var agAkis in connect.FromConnector.AgAkisList)
                                            {
                                                var agAkisTemp = new AgAkis();
                                                agAkisTemp.Id = Guid.NewGuid();
                                                agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                                agAkisTemp.Yuk = agAkis.Yuk;
                                                agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                                agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                                agAkisTemp.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                                                foreach (var varisNoktasiIdName in agAkis.VarisNoktasiIdNameList)
                                                {
                                                    agAkisTemp.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
                                                }
                                                agAkisTemp.FromNodeUniqueId = connect.FromConnector.Node.UniqueId;

                                                connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                                connect.AgYuku = connect.FromConnector.AgAkisList.Select(x => x.Yuk).Sum();
                                            }
                                        }

                                        CommandAddConnect.ExecuteWithSubscribe(connect);
                                        res.Add(connect);
                                        count++;

                                        if (connect.FromConnector.Node.TypeId == (int)TipEnum.Group)
                                        {
                                            var groupNode = GroupList.Where(x => x.UniqueId == connect.FromConnector.Node.UniqueId).FirstOrDefault();

                                            foreach (var node in groupNode.NodeList)
                                            {
                                                var output = node.Transitions.Items.Where(x => x.UniqueId == connect.FromConnector.UniqueId).FirstOrDefault();
                                                if (output != null)
                                                {
                                                    ConnectViewModel c = new ConnectViewModel(this, output);
                                                    c.ToConnector = connect.ToConnector;
                                                    c.AgYuku = connect.AgYuku;
                                                    groupNode.ExternalConnectList.Add(c);
                                                    break;
                                                }
                                            }
                                        }

                                        if (connect.ToConnector.Node.TypeId == (int)TipEnum.Group)
                                        {
                                            var groupNode = GroupList.Where(x => x.UniqueId == connect.ToConnector.Node.UniqueId).FirstOrDefault();

                                            foreach (var node in groupNode.NodeList)
                                            {
                                                var input = node.InputList.Where(x => x.UniqueId == connect.ToConnector.UniqueId).FirstOrDefault();
                                                if (input != null)
                                                {
                                                    ConnectViewModel c = new ConnectViewModel(this, connect.FromConnector);
                                                    c.ToConnector = input;
                                                    c.AgYuku = connect.AgYuku;
                                                    groupNode.ExternalConnectList.Add(c);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Seçtiğiniz cihazlar için ağ arayüzü uyumsuzluğundan dolayı otomatik bağlantı oluşturulamamıştır.";
                                    nfp.Owner = this.MainWindow;
                                    nfp.Show();
                                }
                            }
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Seçtiğiniz cihazlar için ağ arayüzü uyumsuzluğundan dolayı otomatik bağlantı oluşturulamamıştır.";
                            nfp.Owner = this.MainWindow;
                            nfp.Show();
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Lütfen sadece bir ağ anahtarı seçiniz.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Lütfen sadece bir ağ anahtarı ve bağlantı oluşturacağınız uç birimleri seçiniz.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var item in res)
                {
                    if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                            if (output != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, output);
                                c.ToConnector = item.ToConnector;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                            if (input != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, item.FromConnector);
                                c.ToConnector = input;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    CommandAddConnect.ExecuteWithSubscribe(item);
                }
            }

            return res;
        }

        private void OpenZincirTopolojiGateWayPanel()
        {
            if (this.Nodes.Items.Where(x => x.Selected).Count() > 1)
            {
                bool hepsiAgAnahtariMi = true;

                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    if (node.TypeId != (int)TipEnum.AgAnahtari)
                    {
                        hepsiAgAnahtariMi = false;
                        break;
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
                    List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                    this.MainWindow.IsEnabled = false;
                    System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                    blur.Radius = 2;
                    this.MainWindow.Effect = blur;
                    SelectGateWayForTopology popup = new SelectGateWayForTopology(selectedNodes, (int)TopolojiEnum.Zincir);
                    popup.Owner = this.MainWindow;
                    popup.ShowDialog();
                }
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "En az iki ağ anahtarı seçiniz!";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
        private List<ConnectViewModel> RedoZincirTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            foreach (var item in res)
            {
                item.FromConnector.Connect = null;
                Connects.Remove(item);
            }

            return res;
        }
        private List<ConnectViewModel> ZincirTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            if (res == null)
            {
                res = new List<ConnectViewModel>();

                List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                Guid gateWayUniqueId = gateWay.UniqueId;

                AYP.Validations.Validation validation = new AYP.Validations.Validation();
                var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();

                foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != gateWayUniqueId))
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
                                                }
                                            }
                                        }
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
                    zincir = ZincirOlusturRecursive(selectedNode, list, zincir, gateWayUniqueId);

                    if (zincir.Count() == selectedNodes.Count() && zincir.Last().UniqueId == gateWayUniqueId)
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
                            res.Add(connect);

                            if (connect.FromConnector.AgAkisList != null && connect.FromConnector.AgAkisList.Count() != 0)
                            {
                                connect.ToConnector.AgAkisList = new List<AgAkis>();
                                connect.ToConnector.AgAkisList.Clear();
                                foreach (var agAkis in connect.FromConnector.AgAkisList)
                                {
                                    var agAkisTemp = new AgAkis();
                                    agAkisTemp.Id = Guid.NewGuid();
                                    agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                    agAkisTemp.Yuk = agAkis.Yuk;
                                    agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                    agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                    agAkisTemp.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                                    agAkisTemp.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;
                                    agAkisTemp.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                                    foreach (var varisNoktasiIdName in agAkis.VarisNoktasiIdNameList)
                                    {
                                        agAkisTemp.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
                                    }
                                    agAkisTemp.FromNodeUniqueId = agAkis.FromNodeUniqueId;

                                    connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                    connect.AgYuku = connect.ToConnector.AgAkisList.Select(x => x.Yuk).Sum();
                                }
                            }

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
            else
            {
                foreach (var item in res)
                {
                    CommandAddConnect.ExecuteWithSubscribe(item);
                }
            }

            return res;
        }

        private void OpenHalkaTopolojiGateWayPanel()
        {
            if (this.Nodes.Items.Where(x => x.Selected).Count() > 1)
            {
                bool hepsiAgAnahtariMi = true;

                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    if (node.TypeId != (int)TipEnum.AgAnahtari)
                    {
                        hepsiAgAnahtariMi = false;
                        break;
                    }
                }

                if (!hepsiAgAnahtariMi)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Halka topoloji sadece ağ anahtarları arasında oluşturulabilir.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                    this.MainWindow.IsEnabled = false;
                    System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                    blur.Radius = 2;
                    this.MainWindow.Effect = blur;
                    SelectGateWayForTopology popup = new SelectGateWayForTopology(selectedNodes, (int)TopolojiEnum.Halka);
                    popup.Owner = this.MainWindow;
                    popup.ShowDialog();
                }
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "En az iki ağ anahtarı seçiniz!";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
        private List<ConnectViewModel> RedoHalkaTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            foreach (var item in res)
            {
                item.FromConnector.Connect = null;
                Connects.Remove(item);
            }

            return res;
        }
        public List<ConnectViewModel> HalkaTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            if (res == null)
            {
                res = new List<ConnectViewModel>();

                List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                Guid gateWayUniqueId = gateWay.UniqueId;

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
                                                }
                                            }
                                        }
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
                    zincir = ZincirOlusturRecursive(selectedNode, list, zincir, gateWayUniqueId);

                    if (zincir.Count() > 0 && zincir.First().Transitions.Items.Count() > 1)
                    {
                        if (zincir.Count() == selectedNodes.Count() && zincir.Last().UniqueId == gateWayUniqueId)
                        {
                            var ilkElemaninBaglanabildikleri = list.Where(x => x.Key == zincir.First()).Select(s => s.Value).FirstOrDefault();

                            if (ilkElemaninBaglanabildikleri != null && ilkElemaninBaglanabildikleri.Count() > 0)
                            {
                                var ilkElemaninSonElemanaBaglanabildikleri = ilkElemaninBaglanabildikleri.Where(x => x.ToConnector.Node == zincir.Last()).ToList();

                                if (ilkElemaninSonElemanaBaglanabildikleri.Count() > 0)
                                {
                                    List<ConnectViewModel> tempConnects = new List<ConnectViewModel>();
                                    int count = 1;
                                    foreach (var item in zincir)
                                    {
                                        if (count != zincir.Count)
                                        {
                                            var nextNode = zincir[count];

                                            var connects = list.Where(x => x.Key == item).Select(s => s.Value).FirstOrDefault();
                                            var connect = connects.Where(k => k.ToConnector.Node == nextNode).FirstOrDefault();
                                            if (connect != null)
                                            {
                                                var output = connect.FromConnector;
                                                connect.FromConnector.Connect = connect;
                                                tempConnects.Add(connect);
                                                output.Connect = null;
                                            }
                                        }
                                        count++;
                                    }

                                    bool ilkElemanSonElemanaBaglanabilirMi = false;
                                    foreach(var item in ilkElemaninSonElemanaBaglanabildikleri)
                                    {
                                        if(!(tempConnects.Where(x => x.FromConnector == item.FromConnector).Any() || tempConnects.Where(x => x.ToConnector == item.ToConnector).Any()))
                                        {
                                            ilkElemanSonElemanaBaglanabilirMi = true;
                                            break;
                                        }
                                    }

                                    if (ilkElemanSonElemanaBaglanabilirMi)
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

                            if (connect.FromConnector.AgAkisList != null && connect.FromConnector.AgAkisList.Count() != 0)
                            {
                                connect.ToConnector.AgAkisList = new List<AgAkis>();
                                connect.ToConnector.AgAkisList.Clear();
                                foreach (var agAkis in connect.FromConnector.AgAkisList)
                                {
                                    var agAkisTemp = new AgAkis();
                                    agAkisTemp.Id = Guid.NewGuid();
                                    agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                    agAkisTemp.Yuk = agAkis.Yuk;
                                    agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                    agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                    agAkisTemp.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                                    agAkisTemp.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;
                                    agAkisTemp.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                                    foreach (var varisNoktasiIdName in agAkis.VarisNoktasiIdNameList)
                                    {
                                        agAkisTemp.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
                                    }
                                    agAkisTemp.FromNodeUniqueId = agAkis.FromNodeUniqueId;

                                    connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                    connect.AgYuku = connect.ToConnector.AgAkisList.Select(x => x.Yuk).Sum();
                                }
                            }

                            CommandAddConnect.ExecuteWithSubscribe(connect);
                            res.Add(connect);
                        }
                        else
                        {
                            var nextNode = zincir[0];

                            var connects = list.Where(x => x.Key == nextNode).Select(s => s.Value).FirstOrDefault();
                            var connect = connects.Where(k => k.ToConnector.Node == item && !Connects.Where(x => x.ToConnector == k.ToConnector).Any() && k.FromConnector.Connect == null).FirstOrDefault();
                            connect.FromConnector.Connect = connect;
                            CommandAddConnect.ExecuteWithSubscribe(connect);
                            res.Add(connect);
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
            else
            {
                foreach (var item in res)
                {
                    CommandAddConnect.ExecuteWithSubscribe(item);
                }
            }

            return res;
        }

        private List<NodeViewModel> ZincirOlusturRecursive(NodeViewModel node, List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>> list, List<NodeViewModel> zincir, Guid gateWayUniqueId)
        {
            if (node.UniqueId != gateWayUniqueId)
            {
                var temp = list.Where(x => x.Key == node).Select(s => s.Value).FirstOrDefault();
                zincir.Add(node);

                if (temp != null)
                {
                    foreach (var connect in temp)
                    {
                        if (!zincir.Contains(connect.ToConnector.Node))
                        {
                            zincir = ZincirOlusturRecursive(connect.ToConnector.Node, list, zincir, gateWayUniqueId);

                            if (zincir.Count() == Nodes.Items.Where(x => x.Selected).Count() && zincir.Last().UniqueId == gateWayUniqueId)
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

        private void OpenYildizTopolojiGateWayPanel()
        {
            if (this.Nodes.Items.Where(x => x.Selected).Count() > 1)
            {
                bool hepsiAgAnahtariMi = true;

                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    if (node.TypeId != (int)TipEnum.AgAnahtari && node.TypeId != (int)TipEnum.Group)
                    {
                        hepsiAgAnahtariMi = false;
                        break;
                    }
                    else
                    {
                        if (node.TypeId == (int)TipEnum.Group)
                        {
                            if (node.Transitions.Items != null && node.Transitions.Items.Count() > 0)
                            {
                                hepsiAgAnahtariMi = node.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).Any();
                            }
                            else
                            {
                                hepsiAgAnahtariMi = false;
                            }
                        }
                    }

                }

                if (!hepsiAgAnahtariMi)
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Yıldız topoloji sadece ağ anahtarları arasında oluşturulabilir.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
                else
                {
                    List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                    this.MainWindow.IsEnabled = false;
                    System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                    blur.Radius = 2;
                    this.MainWindow.Effect = blur;
                    SelectGateWayForTopology popup = new SelectGateWayForTopology(selectedNodes, (int)TopolojiEnum.Yildiz);
                    popup.Owner = this.MainWindow;
                    popup.ShowDialog();
                }
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "En az iki ağ anahtarı seçiniz!";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
        private List<ConnectViewModel> RedoYildizTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            foreach (var item in res)
            {
                if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                        if (output != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.FromConnector == output && x.ToConnector == item.ToConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                        if (input != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.ToConnector == input && x.FromConnector == item.FromConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                item.FromConnector.Connect = null;
                Connects.Remove(item);
            }

            return res;
        }
        public List<ConnectViewModel> YildizTopolojiOlustur(NodeViewModel gateWay, List<ConnectViewModel> res)
        {
            if (res == null)
            {
                res = new List<ConnectViewModel>();

                List<NodeViewModel> selectedNodes = this.Nodes.Items.Where(x => x.Selected).ToList();
                Guid gateWayUniqueId = gateWay.UniqueId;

                var gateWayInputs = selectedNodes.Where(x => x.UniqueId == gateWayUniqueId).Select(s => s.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)).FirstOrDefault();
                var bosInputSayisi = gateWayInputs.Where(x => x.Connect == null).Count();

                if (bosInputSayisi >= selectedNodes.Where(x => x.UniqueId != gateWayUniqueId).Count())
                {
                    AYP.Validations.Validation validation = new AYP.Validations.Validation();
                    var list = new List<KeyValuePair<NodeViewModel, List<ConnectViewModel>>>();
                    var list2 = new List<KeyValuePair<NodeViewModel, List<ConnectorViewModel>>>();

                    foreach (var selectedNode in selectedNodes.Where(x => x.UniqueId != gateWayUniqueId))
                    {
                        var temp = new List<ConnectViewModel>();
                        var temp2 = new List<ConnectorViewModel>();

                        foreach (var output in selectedNode.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
                        {
                            if (output.Connect == null)
                            {
                                foreach (var input in selectedNodes.Where(x => x.UniqueId == gateWayUniqueId).First().InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu))
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
                            var a = temp2.Distinct().ToList();
                            list.Add(new KeyValuePair<NodeViewModel, List<ConnectViewModel>>(selectedNode, temp));
                            list2.Add(new KeyValuePair<NodeViewModel, List<ConnectorViewModel>>(selectedNode, a));
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
                                //connect = OtoAgAkisiEkle(connect);

                                if (connect.FromConnector.AgAkisList != null && connect.FromConnector.AgAkisList.Count() != 0)
                                {
                                    connect.ToConnector.AgAkisList = new List<AgAkis>();
                                    connect.ToConnector.AgAkisList.Clear();
                                    foreach (var agAkis in connect.FromConnector.AgAkisList)
                                    {
                                        var agAkisTemp = new AgAkis();
                                        agAkisTemp.Id = Guid.NewGuid();
                                        agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                        agAkisTemp.Yuk = agAkis.Yuk;
                                        agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                        agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                        agAkisTemp.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                                        agAkisTemp.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;
                                        agAkisTemp.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                                        foreach (var varisNoktasiIdName in agAkis.VarisNoktasiIdNameList)
                                        {
                                            agAkisTemp.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
                                        }
                                        agAkisTemp.FromNodeUniqueId = agAkis.FromNodeUniqueId;

                                        connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                        connect.AgYuku = connect.ToConnector.AgAkisList.Select(x => x.Yuk).Sum();
                                    }
                                }

                                CommandAddConnect.ExecuteWithSubscribe(connect);
                                res.Add(connect);

                                if (connect.FromConnector.Node.TypeId == (int)TipEnum.Group)
                                {
                                    var groupNode = GroupList.Where(x => x.UniqueId == connect.FromConnector.Node.UniqueId).FirstOrDefault();

                                    foreach (var node in groupNode.NodeList)
                                    {
                                        var output = node.Transitions.Items.Where(x => x.UniqueId == connect.FromConnector.UniqueId).FirstOrDefault();
                                        if (output != null)
                                        {
                                            ConnectViewModel c = new ConnectViewModel(this, output);
                                            c.ToConnector = connect.ToConnector;
                                            c.AgYuku = connect.AgYuku;
                                            groupNode.ExternalConnectList.Add(c);
                                            break;
                                        }
                                    }
                                }

                                if (connect.ToConnector.Node.TypeId == (int)TipEnum.Group)
                                {
                                    var groupNode = GroupList.Where(x => x.UniqueId == connect.ToConnector.Node.UniqueId).FirstOrDefault();

                                    foreach (var node in groupNode.NodeList)
                                    {
                                        var input = node.InputList.Where(x => x.UniqueId == connect.ToConnector.UniqueId).FirstOrDefault();
                                        if (input != null)
                                        {
                                            ConnectViewModel c = new ConnectViewModel(this, connect.FromConnector);
                                            c.ToConnector = input;
                                            c.AgYuku = connect.AgYuku;
                                            groupNode.ExternalConnectList.Add(c);
                                            break;
                                        }
                                    }
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
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Seçtiğiniz ağ anahtarları için ağ arayüzü uyumsuzluğundan dolayı topoloji oluşturulamamıştır.";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var item in res)
                {
                    if (item.FromConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.FromConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var output = node.Transitions.Items.Where(x => x.UniqueId == item.FromConnector.UniqueId).FirstOrDefault();

                            if (output != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, output);
                                c.ToConnector = item.ToConnector;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    if (item.ToConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var group = GroupList.Where(x => x.UniqueId == item.ToConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in group.NodeList)
                        {
                            var input = node.InputList.Where(x => x.UniqueId == item.ToConnector.UniqueId).FirstOrDefault();

                            if (input != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this, item.FromConnector);
                                c.ToConnector = input;
                                c.AgYuku = item.AgYuku;
                                group.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    CommandAddConnect.ExecuteWithSubscribe(item);
                }
            }

            return res;
        }
        private List<ConnectorViewModel> YildizRecursive(List<KeyValuePair<NodeViewModel, List<ConnectorViewModel>>> list, int index, List<ConnectorViewModel> result)
        {
            int oldIndex = index;
            var values = list[index].Value;

            foreach (var value in values)
            {
                if (!result.Contains(value))
                {
                    result.Add(value);

                    if (index == list.Count() - 1)
                    {
                        if (result.Count != Nodes.Items.Where(x => x.Selected).Count() - 1)
                        {
                            result.Clear();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        index = index + 1;
                        result = YildizRecursive(list, index, result);

                        if (result.Count != Nodes.Items.Where(x => x.Selected).Count() - 1)
                        {
                            result.Clear();
                            index = oldIndex;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>> UnCopyToClipboard(Unit x, KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>> result)
        {
            NodeClipboard.Clear();
            ConnectClipboard.Clear();

            return result;
        }
        private KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>> CopyToClipboard(Unit x, KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>> result)
        {
            NodeClipboard.Clear();
            ConnectClipboard.Clear();

            var copiedNodeList = new List<NodeViewModel>();
            var copiedConnectList = new List<ConnectViewModel>();

            if (result.Key == null && result.Value == null)
            {
                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    copiedNodeList.Add(node);
                }

                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    foreach (var transition in node.Transitions.Items.Where(x => x.Selected))
                    {
                        if (transition.Connect != null)
                        {
                            copiedConnectList.Add(transition.Connect);
                        }
                    }
                }
            }
            else
            {
                copiedConnectList = result.Value;
                copiedNodeList = result.Key;
            }

            result = new KeyValuePair<List<NodeViewModel>, List<ConnectViewModel>>(copiedNodeList, copiedConnectList);
            NodeClipboard.AddRange(copiedNodeList);
            ConnectClipboard.AddRange(copiedConnectList);

            return result;
        }

        private List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>> UnPaste(Unit x, List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>> result)
        {
            var nodes = result.Select(s => s.Key).ToList().Select(s => s.Key).Where(x => x != null);

            foreach (var item in result.Select(s => s.Key))
            {
                Nodes.Remove(item.Key);
                if (item.Key.TypeId == (int)TipEnum.Group)
                {
                    DeleteGroupFromNode(item.Key);
                }
                DeleteWithRedoFromProjectHierarchy(item.Key);
                LogDebug("Node with name \"{0}\" was removed", item.Key.Name);
                NodeCounter();
            }

            foreach (var item in result.Select(s => s.Value))
            {
                Connects.Remove(Connects.Where(c => item.Contains(c)).ToList());
            }

            return result;
        }
        private List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>> Paste(Unit x, List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>> result)
        {
            if (result == null)
            {
                bool isExecutable = true;
                if (NodeClipboard.Where(x => x.TypeId == (int)TipEnum.Group).Count() != NodeClipboard.Count())
                {
                    if (NodeClipboard.Where(x => x.TypeId != (int)TipEnum.Group).Count() != NodeClipboard.Count())
                    {
                        isExecutable = false;
                    }
                }

                if (isExecutable)
                {
                    result = new List<KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>>();
                    var temp = new List<KeyValuePair<NodeViewModel, GroupUngroupModel>>();

                    foreach (var node in NodeClipboard)
                    {
                        var newPoint = node.Point1.Addition(50, 50);
                        var newNode = new NodeViewModel(this, GetNameForNewNode(node.TypeId, node.Name), Guid.NewGuid(), newPoint, node.Id, node.TypeId,
                            node.AgArayuzuList, node.GucArayuzuList, new List<ConnectorViewModel>(), new List<ConnectorViewModel>(), node.VerimlilikOrani,
                            node.DahiliGucTuketimDegeri, node.Sembol, node.StokNo, node.Tanim, node.UreticiAdi, node.UreticiParcaNo, node.TurAd);

                        if (node.TypeId == (int)TipEnum.Group)
                        {
                            var willBeCopiedGroup = GroupList.Where(x => x.UniqueId == node.UniqueId).FirstOrDefault();
                            newNode = CopyPasteGroup(willBeCopiedGroup, newNode);
                        }
                        else
                        {
                            AddToProjectHierarchy(newNode);
                        }

                        Nodes.Add(newNode);
                        LogDebug("Node with name \"{0}\" was copied", newNode.Name);

                        if (newNode.TypeId == (int)TipEnum.Group)
                        {
                            var group = GroupList.Where(x => x.UniqueId == newNode.UniqueId).FirstOrDefault();
                            temp.Add(new KeyValuePair<NodeViewModel, GroupUngroupModel>(newNode, group));
                        }
                        else
                        {
                            temp.Add(new KeyValuePair<NodeViewModel, GroupUngroupModel>(newNode, null));
                        }

                    }

                    foreach (var item in temp)
                    {
                        var connects = new List<ConnectViewModel>();
                        var node = item.Key;
                        foreach (var output in node.Transitions.Items)
                        {
                            var oldConnect = ConnectClipboard
                                                  .Where(x => x.FromConnector.Label == output.Label &&
                                                   x.FromConnector.PositionConnectPoint.Addition((x.FromConnector.Artik * -1) + 50, 50) == output.PositionConnectPoint)
                                                  .FirstOrDefault();

                            if (oldConnect != null)
                            {
                                foreach (var item2 in temp)
                                {
                                    var node2 = item2.Key;
                                    if (node != node2)
                                    {
                                        foreach (var input in node2.InputList)
                                        {
                                            if (input.Label == oldConnect.ToConnector.Label && input.PositionConnectPoint == oldConnect.ToConnector.PositionConnectPoint.Addition(50, 50))
                                            {
                                                var newConnect = new ConnectViewModel(this, output);
                                                newConnect.ToConnector = input;
                                                output.Connect = newConnect;
                                                newConnect.AgYuku = oldConnect.AgYuku;
                                                newConnect.GucMiktari = oldConnect.GucMiktari;
                                                newConnect.FromConnector.KalanKapasite -= oldConnect.GucMiktari;
                                                newConnect.Uzunluk = oldConnect.Uzunluk;
                                                newConnect.KabloKesitOnerisi = oldConnect.KabloKesitOnerisi;
                                                newConnect.FromConnector.AgAkisList = oldConnect.FromConnector.AgAkisList;
                                                newConnect.ToConnector.AgAkisList = oldConnect.ToConnector.AgAkisList;
                                                AddConnect(newConnect);
                                                connects.Add(newConnect);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        result.Add(new KeyValuePair<KeyValuePair<NodeViewModel, GroupUngroupModel>, List<ConnectViewModel>>(item, connects));
                    }

                    NodeCounter();
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Seçimlerin tamamı grup veya cihaz olmalıdır!";
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var item in result.Select(s => s.Key))
                {
                    if (item.Key.TypeId == (int)TipEnum.Group)
                    {
                        GroupList.Add(item.Value);
                        AddToProjectHierarchy(item.Key);
                        foreach (var node in item.Value.NodeList)
                        {
                            AddChildToProjectHierarchyWithTransitions(item.Key.Name, node);
                        }
                    }
                    else
                    {
                        AddToProjectHierarchy(item.Key);
                    }

                    Nodes.Add(item.Key);
                    LogDebug("Node with name \"{0}\" was copied", item.Key.Name);
                }

                foreach (var item in result.Select(s => s.Value))
                {
                    foreach (var connect in item)
                    {
                        AddConnect(connect);
                    }
                }

                NodeCounter();
            }

            return result;
        }

        private NodeViewModel CopyPasteGroup(GroupUngroupModel willBeCopiedGroup, NodeViewModel willBeCopiedNode)
        {
            var temp = new List<NodeViewModel>();
            var newGroupInputs = new List<ConnectorViewModel>();
            var newGroupOutputs = new List<ConnectorViewModel>();

            foreach (var node in willBeCopiedGroup.NodeList)
            {
                var oldNodeInputs = new List<ConnectorViewModel>();
                foreach (var oldNodeInput in node.InputList)
                {
                    foreach (var oldGroupInput in willBeCopiedGroup.InputList)
                    {
                        if (oldNodeInput.UniqueId == oldGroupInput.UniqueId)
                        {
                            oldNodeInputs.Add(oldNodeInput);
                            break;
                        }
                    }
                }

                var oldNodeOutputs = new List<ConnectorViewModel>();
                foreach (var oldNodeOutput in node.Transitions.Items)
                {
                    foreach (var oldGroupOutput in willBeCopiedGroup.OutputList)
                    {
                        if (oldNodeOutput.UniqueId == oldGroupOutput.UniqueId)
                        {
                            oldNodeOutputs.Add(oldNodeOutput);
                            break;
                        }
                    }
                }

                var newPoint = node.Point1.Addition(50, 50);
                var newNode = new NodeViewModel(this, GetNameForNewNode(node.TypeId, node.Name), Guid.NewGuid(), newPoint, node.Id, node.TypeId,
                    node.AgArayuzuList, node.GucArayuzuList, new List<ConnectorViewModel>(), new List<ConnectorViewModel>(), node.VerimlilikOrani,
                    node.DahiliGucTuketimDegeri, node.Sembol, node.StokNo, node.Tanim, node.UreticiAdi, node.UreticiParcaNo, node.TurAd);

                foreach (var newNodeInput in newNode.InputList)
                {
                    foreach (var oldNodeInput in oldNodeInputs)
                    {
                        if (oldNodeInput.Port == newNodeInput.Port && oldNodeInput.Label == newNodeInput.Label)
                        {
                            newNodeInput.AgAkisList = oldNodeInput.AgAkisList;
                            newGroupInputs.Add(newNodeInput);
                        }
                    }
                }

                foreach (var newNodeOutput in newNode.Transitions.Items)
                {
                    foreach (var oldNodeOutput in oldNodeOutputs)
                    {
                        if (oldNodeOutput.Port == newNodeOutput.Port && oldNodeOutput.Label == newNodeOutput.Label)
                        {
                            newNodeOutput.AgAkisList = oldNodeOutput.AgAkisList;
                            newGroupOutputs.Add(newNodeOutput);
                        }
                    }
                }

                temp.Add(newNode);
            }

            willBeCopiedNode = CreateGroupNodeInOutList(willBeCopiedNode, newGroupInputs, newGroupOutputs);

            var tempInternalConnectList = new List<ConnectViewModel>();
            foreach (var internalConnect in willBeCopiedGroup.InternalConnectList)
            {
                var oldFromConnector = internalConnect.FromConnector;
                var oldToConnector = internalConnect.ToConnector;

                ConnectViewModel connect = null;
                foreach (var node in temp)
                {
                    var newFromConnector = node.Transitions.Items.Where(x => x.Label == oldFromConnector.Label &&
                        x.PositionConnectPoint == oldFromConnector.PositionConnectPoint.Addition((oldFromConnector.Artik * -1) + 50, 50))
                        .FirstOrDefault();

                    if (newFromConnector != null)
                    {
                        newFromConnector.AgAkisList = oldFromConnector.AgAkisList;
                        connect = new ConnectViewModel(this, newFromConnector);
                        break;
                    }
                }

                foreach (var node in temp)
                {
                    var newToConnector = node.InputList.Where(x => x.Label == oldToConnector.Label &&
                        x.PositionConnectPoint == oldToConnector.PositionConnectPoint.Addition(50, 50))
                        .FirstOrDefault();

                    if (newToConnector != null)
                    {
                        newToConnector.AgAkisList = oldToConnector.AgAkisList;
                        connect.ToConnector = newToConnector;
                        break;
                    }
                }

                connect.FromConnector.Connect = connect;
                connect.AgYuku = internalConnect.AgYuku;
                connect.GucMiktari = internalConnect.GucMiktari;
                connect.FromConnector.KalanKapasite -= internalConnect.GucMiktari;
                connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                connect.Uzunluk = internalConnect.Uzunluk;

                tempInternalConnectList.Add(connect);
            }

            var tempExternalConnectList = new List<ConnectViewModel>();

            GroupUngroupModel model = new GroupUngroupModel();
            model.Name = willBeCopiedNode.Name;
            model.UniqueId = willBeCopiedNode.UniqueId;
            model.NodeList = temp;
            model.InternalConnectList = tempInternalConnectList;
            model.ExternalConnectList = tempExternalConnectList;
            model.InputList = willBeCopiedNode.InputList;
            model.OutputList = willBeCopiedNode.OutputList;
            GroupList.Add(model);
            AddToProjectHierarchy(willBeCopiedNode);
            NodeCounter();
            foreach (var node in temp)
            {
                AddChildToProjectHierarchyWithTransitions(willBeCopiedNode.Name, node);
            }

            return willBeCopiedNode;
        }

        private KeyValuePair<NodeViewModel, GroupUngroupModel> RedoGroup(Unit x, KeyValuePair<NodeViewModel, GroupUngroupModel> result)
        {
            var nodeList = new List<NodeViewModel>();
            nodeList.Add(result.Key);

            foreach (var node in nodeList)
            {
                var group = GroupList.Where(g => g.UniqueId == node.UniqueId).FirstOrDefault();

                if (group != null)
                {
                    var willBeDeletedList = new List<ConnectViewModel>();
                    foreach (var connect in Connects)
                    {
                        if (connect.FromConnector.Node == node || connect.ToConnector.Node == node)
                        {
                            willBeDeletedList.Add(connect);
                        }
                    }

                    foreach (var willBeDeleted in willBeDeletedList)
                    {
                        Connects.Remove(willBeDeleted);
                    }

                    Nodes.Remove(node);
                    if (node.TypeId == (int)TipEnum.Group)
                    {
                        DeleteGroupFromNode(node);
                    }
                    DeleteWithRedoFromProjectHierarchy(node);

                    foreach (var groupedNode in group.NodeList)
                    {
                        Nodes.Add(groupedNode);
                        AddToProjectHierarchy(groupedNode);

                        foreach (var output in groupedNode.Transitions.Items)
                        {
                            foreach (var internalConnect in group.InternalConnectList)
                            {
                                if (internalConnect.FromConnector == output)
                                {
                                    bool varMi = false;
                                    foreach (var connect in Connects)
                                    {
                                        if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                        {
                                            varMi = true;
                                            break;
                                        }
                                    }

                                    if (!varMi)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, output);
                                        connect.ToConnector = internalConnect.ToConnector;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = internalConnect.AgYuku;
                                        connect.GucMiktari = internalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = internalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }

                            foreach (var externalConnect in group.ExternalConnectList)
                            {
                                if (externalConnect.FromConnector == output)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, output);
                                    connect.ToConnector = externalConnect.ToConnector;
                                    connect.FromConnector.Connect = connect;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    AddConnect(connect);
                                }
                            }
                        }

                        foreach (var input in groupedNode.InputList)
                        {
                            foreach (var internalConnect in group.InternalConnectList)
                            {
                                if (internalConnect.ToConnector == input)
                                {
                                    bool varMi = false;
                                    foreach (var connect in Connects)
                                    {
                                        if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                        {
                                            varMi = true;
                                            break;
                                        }
                                    }

                                    if (!varMi)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, internalConnect.FromConnector);
                                        connect.ToConnector = input;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = internalConnect.AgYuku;
                                        connect.GucMiktari = internalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = internalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }

                            foreach (var externalConnect in group.ExternalConnectList)
                            {
                                if (externalConnect.ToConnector == input)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                                    connect.ToConnector = externalConnect.ToConnector;
                                    connect.FromConnector.Connect = connect;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    AddConnect(connect);
                                }
                            }
                        }
                    }

                    GroupList.Remove(group);
                }
            }
            NodeCounter();


            return result;
        }
        private KeyValuePair<NodeViewModel, GroupUngroupModel> Group(Unit x, KeyValuePair<NodeViewModel, GroupUngroupModel> result)
        {
            if (result.Key == null && result.Value == null)
            {
                bool grubunGrubuMu = false;
                var nodeList = new List<NodeViewModel>();

                foreach (var node in this.Nodes.Items.Where(x => x.Selected))
                {
                    if (node.TypeId == (int)TipEnum.Group)
                    {
                        grubunGrubuMu = true;
                        break;
                    }
                    nodeList.Add(node);
                }

                if (nodeList.Count() > 1)
                {
                    if (!grubunGrubuMu)
                    {
                        var internalConnectList = new List<ConnectViewModel>();
                        var externalConnectList = new List<ConnectViewModel>();

                        foreach (var connect in Connects)
                        {
                            if (nodeList.Contains(connect.FromConnector.Node) || nodeList.Contains(connect.ToConnector.Node))
                            {
                                if (nodeList.Contains(connect.FromConnector.Node) && nodeList.Contains(connect.ToConnector.Node))
                                {
                                    internalConnectList.Add(connect);
                                }
                                else
                                {
                                    externalConnectList.Add(connect);
                                }
                            }
                        }

                        var temp2 = externalConnectList.Union(internalConnectList);
                        foreach (var item in temp2)
                        {
                            Connects.Remove(item);
                        }
                        CommandDeleteSelectedNodes.Execute();

                        GroupUngroupModel model = new GroupUngroupModel();
                        model.Name = GetNameForNewNode((int)TipEnum.Group, "Grup");
                        model.UniqueId = Guid.NewGuid();
                        model.NodeList = nodeList;
                        model.InternalConnectList = internalConnectList;
                        model.ExternalConnectList = externalConnectList;

                        var tempInputList = new List<ConnectorViewModel>();
                        var tempOutputList = new List<ConnectorViewModel>();

                        foreach (var externalConnect in externalConnectList)
                        {
                            if (nodeList.Contains(externalConnect.FromConnector.Node))
                            {
                                tempOutputList.Add(externalConnect.FromConnector);
                            }
                            else
                            {
                                tempInputList.Add(externalConnect.ToConnector);
                            }
                        }

                        NodeViewModel newNode = new NodeViewModel(this, model.Name, model.UniqueId, new Point(), 0, 9);
                        newNode = CreateGroupNodeInOutList(newNode, tempInputList, tempOutputList);
                        model.InputList = newNode.InputList;
                        model.OutputList = newNode.OutputList;
                        GroupList.Add(model);

                        Nodes.Add(newNode);
                        AddToProjectHierarchy(newNode);
                        result = new KeyValuePair<NodeViewModel, GroupUngroupModel>(newNode, model);

                        foreach (var node in nodeList)
                        {
                            AddChildToProjectHierarchyWithTransitions(newNode.Name, node);
                        }


                        foreach (var output in newNode.Transitions.Items)
                        {
                            foreach (var externalConnect in externalConnectList)
                            {
                                if (externalConnect.FromConnector.UniqueId == output.UniqueId)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, output);
                                    connect.ToConnector = externalConnect.ToConnector;
                                    connect.FromConnector.Connect = connect;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;

                                    AddConnect(connect);
                                }
                            }
                        }

                        foreach (var input in newNode.InputList)
                        {
                            foreach (var externalConnect in externalConnectList)
                            {
                                if (externalConnect.ToConnector.UniqueId == input.UniqueId)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                                    connect.ToConnector = input;
                                    connect.FromConnector.Connect = connect;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;

                                    AddConnect(connect);
                                }
                            }
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Bir grup başka bir cihazla gruplanamaz!";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
            }
            else
            {
                var internalConnectList = new List<ConnectViewModel>();
                var externalConnectList = new List<ConnectViewModel>();

                foreach (var connect in Connects)
                {
                    if (result.Value.NodeList.Contains(connect.FromConnector.Node) || result.Value.NodeList.Contains(connect.ToConnector.Node))
                    {
                        if (result.Value.NodeList.Contains(connect.FromConnector.Node) && result.Value.NodeList.Contains(connect.ToConnector.Node))
                        {
                            internalConnectList.Add(connect);
                        }
                        else
                        {
                            externalConnectList.Add(connect);
                        }
                    }
                }

                var connectsToDelete = externalConnectList.Union(internalConnectList);
                foreach (var item in connectsToDelete)
                {
                    Connects.Remove(item);
                }

                foreach (var item in result.Value.NodeList)
                {
                    Nodes.Remove(item);
                    if (item.TypeId == (int)TipEnum.Group)
                    {
                        DeleteGroupFromNode(item);
                    }
                    DeleteWithRedoFromProjectHierarchy(item);
                }

                result.Value.ExternalConnectList = externalConnectList;
                result.Value.InternalConnectList = internalConnectList;

                GroupList.Add(result.Value);
                Nodes.Add(result.Key);
                AddToProjectHierarchy(result.Key);

                foreach (var node in result.Value.NodeList)
                {
                    AddChildToProjectHierarchyWithTransitions(result.Key.Name, node);
                }

                foreach (var output in result.Key.Transitions.Items)
                {
                    foreach (var externalConnect in externalConnectList)
                    {
                        if (externalConnect.FromConnector.UniqueId == output.UniqueId)
                        {
                            ConnectViewModel connect = new ConnectViewModel(this, output);
                            connect.ToConnector = externalConnect.ToConnector;
                            connect.FromConnector.Connect = connect;
                            connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                            connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                            connect.AgYuku = externalConnect.AgYuku;
                            connect.GucMiktari = externalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = externalConnect.Uzunluk;

                            AddConnect(connect);
                        }
                    }
                }

                foreach (var input in result.Key.InputList)
                {
                    foreach (var externalConnect in externalConnectList)
                    {
                        if (externalConnect.ToConnector.UniqueId == input.UniqueId)
                        {
                            ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                            connect.ToConnector = input;
                            connect.FromConnector.Connect = connect;
                            connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                            connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                            connect.AgYuku = externalConnect.AgYuku;
                            connect.GucMiktari = externalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = externalConnect.Uzunluk;

                            AddConnect(connect);
                        }
                    }
                }
            }

            NodeCounter();

            return result;
        }
        private NodeViewModel CreateGroupNodeInOutList(NodeViewModel newNode, List<ConnectorViewModel> tempInputList, List<ConnectorViewModel> tempOutputList)
        {
            List<ConnectorViewModel> inputList = new List<ConnectorViewModel>();
            List<ConnectorViewModel> outputList = new List<ConnectorViewModel>();

            int i = 0;
            foreach (var input in tempInputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu))
            {
                var inputMain = new ConnectorViewModel(input.NodesCanvas, newNode, input.Name, newNode.Point1.Addition(0, 30 + (i * 20)), input.UniqueId, input.KapasiteId, input.MinKapasite, input.MaxKapasite, input.FizikselOrtamId, null, input.KullanimAmaciId, null, null, null, null, null, null, null, null, input.Label, input.TypeId, input.Id, input.Port, null);
                inputMain.AgAkisList = input.AgAkisList;
                inputList.Add(inputMain);
                i++;
            }

            int count = i;
            i = 0;
            foreach (var input in tempInputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu))
            {
                var inputMain = new ConnectorViewModel(input.NodesCanvas, newNode, input.Name, newNode.Point1.Addition(0, 30 + ((i + count) * 20)), input.UniqueId, null, null, null, null, input.GerilimTipiId, input.KullanimAmaciId,
                                input.GirdiDuraganGerilimDegeri1, input.GirdiDuraganGerilimDegeri2, input.GirdiDuraganGerilimDegeri3, input.GirdiMinimumGerilimDegeri, input.GirdiMaksimumGerilimDegeri, input.GirdiTukettigiGucMiktari,
                                input.CiktiDuraganGerilimDegeri, input.CiktiUrettigiGucKapasitesi, input.Label, input.TypeId, input.Id, input.Port, null);

                inputList.Add(inputMain);
                i++;
            }

            i = 0;
            foreach (var output in tempOutputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu))
            {
                var outputMain = new ConnectorViewModel(output.NodesCanvas, newNode, output.Name, newNode.Point1.Addition(80, 54 + (i * 20)), output.UniqueId, output.KapasiteId, output.MinKapasite, output.MaxKapasite, output.FizikselOrtamId, null, output.KullanimAmaciId, null, null, null, null, null, null, null, null, output.Label, output.TypeId, output.Id, output.Port)
                {
                    Visible = null
                };

                outputMain.AgAkisList = output.AgAkisList;
                outputMain.KalanKapasite = output.KalanKapasite;
                i++;
                outputList.Add(outputMain);
            }

            count = i;
            i = 0;
            foreach (var output in tempOutputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu))
            {
                var outputMain = new ConnectorViewModel(output.NodesCanvas, newNode, output.Name, newNode.Point1.Addition(80, 54 + ((i + count) * 20)), output.UniqueId, null, null, null, null, output.GerilimTipiId, output.KullanimAmaciId,
                                    output.GirdiDuraganGerilimDegeri1, output.GirdiDuraganGerilimDegeri2, output.GirdiDuraganGerilimDegeri3, output.GirdiMinimumGerilimDegeri, output.GirdiMaksimumGerilimDegeri, output.GirdiTukettigiGucMiktari,
                                    output.CiktiDuraganGerilimDegeri, output.CiktiUrettigiGucKapasitesi, output.Label, output.TypeId, output.Id, output.Port)
                {
                    Visible = null
                };

                if (output.TypeId == (int)TipEnum.GucUreticiGucArayuzu && output.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                {
                    outputMain.KalanKapasite = output.KalanKapasite;
                }

                i++;
                outputList.Add(outputMain);
            }

            newNode.InputList = inputList;
            newNode.OutputList = outputList;
            newNode.AddEmptyConnector();

            return newNode;
        }

        private List<KeyValuePair<NodeViewModel, GroupUngroupModel>> RedoUngroup(Unit x, List<KeyValuePair<NodeViewModel, GroupUngroupModel>> result)
        {
            foreach (var res in result)
            {
                var internalConnectList = new List<ConnectViewModel>();
                var externalConnectList = new List<ConnectViewModel>();

                foreach (var connect in Connects)
                {
                    if (res.Value.NodeList.Contains(connect.FromConnector.Node) || res.Value.NodeList.Contains(connect.ToConnector.Node))
                    {
                        if (res.Value.NodeList.Contains(connect.FromConnector.Node) && res.Value.NodeList.Contains(connect.ToConnector.Node))
                        {
                            internalConnectList.Add(connect);
                        }
                        else
                        {
                            externalConnectList.Add(connect);
                        }
                    }
                }

                var connectsToDelete = externalConnectList.Union(internalConnectList);
                foreach (var item in connectsToDelete)
                {
                    Connects.Remove(item);
                }

                foreach (var item in res.Value.NodeList)
                {
                    Nodes.Remove(item);
                    if (item.TypeId == (int)TipEnum.Group)
                    {
                        DeleteGroupFromNode(item);
                    }
                    DeleteWithRedoFromProjectHierarchy(item);
                }

                res.Value.ExternalConnectList = externalConnectList;
                res.Value.InternalConnectList = internalConnectList;

                GroupList.Add(res.Value);
                Nodes.Add(res.Key);
                AddToProjectHierarchy(res.Key);

                foreach (var node in res.Value.NodeList)
                {
                    AddChildToProjectHierarchyWithTransitions(res.Key.Name, node);
                }

                foreach (var output in res.Key.Transitions.Items)
                {
                    foreach (var externalConnect in externalConnectList)
                    {
                        if (externalConnect.FromConnector.UniqueId == output.UniqueId)
                        {
                            ConnectViewModel connect = new ConnectViewModel(this, output);
                            connect.ToConnector = externalConnect.ToConnector;
                            connect.FromConnector.Connect = connect;
                            connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                            connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                            connect.AgYuku = externalConnect.AgYuku;
                            connect.GucMiktari = externalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = externalConnect.Uzunluk;

                            AddConnect(connect);
                        }
                    }
                }

                foreach (var input in res.Key.InputList)
                {
                    foreach (var externalConnect in externalConnectList)
                    {
                        if (externalConnect.ToConnector.UniqueId == input.UniqueId)
                        {
                            ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                            connect.ToConnector = input;
                            connect.FromConnector.Connect = connect;
                            connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                            connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                            connect.AgYuku = externalConnect.AgYuku;
                            connect.GucMiktari = externalConnect.GucMiktari;
                            connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                            connect.Uzunluk = externalConnect.Uzunluk;

                            AddConnect(connect);
                        }
                    }
                }
            }
            NodeCounter();

            return result;
        }
        private List<KeyValuePair<NodeViewModel, GroupUngroupModel>> Ungroup(Unit x, List<KeyValuePair<NodeViewModel, GroupUngroupModel>> result)
        {
            if (result == null)
            {
                result = new List<KeyValuePair<NodeViewModel, GroupUngroupModel>>();

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
                        var willBeDeletedList = new List<ConnectViewModel>();
                        foreach (var connect in Connects)
                        {
                            if (connect.FromConnector.Node == node || connect.ToConnector.Node == node)
                            {
                                willBeDeletedList.Add(connect);
                            }
                        }

                        foreach (var willBeDeleted in willBeDeletedList)
                        {
                            Connects.Remove(willBeDeleted);
                        }

                        Nodes.Remove(node);
                        if (node.TypeId == (int)TipEnum.Group)
                        {
                            DeleteGroupFromNode(node);
                        }
                        DeleteWithRedoFromProjectHierarchy(node);

                        foreach (var groupedNode in group.NodeList)
                        {
                            Nodes.Add(groupedNode);
                            AddToProjectHierarchy(groupedNode);

                            foreach (var output in groupedNode.Transitions.Items)
                            {
                                foreach (var internalConnect in group.InternalConnectList)
                                {
                                    if (internalConnect.FromConnector == output)
                                    {
                                        bool varMi = false;
                                        foreach (var connect in Connects)
                                        {
                                            if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                            {
                                                varMi = true;
                                                break;
                                            }
                                        }

                                        if (!varMi)
                                        {
                                            ConnectViewModel connect = new ConnectViewModel(this, output);
                                            connect.ToConnector = internalConnect.ToConnector;
                                            connect.FromConnector.Connect = connect;
                                            connect.AgYuku = internalConnect.AgYuku;
                                            connect.GucMiktari = internalConnect.GucMiktari;
                                            connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                            connect.Uzunluk = internalConnect.Uzunluk;
                                            connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                            connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                            AddConnect(connect);
                                        }
                                    }
                                }

                                foreach (var externalConnect in group.ExternalConnectList)
                                {
                                    if (externalConnect.FromConnector == output)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, output);
                                        connect.ToConnector = externalConnect.ToConnector;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = externalConnect.AgYuku;
                                        connect.GucMiktari = externalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = externalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }

                            foreach (var input in groupedNode.InputList)
                            {
                                foreach (var internalConnect in group.InternalConnectList)
                                {
                                    if (internalConnect.ToConnector == input)
                                    {
                                        bool varMi = false;
                                        foreach (var connect in Connects)
                                        {
                                            if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                            {
                                                varMi = true;
                                                break;
                                            }
                                        }

                                        if (!varMi)
                                        {
                                            ConnectViewModel connect = new ConnectViewModel(this, internalConnect.FromConnector);
                                            connect.ToConnector = input;
                                            connect.FromConnector.Connect = connect;
                                            connect.AgYuku = internalConnect.AgYuku;
                                            connect.GucMiktari = internalConnect.GucMiktari;
                                            connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                            connect.Uzunluk = internalConnect.Uzunluk;
                                            connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                            connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                            AddConnect(connect);
                                        }
                                    }
                                }

                                foreach (var externalConnect in group.ExternalConnectList)
                                {
                                    if (externalConnect.ToConnector == input)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                                        connect.ToConnector = externalConnect.ToConnector;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = externalConnect.AgYuku;
                                        connect.GucMiktari = externalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = externalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }
                        }

                        GroupList.Remove(group);
                        result.Add(new KeyValuePair<NodeViewModel, GroupUngroupModel>(node, group));
                    }
                }
            }
            else
            {
                foreach (var res in result)
                {
                    var willBeDeletedList = new List<ConnectViewModel>();
                    foreach (var connect in Connects)
                    {
                        if (connect.FromConnector.Node == res.Key || connect.ToConnector.Node == res.Key)
                        {
                            willBeDeletedList.Add(connect);
                        }
                    }

                    foreach (var willBeDeleted in willBeDeletedList)
                    {
                        Connects.Remove(willBeDeleted);
                    }

                    Nodes.Remove(res.Key);
                    if (res.Key.TypeId == (int)TipEnum.Group)
                    {
                        DeleteGroupFromNode(res.Key);
                    }
                    DeleteWithRedoFromProjectHierarchy(res.Key);

                    foreach (var groupedNode in res.Value.NodeList)
                    {
                        Nodes.Add(groupedNode);
                        AddToProjectHierarchy(groupedNode);

                        foreach (var output in groupedNode.Transitions.Items)
                        {
                            foreach (var internalConnect in res.Value.InternalConnectList)
                            {
                                if (internalConnect.FromConnector == output)
                                {
                                    bool varMi = false;
                                    foreach (var connect in Connects)
                                    {
                                        if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                        {
                                            varMi = true;
                                            break;
                                        }
                                    }

                                    if (!varMi)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, output);
                                        connect.ToConnector = internalConnect.ToConnector;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = internalConnect.AgYuku;
                                        connect.GucMiktari = internalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = internalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }

                            foreach (var externalConnect in res.Value.ExternalConnectList)
                            {
                                if (externalConnect.FromConnector == output)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, output);
                                    connect.ToConnector = externalConnect.ToConnector;
                                    connect.FromConnector.Connect = connect;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    AddConnect(connect);
                                }
                            }
                        }

                        foreach (var input in groupedNode.InputList)
                        {
                            foreach (var internalConnect in res.Value.InternalConnectList)
                            {
                                if (internalConnect.ToConnector == input)
                                {
                                    bool varMi = false;
                                    foreach (var connect in Connects)
                                    {
                                        if (connect.FromConnector == internalConnect.FromConnector && connect.ToConnector == internalConnect.ToConnector)
                                        {
                                            varMi = true;
                                            break;
                                        }
                                    }

                                    if (!varMi)
                                    {
                                        ConnectViewModel connect = new ConnectViewModel(this, internalConnect.FromConnector);
                                        connect.ToConnector = input;
                                        connect.FromConnector.Connect = connect;
                                        connect.AgYuku = internalConnect.AgYuku;
                                        connect.GucMiktari = internalConnect.GucMiktari;
                                        connect.KabloKesitOnerisi = internalConnect.KabloKesitOnerisi;
                                        connect.Uzunluk = internalConnect.Uzunluk;
                                        connect.FromConnector.AgAkisList = internalConnect.FromConnector.AgAkisList;
                                        connect.ToConnector.AgAkisList = internalConnect.ToConnector.AgAkisList;
                                        AddConnect(connect);
                                    }
                                }
                            }

                            foreach (var externalConnect in res.Value.ExternalConnectList)
                            {
                                if (externalConnect.ToConnector == input)
                                {
                                    ConnectViewModel connect = new ConnectViewModel(this, externalConnect.FromConnector);
                                    connect.ToConnector = externalConnect.ToConnector;
                                    connect.FromConnector.Connect = connect;
                                    connect.AgYuku = externalConnect.AgYuku;
                                    connect.GucMiktari = externalConnect.GucMiktari;
                                    connect.KabloKesitOnerisi = externalConnect.KabloKesitOnerisi;
                                    connect.Uzunluk = externalConnect.Uzunluk;
                                    connect.FromConnector.AgAkisList = externalConnect.FromConnector.AgAkisList;
                                    connect.ToConnector.AgAkisList = externalConnect.ToConnector.AgAkisList;
                                    AddConnect(connect);
                                }
                            }
                        }
                    }

                    GroupList.Remove(res.Value);
                }
            }

            NodeCounter();

            return result;
        }

        private void EditSelected()
        {
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

        public string GetNameForNewNode(int typeId, string oldName)
        {
            if (oldName.Contains("#"))
            {
                oldName = oldName.Substring(0, oldName.IndexOf("#") - 1);
            }

            var sameNodes = Nodes.Items.Where(x => (x.Name.Contains("#") ? x.Name.Substring(0, x.Name.IndexOf("#") - 1).ToLower() == oldName.ToLower()
                                                                    : x.Name.ToLower() == oldName.ToLower()) && x.TypeId == typeId).ToList();

            foreach (var group in GroupList)
            {
                sameNodes.AddRange(group.NodeList.Where(x => (x.Name.Contains("#") ? x.Name.Substring(0, x.Name.IndexOf("#") - 1).ToLower() == oldName.ToLower()
                                                                    : x.Name.ToLower() == oldName.ToLower()) && x.TypeId == typeId).ToList());
            }

            int count = 1;
            if (sameNodes != null && sameNodes.Count != 0)
            {
                string nodeName = sameNodes.First().Name;
                count = Convert.ToInt32(nodeName.Substring(nodeName.IndexOf("#") + 1));
                foreach (var sameNode in sameNodes)
                {
                    int count2 = Convert.ToInt32(sameNode.Name.Substring(sameNode.Name.IndexOf("#") + 1));
                    if (count2 > count)
                    {
                        count = count2;
                    }
                }

                count = count + 1;
            }

            string name = oldName + " #" + count;

            return name;
        }

        private void SelectConnects()
        {
            //Point cutterStartPoint = Cutter.StartPoint.Division(Scale.Value);
            //Point cutterEndPoint = Cutter.EndPoint.Division(Scale.Value);

            Point cutterStartPoint = Cutter.StartPoint;
            Point cutterEndPoint = Cutter.EndPoint;

            var connects = Connects.Where(x => MyUtils.CheckIntersectCubicBezierCurveAndLine(x.StartPoint, x.Point1, x.Point2, x.EndPoint, cutterStartPoint, cutterEndPoint));
            foreach (var connect in Connects)
            {
                connect.Selected = false;
            }

            foreach (var connect in connects)
            {
                connect.Selected = true;
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
            this.SetupStartState();
            NodeCounter();

        }
        private void ClearScheme()
        {
            this.MainWindow.OpenProjectName.Content = "";
            this.Nodes.Clear();
            this.Connects.Clear();
            this.NodesCount = 0;
            this.TransitionsCount = 0;
            this.SchemePath = "";
            this.ImagePath = "";
            WithoutMessages = false;
            this.Messages.Clear();
            ItSaved = true;
            StartState = null;
            DeleteProjectHierarchy();
            DeleteDogrulamaPaneli();
            ConnectClipboard.Clear();
            NodeClipboard.Clear();
            GroupList.Clear();

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
            string filenamewithextension = System.IO.Path.GetFileName(Dialog.FileName);
            ClearScheme();

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
                else
                {
                    if (viewModelNode.TypeId != (int)TipEnum.Group)
                    {
                        AddToProjectHierarchy(viewModelNode);
                    }
                }
            }

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

            #region setup dogrulamalar

            var Dogrulamalar = stateMachineXElement.Element("Dogrulamalar")?.Elements()?.ToList() ?? new List<XElement>();
            DogrulamaModel viewModelDogrulama = null;
            foreach (var dogrulama in Dogrulamalar)
            {
                viewModelDogrulama = DogrulamaModel.FromXElement(this, dogrulama, out string errorMesage, ConnectsExist);
                if (WithError(errorMesage, x => this.MainWindow.DogrulamaDataGrid.Items.Add(x), viewModelDogrulama))
                    return;
            }
            SchemePath = fileName;

            #endregion  setup Transitions/connects

            #region setup Groups
            var Groups = stateMachineXElement.Element("Groups")?.Elements()?.ToList() ?? new List<XElement>();
            GroupUngroupModel viewModelGroup = null;
            foreach (var group in Groups)
            {
                viewModelGroup = GroupUngroupModel.FromXElement(this, group, out string errorMesage, ConnectsExist, stateMachineXElement);
                if (WithError(errorMesage, x => this.GroupList.Add(x), viewModelGroup))
                    return;
                else
                {
                    var rootNode = Nodes.Items.Where(x => x.UniqueId == viewModelGroup.UniqueId).FirstOrDefault();
                    AddToProjectHierarchy(rootNode);


                    foreach (var node in viewModelGroup.NodeList)
                    {
                        AddChildToProjectHierarchyWithTransitions(rootNode.Name, node);
                    }
                }
            }
            #endregion

            #region setup Visualization
            XElement Visualization = stateMachineXElement.Element("Visualization");


            if (Visualization != null)
            {
                var visualizationStates = Visualization.Elements()?.ToList();
                if (visualizationStates != null)
                {
                    var nodes = this.Nodes.Items.ToDictionary(x => x.UniqueId, x => x);
                    Point point;
                    bool isCollapse;
                    Guid uniqueId;
                    string pointAttribute;
                    string isCollapseAttribute;
                    foreach (var visualization in visualizationStates)
                    {
                        uniqueId = new Guid(visualization.Attribute("UniqueId")?.Value);
                        if (nodes.TryGetValue(uniqueId, out NodeViewModel node))
                        {
                            pointAttribute = visualization.Attribute("Position")?.Value;
                            if (!PointExtensition.TryParseFromString(pointAttribute, out point))
                            {
                                Error(String.Format("Error parse attribute \'position\' for state with name \'{0}\'", uniqueId));
                                return;
                            }
                            isCollapseAttribute = visualization.Attribute("IsCollapse")?.Value;
                            if (!bool.TryParse(isCollapseAttribute, out isCollapse))
                            {
                                Error(String.Format("Error parse attribute \'isCollapse\' for state with name \'{0}\'", uniqueId));
                                return;
                            }
                            node.Point1 = point;
                            node.IsCollapse = isCollapse;
                        }
                        else
                        {
                            Error(String.Format("Visualization for state with name \'{0}\' that not exist", uniqueId));
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

            this.MainWindow.OpenProjectName.Content = filenamewithextension;

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
                //System.IO.File.WriteAllText(SchemePath, string.Empty);
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

            Dialog.ShowSaveFileDialog("XML-File | *.xml", SchemeName(), "Save scheme as...");
            if (Dialog.Result != DialogResult.Ok)
                return;

            Save(Dialog.FileName);

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

            XElement inputs = new XElement("Inputs");
            stateMachineXElement.Add(inputs);
            foreach (var input in Nodes.Items.SelectMany(x => x.InputList))
            {
                inputs.Add(input.ToInputXElement());
            }

            XElement outputs = new XElement("Outputs");
            stateMachineXElement.Add(outputs);
            foreach (var output in Nodes.Items.SelectMany(x => x.Transitions.Items))
            {
                outputs.Add(output.ToOutputXElement());
            }

            XElement gucArayuzus = new XElement("GucArayuzus");
            stateMachineXElement.Add(gucArayuzus);
            foreach (var node in Nodes.Items)
            {
                if (node.GucArayuzuList != null)
                {
                    foreach (var gucArayuzu in node.GucArayuzuList)
                    {
                        gucArayuzus.Add(gucArayuzu.ToXElement(node.UniqueId));
                    }
                }
            }

            XElement agArayuzus = new XElement("AgArayuzus");
            stateMachineXElement.Add(agArayuzus);
            foreach (var node in Nodes.Items)
            {
                if (node.AgArayuzuList != null)
                {

                    foreach (var agArayuzu in node.AgArayuzuList)
                    {
                        agArayuzus.Add(agArayuzu.ToXElement(node.UniqueId));
                    }
                }
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

            XElement agAkisVarisNoktalari = new XElement("AgAkisVarisNoktalari");
            stateMachineXElement.Add(agAkisVarisNoktalari);
            foreach (var input in Nodes.Items.SelectMany(x => x.InputList))
            {
                foreach (var agAkis in input.AgAkisList)
                {
                    foreach (var varisNoktalari in agAkis.VarisNoktasiIdNameList)
                    {
                        XElement element = new XElement("VarisNoktasi");
                        element.Add(new XAttribute("AgAkisId", agAkis.Id));
                        element.Add(new XAttribute("Id", varisNoktalari.Key));
                        element.Add(new XAttribute("Name", varisNoktalari.Value));
                        agAkisVarisNoktalari.Add(element);
                    }
                }
            }

            foreach (var output in Nodes.Items.SelectMany(x => x.Transitions.Items))
            {
                foreach (var agAkis in output.AgAkisList)
                {
                    foreach (var varisNoktalari in agAkis.VarisNoktasiIdNameList)
                    {
                        XElement element = new XElement("VarisNoktasi");
                        element.Add(new XAttribute("AgAkisId", agAkis.Id));
                        element.Add(new XAttribute("Id", varisNoktalari.Key));
                        element.Add(new XAttribute("Name", varisNoktalari.Value));
                        agAkisVarisNoktalari.Add(element);
                    }
                }
            }

            XElement dogrulamalar = new XElement("Dogrulamalar");
            stateMachineXElement.Add(dogrulamalar);
            foreach (var item in this.MainWindow.DogrulamaDataGrid.Items)
            {
                //dogrulamalar.Add((item as DogrulamaModel).ToXElement());
            }

            XElement groups = new XElement("Groups");
            stateMachineXElement.Add(groups);
            foreach (var group in this.GroupList)
            {
                groups.Add(group.ToXElement());
            }

            XElement nodes = new XElement("GroupNodes");
            stateMachineXElement.Add(nodes);
            foreach (var group in GroupList)
            {
                foreach (var node in group.NodeList)
                {
                    nodes.Add(node.ToGroupNodeXElement(group.UniqueId));
                }
            }

            XElement groupNodeInputs = new XElement("GroupNodeInputs");
            stateMachineXElement.Add(groupNodeInputs);
            foreach (var group in GroupList)
            {
                foreach (var input in group.NodeList.SelectMany(x => x.InputList))
                {
                    groupNodeInputs.Add(input.ToGroupInputXEelement());
                }
            }

            XElement groupNodeOutputs = new XElement("GroupNodeOutputs");
            stateMachineXElement.Add(groupNodeOutputs);
            foreach (var group in GroupList)
            {
                foreach (var output in group.NodeList.SelectMany(x => x.Transitions.Items))
                {
                    groupNodeOutputs.Add(output.ToGroupOutputXEelement());
                }
            }

            XElement groupNodeAgArayuzus = new XElement("GroupNodeAgArayuzus");
            stateMachineXElement.Add(groupNodeAgArayuzus);
            foreach (var group in GroupList)
            {
                foreach (var node in group.NodeList)
                {
                    foreach (var agArayuzu in node.AgArayuzuList)
                    {
                        groupNodeAgArayuzus.Add(agArayuzu.ToXElement(node.UniqueId));
                    }
                }
            }

            XElement groupNodeGucArayuzus = new XElement("GroupNodeGucArayuzus");
            stateMachineXElement.Add(groupNodeGucArayuzus);
            foreach (var group in GroupList)
            {
                foreach (var node in group.NodeList)
                {
                    foreach (var gucArayuzu in node.GucArayuzuList)
                    {
                        groupNodeGucArayuzus.Add(gucArayuzu.ToXElement(node.UniqueId));
                    }
                }
            }

            XElement groupNodeAgAkislari = new XElement("GroupNodeAgAkislari");
            stateMachineXElement.Add(groupNodeAgAkislari);
            foreach (var group in GroupList)
            {
                foreach (var input in group.NodeList.SelectMany(x => x.InputList))
                {
                    foreach (var agAkis in input.AgAkisList)
                    {
                        groupNodeAgAkislari.Add(agAkis.ToXElement());
                    }
                }
            }

            foreach (var group in GroupList)
            {
                foreach (var output in group.NodeList.SelectMany(x => x.Transitions.Items))
                {
                    foreach (var agAkis in output.AgAkisList)
                    {
                        groupNodeAgAkislari.Add(agAkis.ToXElement());
                    }
                }
            }


            XElement internalConnects = new XElement("GroupInternalConnects");
            stateMachineXElement.Add(internalConnects);
            foreach (var group in GroupList)
            {
                foreach (var internalConnect in group.InternalConnectList)
                {
                    internalConnects.Add(internalConnect.ToInternalXElement(group.UniqueId));
                }
            }

            XElement externalConnects = new XElement("GroupExternalConnects");
            stateMachineXElement.Add(externalConnects);
            foreach (var group in GroupList)
            {
                foreach (var externalConnect in group.ExternalConnectList)
                {
                    externalConnects.Add(externalConnect.ToExternalXElement(group.UniqueId));
                }
            }

            XElement groupInputs = new XElement("GroupInputs");
            stateMachineXElement.Add(groupInputs);
            foreach (var group in GroupList)
            {
                foreach (var input in group.InputList)
                {
                    groupInputs.Add(input.ToGroupXInputXEelement());
                }
            }

            XElement groupOutputs = new XElement("GroupOutputs");
            stateMachineXElement.Add(groupOutputs);
            foreach (var group in GroupList)
            {
                foreach (var output in group.OutputList)
                {
                    groupOutputs.Add(output.ToGroupXOutputXEelement());
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

            NotifySuccessPopup nfp = new NotifySuccessPopup();
            nfp.msg.Text = "Proje başarıyla kaydedildi.";
            nfp.Owner = this.MainWindow;
            nfp.Show();
            LogDebug("Scheme was saved as \"{0}\"", SchemePath);

            this.MainWindow.OpenProjectName.Content = System.IO.Path.GetFileName(fileName);
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

        public void Zoom((Point point, double delta) element)
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

            if (ViewModelConnect.FromConnector.Node.TypeId == (int)TipEnum.GucUretici)
            {
                ViewModelConnect.FromConnector.KalanKapasite += ViewModelConnect.ToConnector.GirdiTukettigiGucMiktari.Value;
            }
            else
            {
                if (ViewModelConnect.FromConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == ViewModelConnect.FromConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var output = node.Transitions.Items.Where(x => x.UniqueId == ViewModelConnect.FromConnector.UniqueId).FirstOrDefault();

                        if (output != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.FromConnector == output && x.ToConnector == ViewModelConnect.ToConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                if (ViewModelConnect.ToConnector.Node.TypeId == (int)TipEnum.Group)
                {
                    var group = GroupList.Where(x => x.UniqueId == ViewModelConnect.ToConnector.Node.UniqueId).FirstOrDefault();

                    foreach (var node in group.NodeList)
                    {
                        var input = node.InputList.Where(x => x.UniqueId == ViewModelConnect.ToConnector.UniqueId).FirstOrDefault();

                        if (input != null)
                        {
                            var connectToDelete = group.ExternalConnectList.Where(x => x.ToConnector == input && x.FromConnector == ViewModelConnect.FromConnector).FirstOrDefault();
                            group.ExternalConnectList.Remove(connectToDelete);
                        }
                    }
                }

                if (ViewModelConnect.ToConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                {
                    foreach (var output in ViewModelConnect.ToConnector.Node.Transitions.Items)
                    {
                        var beDeletedList = output.AgAkisList.Where(x => x.IliskiliAgArayuzuId == ViewModelConnect.ToConnector.UniqueId).ToList();

                        foreach (var beDeleted in beDeletedList)
                        {
                            output.AgAkisList.Remove(beDeleted);

                            foreach (var connect in Connects)
                            {
                                if (connect.FromConnector == output)
                                {
                                    connect.AgYuku -= beDeleted.Yuk;
                                    var deletedList = connect.ToConnector.AgAkisList.Where(x => x.IliskiliAgArayuzuId == ViewModelConnect.ToConnector.UniqueId).ToList();
                                    foreach (var deleted in deletedList)
                                    {
                                        connect.ToConnector.AgAkisList.Remove(deleted);
                                    }
                                }
                            }
                        }
                    }

                    ViewModelConnect.ToConnector.AgAkisList = new List<AgAkis>();
                }
                else if (ViewModelConnect.ToConnector.Node.TypeId == (int)TipEnum.UcBirim)
                {
                    ViewModelConnect.ToConnector.AgAkisList = new List<AgAkis>();
                }

            }

            ViewModelConnect.FromConnector.Connect = null;
            ViewModelConnect.FromConnector.Node.CurrentConnector = null;
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
                string newName = "";
                if (string.IsNullOrEmpty(parameter.Node.Name))
                {
                    newName = GetNameForNewNode(parameter.Node.TypeId, parameter.Node.Tanim);
                }
                else
                {
                    newName = GetNameForNewNode(parameter.Node.TypeId, parameter.Node.Name);
                }

                newNode = new NodeViewModel(this, newName, Guid.NewGuid(), parameter.Point, parameter.Node.Id, parameter.Node.TypeId,
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

            if (newNode.TypeId == (int)TipEnum.Group)
            {
                var willBeCopiedGroup = GroupList.Where(x => x.UniqueId == parameter.Node.UniqueId).FirstOrDefault();
                newNode = CopyPasteGroup(willBeCopiedGroup, newNode);
            }
            else
            {
                AddToProjectHierarchy(newNode);
            }

            Nodes.Add(newNode);
            NodeCounter();
            LogDebug("Node with name \"{0}\" was added", newNode.Name);
            return newNode;
        }
        private NodeViewModel DeleteNodetWithUndoRedo(ExternalNode parameter, NodeViewModel result)
        {
            Nodes.Remove(result);
            if (result.TypeId == (int)TipEnum.Group)
            {
                DeleteGroupFromNode(result);
            }
            DeleteWithRedoFromProjectHierarchy(result);
            LogDebug("Node with name \"{0}\" was removed", result.Name);
            NodeCounter();
            return result;
        }

        private ConnectorViewModel AddConnectorWithConnect(ConnectorViewModel parameter, ConnectorViewModel result)
        {
            if (result == null)
            {
                return parameter;
            }
            else
                TransitionsCount--;

            result.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((1, result));
            LogDebug("Transition with name \"{0}\" was added", result.Name);
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

            if ((result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedConnectors.Execute();
            }
            if (result == DeleteMode.DeleteConnects)
            {
                CommandDeleteSelectedConnects.Execute();
            }
            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedNodes.Execute();
            }
            NodeCounter();
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
            NodeCounter();
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
                var connects = new List<ConnectViewModel>();

                foreach (var connect in Connects)
                {
                    if (element.connector == connect.FromConnector)
                    {
                        connects.Add(connect);
                    }
                }

                if (connects.Count == 1)
                {
                    element.connector.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(element.connector);
                }
                else
                {
                    foreach (var connect in connects)
                    {
                        element.connector.Node.NodesCanvas.CommandDeleteConnect.ExecuteWithSubscribe(connect);
                    }
                }

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

        private List<(int index, ConnectViewModel connect)> DeleteSelectedConnects(List<ConnectViewModel> parameter, List<(int index, ConnectViewModel connect)> result)
        {
            if (result == null)
            {

                result = new List<(int index, ConnectViewModel element)>();
                foreach (var connect in parameter ?? Connects.Where(x => x.Selected))
                {
                    result.Add((Connects.IndexOf(connect), connect));
                }
            }

            foreach (var element in result)
            {
                element.connect.FromConnector.Node.NodesCanvas.CommandDeleteConnect.ExecuteWithSubscribe(element.connect);
            }

            return result;
        }
        private List<(int index, ConnectViewModel connector)> UnDeleteSelectedConnects(List<ConnectViewModel> parameter, List<(int index, ConnectViewModel connect)> result)
        {
            foreach (var element in result)
            {
                element.connect.FromConnector.NodesCanvas.CommandAddConnect.ExecuteWithSubscribe((element.connect));
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
                        result.ConnectsToDelete.Add(connect);
                    }
                }
            }
            
            Connects.RemoveMany(result.ConnectsToDelete);
            Nodes.RemoveMany(result.NodesToDelete);
            foreach (var item in result.NodesToDelete)
            {
                if (item.TypeId == (int)TipEnum.Group)
                {
                    DeleteGroupFromNode(item);
                }
            }
            foreach (var connectToDelete in result.ConnectsToDelete)
            {
                //RemoveFromDogrulamaPaneli(connectToDelete.FromConnector);
            }

            foreach (var node in result.NodesToDelete)
            {
                DeleteWithRedoFromProjectHierarchy(node);
                LogDebug("Node with name \"{0}\" was removed", node.Name);
            }
            NodeCounter();
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
            NodeCounter();
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

        #region Dogrulama Paneli

        private void DeleteDogrulamaPaneli()
        {
            this.MainWindow.DogrulamaDataGrid.Items.Clear();
        }
        #endregion

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

        private void AddChildToProjectHierarchyWithTransitions(string rootName, NodeViewModel childNode)
        {
            MainWindow.ProjectHierarchyAddChild(rootName, childNode);
        }
        private void DeleteProjectHierarchy()
        {
            MainWindow.ProjeHiyerarsi.Items.Clear();
        }

        #endregion

        private void CopyMultiple()
        {
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

        private void NodeCounter()
        {
            int totalNodesCount = 0;

            List<NodeViewModel> groupNodes = this.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.Group).ToList();
            List<NodeViewModel> otherNodes = this.Nodes.Items.Where(x => x.TypeId != (int)TipEnum.Group).ToList();

            foreach (var i in groupNodes)
            {
                var group = GroupList.Where(g => g.UniqueId == i.UniqueId).FirstOrDefault();
                if (group != null)
                {
                    totalNodesCount += group.NodeList.Count();
                }
            }

            totalNodesCount += otherNodes.Count();
            MainWindow.DeviceCounter(totalNodesCount);
        }

        private void DeleteGroupFromNode(NodeViewModel node)
        {
            GroupList.Remove(GroupList.Where(x => x.UniqueId == node.UniqueId).FirstOrDefault());
        }
    }
}
