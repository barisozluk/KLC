using DynamicData;
using ReactiveUI;
using AYP.Helpers;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using System;
using System.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AYP.Enums;
using AYP.Validations;
using AYP.Models;
using AYP.Entities;
using System.Collections.Generic;
using AYP.Interfaces;
using AYP.DbContext.AYP.DbContexts;
using AYP.Services;

namespace AYP.ViewModel
{
    public partial class ConnectorViewModel
    {
        public ReactiveCommand<Unit, Unit> CommandConnectPointDrag { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectPointDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCheckConnectPointDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDrag { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDragEnter { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSetAsLoop { get; set; }
        public ReactiveCommand<SelectMode, Unit> CommandSelect { get; set; }
        public ReactiveCommand<string, Unit> CommandValidateName { get; set; }

        private void SetupCommands()
        {

            CommandConnectPointDrag = ReactiveCommand.Create(ConnectPointDrag);
            CommandConnectPointDrop = ReactiveCommand.Create(ConnectPointDrop);
            CommandSetAsLoop = ReactiveCommand.Create(SetAsLoop);
            CommandCheckConnectPointDrop = ReactiveCommand.Create(CheckConnectPointDrop);

            CommandConnectorDrag = ReactiveCommand.Create(ConnectorDrag);
            CommandConnectorDragEnter = ReactiveCommand.Create(ConnectorDragEnter);
            CommandConnectorDrop = ReactiveCommand.Create(ConnectorDrop);

            CommandValidateName = ReactiveCommand.Create<string>(ValidateName);

            CommandSelect = ReactiveCommand.Create<SelectMode>(Select);

            NotSavedSubscribe();
        }

        private void NotSavedSubscribe()
        {
            CommandSetAsLoop.Subscribe(_ => NotSaved());
            CommandValidateName.Subscribe(_ => NotSaved());
        }

        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }

        private void SetAsLoop()
        {
            if (this == Node.Output)
                return;
            ToLoop();
            ItsLoop = true;
            //Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
        }
        private void ToLoop()
        {
            this.FormStrokeThickness = 0;
            this.FormFill = Application.Current.Resources["IconLoop"] as DrawingBrush;
        }
        private void ConnectPointDrag()
        {
            NodesCanvas.CommandAddDraggedConnect.ExecuteWithSubscribe(Node.CurrentConnector);
        }

        private void ConnectPointDrop()
        {
            Validation validation = new Validation();
            var valid = validation.ValidateDuringDrawEnd(NodesCanvas, this);

            if (valid)
            {
                var connect = NodesCanvas.DraggedConnect;
                if (connect.FromConnector.Node != this.Node)
                {
                    connect.ToConnector = this;
                    var temp = connect.FromConnector;
                    if (connect.FromConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var groupNode = this.NodesCanvas.GroupList.Where(x => x.UniqueId == connect.FromConnector.Node.UniqueId).FirstOrDefault();

                        foreach (var node in groupNode.NodeList)
                        {
                            var output = node.Transitions.Items.Where(x => x.Label == connect.FromConnector.Label).FirstOrDefault();
                            if (output != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this.NodesCanvas, output);
                                c.ToConnector = connect.ToConnector;
                                groupNode.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    if (connect.ToConnector.Node.TypeId == (int)TipEnum.Group)
                    {
                        var groupNode = this.NodesCanvas.GroupList.Where(x => x.UniqueId == connect.ToConnector.Node.UniqueId).FirstOrDefault();
                        foreach (var node in groupNode.NodeList)
                        {
                            var input = node.InputList.Where(x => x.Label == connect.ToConnector.Label).FirstOrDefault();
                            if (input != null)
                            {
                                ConnectViewModel c = new ConnectViewModel(this.NodesCanvas, connect.FromConnector);
                                c.ToConnector = input;
                                groupNode.ExternalConnectList.Add(c);
                                break;
                            }
                        }
                    }

                    if (connect.FromConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                       
                    }
                    else if (connect.FromConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        int connectSayisi = 0;
                        foreach (var output in connect.FromConnector.Node.Transitions.Items)
                        {
                            if (output.Connect != null)
                            {
                                connectSayisi++;
                            }
                        }

                        foreach (var output in connect.FromConnector.Node.Transitions.Items)
                        {
                            if (output.Connect != null)
                            {
                                if (connect.FromConnector.Node.InputList.Count() > 0)
                                {
                                    output.AgAkisList = new List<AgAkis>();
                                }

                                foreach (var input in connect.FromConnector.Node.InputList)
                                {
                                    if (input.AgAkisList != null && input.AgAkisList.Count() > 0)
                                    {
                                        foreach (var agAkis in input.AgAkisList)
                                        {
                                            var tempAgAkis = new AgAkis();
                                            tempAgAkis.Id = Guid.NewGuid();
                                            tempAgAkis.AgArayuzuId = output.UniqueId;
                                            tempAgAkis.Yuk = agAkis.Yuk / connectSayisi;
                                            tempAgAkis.AgAkisTipiId = agAkis.AgAkisTipiId;
                                            tempAgAkis.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                            tempAgAkis.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                                            tempAgAkis.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                                            tempAgAkis.IliskiliAgArayuzuId = input.UniqueId;
                                            tempAgAkis.IliskiliAgArayuzuAdi = input.Label;

                                            output.AgAkisList.Add(tempAgAkis);
                                        }
                                    }
                                }

                                output.Connect.ToConnector.AgAkisList = new List<AgAkis>();
                                foreach (var agAkis in output.AgAkisList)
                                {
                                    var tempAgAkis = new AgAkis();
                                    tempAgAkis.Id = Guid.NewGuid();
                                    tempAgAkis.AgArayuzuId = output.Connect.ToConnector.UniqueId;
                                    tempAgAkis.Yuk = agAkis.Yuk;
                                    tempAgAkis.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                                    tempAgAkis.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                                    tempAgAkis.AgAkisTipiId = agAkis.AgAkisTipiId;
                                    tempAgAkis.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                    tempAgAkis.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                                    tempAgAkis.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;

                                    output.Connect.ToConnector.AgAkisList.Add(tempAgAkis);
                                }

                                output.Connect.AgYuku = output.AgAkisList.Select(s => s.Yuk).Sum();
                            }
                        }
                    }
                    else if (connect.FromConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                    {
                        connect.GucMiktari = connect.ToConnector.GirdiTukettigiGucMiktari.HasValue ? connect.ToConnector.GirdiTukettigiGucMiktari.Value : 0;
                    }

                    if (connect.FromConnector.Node.TypeId != (int)TipEnum.Group)
                    {
                        this.NodesCanvas.MainWindow.IsEnabled = false;
                        System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                        blur.Radius = 2;
                        this.NodesCanvas.MainWindow.Effect = blur;

                        CableLengthPopupWindow cl = new CableLengthPopupWindow(connect);
                        cl.Owner = this.NodesCanvas.MainWindow;
                        cl.ShowDialog();
                    }
                }
                else
                {
                    this.NodesCanvas.CommandDeleteConnect.Execute(connect);
                }
            }

        }

        private void CheckConnectPointDrop()
        {
            if (NodesCanvas.DraggedConnect.ToConnector == null)
            {
                NodesCanvas.CommandDeleteDraggedConnect.ExecuteWithSubscribe();
            }
            else
            {
                NodesCanvas.CommandAddConnectorWithConnect.Execute(Node.CurrentConnector);
                //Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
                NodesCanvas.DraggedConnect = null;
            }
        }

        private void ConnectorDrag()
        {
            NodesCanvas.ConnectorPreviewForDrop = this;
        }
        private void ConnectorDragEnter()
        {
            if (Node != NodesCanvas.ConnectorPreviewForDrop.Node)
                return;

            int indexTo = Node.Transitions.Items.IndexOf(this);
            if (indexTo == 0)
                return;
            int count = this.Node.Transitions.Count;
            int indexFrom = this.Node.Transitions.Items.IndexOf(this.NodesCanvas.ConnectorPreviewForDrop);

            if ((indexFrom > -1) && (indexTo > -1) && (indexFrom < count) && (indexTo < count))
            {
                Point positionTo = this.Node.Transitions.Items.ElementAt(indexTo).PositionConnectPoint;
                Point position;
                //shift down
                if (indexTo > indexFrom)
                {
                    for (int i = indexTo; i >= indexFrom + 1; i--)
                    {
                        position = this.Node.Transitions.Items.ElementAt(i - 1).PositionConnectPoint;
                        this.Node.Transitions.Items.ElementAt(i).PositionConnectPoint = position;
                    }
                }
                //shift up
                else if (indexFrom > indexTo)
                {
                    for (int i = indexTo; i <= indexFrom - 1; i++)
                    {
                        position = this.Node.Transitions.Items.ElementAt(i + 1).PositionConnectPoint;
                        this.Node.Transitions.Items.ElementAt(i).PositionConnectPoint = position;
                    }
                }
                this.Node.Transitions.Items.ElementAt(indexFrom).PositionConnectPoint = positionTo;
                this.Node.Transitions.Move(indexFrom, indexTo);
            }
        }
        private void ConnectorDrop()
        {
            this.NodesCanvas.ConnectorPreviewForDrop = null;
        }
        private void ValidateName(string newName)
        {
            NodesCanvas.CommandValidateConnectName.ExecuteWithSubscribe((this, newName));
        }


        private void Select(bool value)
        {
            this.FormStroke = Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
            this.Foreground = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorForeground"] as SolidColorBrush;
            if (!this.ItsLoop)
            {
                if (this.Node.TypeId == (int)TipEnum.UcBirim || this.Node.TypeId == (int)TipEnum.AgAnahtari || this.Node.TypeId == (int)TipEnum.Group)
                {
                    if (TypeId == (int)TipEnum.UcBirimAgArayuzu || TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        if (this.KapasiteId == (int)KapasiteEnum.Ethernet)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorEthernet"] as SolidColorBrush;
                        }
                        else if (this.KapasiteId == (int)KapasiteEnum.FastEthernet)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorFastEthernet"] as SolidColorBrush;
                        }
                        else if (this.KapasiteId == (int)KapasiteEnum.GigabitEthernet)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorGigabitEthernet"] as SolidColorBrush;
                        }
                        else if (this.KapasiteId == (int)KapasiteEnum._10GigabitEthernet)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnector10GigabitEthernet"] as SolidColorBrush;
                        }
                        else if (this.KapasiteId == (int)KapasiteEnum._40GigabitEthernet)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnector40GigabitEthernet"] as SolidColorBrush;
                        }
                    }
                    else
                    {
                        if (this.GerilimTipiId == (int)GerilimTipiEnum.AC)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorAC"] as SolidColorBrush;
                        }
                        else if (this.GerilimTipiId == (int)GerilimTipiEnum.DC)
                        {
                            this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorDC"] as SolidColorBrush;
                        }
                    }
                }
                else
                {
                    if (this.GerilimTipiId == (int)GerilimTipiEnum.AC)
                    {
                        this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorAC"] as SolidColorBrush;
                    }
                    else if (this.GerilimTipiId == (int)GerilimTipiEnum.DC)
                    {
                        this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnectorDC"] as SolidColorBrush;
                    }
                }
                //this.FormFill = Application.Current.Resources[this.Selected ? "ColorSelectedElement" : "ColorConnector"] as SolidColorBrush;

            }
            else
            {
                this.FormFill = Application.Current.Resources[this.Selected ? "IconSelectedLoop" : "IconLoop"] as DrawingBrush;
            }
        }
        private void Select(SelectMode selectMode)
        {
            switch (selectMode)
            {
                case SelectMode.Click:
                    {
                        if (!this.Selected)
                        {
                            this.Node.CommandSetConnectorAsStartSelect.ExecuteWithSubscribe(this);
                        }

                        break;
                    }
                case SelectMode.ClickWithCtrl:
                    {
                        this.Selected = !this.Selected;
                        break;
                    }
                case SelectMode.ClickWithShift:
                    {
                        this.Node.CommandSelectWithShiftForConnectors.ExecuteWithSubscribe(this);
                        break;
                    }
            }


        }
    }
}
