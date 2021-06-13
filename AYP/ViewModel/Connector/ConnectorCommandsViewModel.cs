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
                    if (temp.Node.TypeId == (int)TipEnum.Group)
                    {
                        var groupNode = this.NodesCanvas.GroupList.Where(x => x.UniqueId == temp.Node.UniqueId).FirstOrDefault();

                        foreach (var node in groupNode.NodeList)
                        {
                            var output = node.Transitions.Items.Where(x => x.Label == temp.Label).FirstOrDefault();
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

                    if (temp.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                    {
                        if(connect.ToConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            if (temp.AgAkisList != null && temp.AgAkisList.Count() != 0)
                            {
                                connect.ToConnector.AgAkisList = new List<AgAkis>();
                                connect.ToConnector.AgAkisList.Clear();
                                foreach (var agAkis in temp.AgAkisList)
                                {
                                    var agAkisTemp = new AgAkis();
                                    agAkisTemp.Id = Guid.NewGuid();
                                    agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                    agAkisTemp.Yuk = agAkis.Yuk;
                                    agAkisTemp.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                                    agAkisTemp.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                                    agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                    agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                    agAkisTemp.VarisNoktasiIdNameList = agAkis.VarisNoktasiIdNameList;

                                    connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                    connect.AgYuku = temp.AgAkisList.Select(x => x.Yuk).Sum();
                                }
                            }
                        }
                    }
                    else if (temp.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                    {
                        if (connect.ToConnector.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                        {
                            if (temp.AgAkisList != null && temp.AgAkisList.Count() != 0)
                            {
                                bool varisNoktasiMi = false;
                                foreach (var agAkis in temp.AgAkisList)
                                {
                                    varisNoktasiMi = agAkis.VarisNoktasiIdNameList.Where(x => x.Key == connect.ToConnector.Node.UniqueId).Any();
                                    if(varisNoktasiMi)
                                    {
                                        break;
                                    }
                                }

                                if (!varisNoktasiMi)
                                {
                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Seçtiğiniz ağ arayüzünde tanımlanmış akışlarının hiçbirinin varış noktası " + connect.ToConnector.Node.Name + " değildir!";
                                    nfp.Owner = connect.ToConnector.Node.NodesCanvas.MainWindow;
                                    nfp.Show();

                                    NodesCanvas.DraggedConnect.ToConnector = null;
                                }
                                else
                                {
                                    connect.ToConnector.AgAkisList = new List<AgAkis>();
                                    connect.ToConnector.AgAkisList.Clear();
                                    foreach (var agAkis in temp.AgAkisList)
                                    {
                                        if (agAkis.VarisNoktasiIdNameList.Where(x => x.Key == connect.ToConnector.Node.UniqueId).Any())
                                        {
                                            var agAkisTemp = new AgAkis();
                                            agAkisTemp.Id = Guid.NewGuid();
                                            agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                            agAkisTemp.Yuk = agAkis.Yuk;
                                            agAkisTemp.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                                            agAkisTemp.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                                            agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                            agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                            agAkisTemp.VarisNoktasiIdNameList = agAkis.VarisNoktasiIdNameList;

                                            connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                            connect.AgYuku = temp.AgAkisList.Select(x => x.Yuk).Sum();
                                        }
                                    }
                                }
                            }
                        }
                        else if (connect.ToConnector.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                        {
                            if (temp.AgAkisList != null && temp.AgAkisList.Count() != 0)
                            {
                                connect.ToConnector.AgAkisList = new List<AgAkis>();
                                connect.ToConnector.AgAkisList.Clear();
                                foreach (var agAkis in temp.AgAkisList)
                                {
                                    var agAkisTemp = new AgAkis();
                                    agAkisTemp.Id = Guid.NewGuid();
                                    agAkisTemp.AgArayuzuId = connect.ToConnector.UniqueId;
                                    agAkisTemp.Yuk = agAkis.Yuk;
                                    agAkisTemp.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                                    agAkisTemp.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                                    agAkisTemp.AgAkisTipiId = agAkis.AgAkisTipiId;
                                    agAkisTemp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;
                                    agAkisTemp.IliskiliAgArayuzuId = agAkis.IliskiliAgArayuzuId;
                                    agAkisTemp.IliskiliAgArayuzuAdi = agAkis.IliskiliAgArayuzuAdi;
                                    agAkisTemp.VarisNoktasiIdNameList = agAkis.VarisNoktasiIdNameList;

                                    connect.ToConnector.AgAkisList.Add(agAkisTemp);
                                    connect.AgYuku = temp.AgAkisList.Select(x => x.Yuk).Sum();
                                }
                            }
                        }
                    }
                    else if (temp.TypeId == (int)TipEnum.GucUreticiGucArayuzu)
                    {
                        connect.GucMiktari = connect.ToConnector.GirdiTukettigiGucMiktari.HasValue ? connect.ToConnector.GirdiTukettigiGucMiktari.Value : 0;
                    }

                    if (temp.TypeId != (int)TipEnum.Group)
                    {
                        if (NodesCanvas.DraggedConnect.ToConnector != null)
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
                }
                else
                {
                    NodesCanvas.DraggedConnect.ToConnector = null;
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

                        if (this.Node.TypeId != (int)TipEnum.Group && this.Node.TypeId != (int)TipEnum.Group)
                        {
                            if (this.TypeId == (int)TipEnum.UcBirimAgArayuzu)
                            {
                                this.NodesCanvas.MainWindow.IsEnabled = false;
                                System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                                blur.Radius = 2;
                                this.NodesCanvas.MainWindow.Effect = blur;

                                UcBirimAgAkisPopupWindow agAkisiPopup = new UcBirimAgAkisPopupWindow(this);
                                agAkisiPopup.Owner = this.NodesCanvas.MainWindow;
                                agAkisiPopup.ShowDialog();
                            }
                            else if (this.TypeId == (int)TipEnum.AgAnahtariAgArayuzu)
                            {
                                bool isConnectExist = false;
                                foreach (var input in this.Node.InputList)
                                {
                                    foreach (var connect in this.NodesCanvas.Connects)
                                    {
                                        if (connect.ToConnector == input)
                                        {
                                            if (connect.ToConnector.AgAkisList.Count > 0)
                                            {
                                                isConnectExist = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (isConnectExist)
                                        break;
                                }

                                if (isConnectExist)
                                {
                                    this.NodesCanvas.MainWindow.IsEnabled = false;
                                    System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                                    blur.Radius = 2;
                                    this.NodesCanvas.MainWindow.Effect = blur;

                                    AgAnahtariAgAkisPopupWindow agAkisiPopup = new AgAnahtariAgAkisPopupWindow(this);
                                    agAkisiPopup.Owner = this.NodesCanvas.MainWindow;
                                    agAkisiPopup.ShowDialog();
                                }
                                else
                                {
                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Ağ anahtarı için tanımlanmış bir ağ akışı olmadığıdan bu bağlantı için ağ akışı oluşturulamaz.";
                                    nfp.Owner = this.NodesCanvas.MainWindow;
                                    nfp.Show();
                                }
                            }
                        }

                        break;
                    }
            }


        }
    }
}
