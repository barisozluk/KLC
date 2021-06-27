using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Extensions;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using AYP.ViewModel;
using AYP.ViewModel.Node;
using DynamicData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace AYP
{
    /// <summary>
    /// Interaction logic for UcBirimTurPopupWindow.xaml
    /// </summary>
    public partial class GrupArayuzOlusturPopupWindow : Window
    {
        NodeViewModel node;
        GroupUngroupModel group;
        public GrupArayuzOlusturPopupWindow(NodeViewModel node)
        {
            this.node = node;
            InitializeComponent();
            group = node.NodesCanvas.GroupList.Where(x => x.UniqueId == node.UniqueId).FirstOrDefault();
            NodeComboBox.ItemsSource = group.NodeList;
            NodeComboBox.SelectedItem = group.NodeList.First();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();

        }

        private void Save_Popup(object sender, RoutedEventArgs e)
        {
            var selectedAgArayuzInputItems = new List<ConnectorViewModel>();
            var selectedGucArayuzInputItems = new List<ConnectorViewModel>();
            var selectedAgArayuzOutputItems = new List<ConnectorViewModel>();
            var selectedGucArayuzOutputItems = new List<ConnectorViewModel>();

            selectedAgArayuzInputItems.AddRange(GirdiAgArayuzListBox.SelectedItems.Cast<ConnectorViewModel>());
            selectedGucArayuzInputItems.AddRange(GirdiGucArayuzListBox.SelectedItems.Cast<ConnectorViewModel>());
            selectedAgArayuzOutputItems.AddRange(CiktiAgArayuzListBox.SelectedItems.Cast<ConnectorViewModel>());
            selectedGucArayuzOutputItems.AddRange(CiktiGucArayuzListBox.SelectedItems.Cast<ConnectorViewModel>());

            if(selectedAgArayuzInputItems.Count > 0 || selectedGucArayuzInputItems.Count > 0 || selectedAgArayuzOutputItems.Count > 0 || selectedGucArayuzOutputItems.Count > 0)
            {
                var agInputs = this.node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu).ToList();
                agInputs = agInputs.Union(selectedAgArayuzInputItems).ToList();

                var gucInputs = this.node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).ToList();
                gucInputs = gucInputs.Union(selectedGucArayuzInputItems).ToList();

                var agOutputs = this.node.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu).ToList();
                agOutputs = agOutputs.Union(selectedAgArayuzOutputItems).ToList();

                var gucOutputs = this.node.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).ToList();
                gucOutputs = gucOutputs.Union(selectedGucArayuzOutputItems).ToList();

                this.node.InputList = new List<ConnectorViewModel>();
                this.node.OutputList = new List<ConnectorViewModel>();
                this.node.Transitions = new SourceList<ConnectorViewModel>();

                NodeViewModel newNode = new NodeViewModel(this.node.NodesCanvas, this.node.Name, this.node.UniqueId, new Point(), 0, 9);

                var inputList = new List<ConnectorViewModel>();
                var outputList = new List<ConnectorViewModel>();
                var connctesToDelete = new List<ConnectViewModel>();
                var connctesToAdd = new List<ConnectViewModel>();

                int i = 0;
                foreach (var input in agInputs)
                {
                    var inputMain = new ConnectorViewModel(input.NodesCanvas, newNode, input.Name, newNode.Point1.Addition(0, 30 + (i * 20)), input.UniqueId, input.KapasiteId, input.MinKapasite, input.MaxKapasite, input.FizikselOrtamId, null, input.KullanimAmaciId, null, null, null, null, null, null, null, null, input.Label, input.TypeId, input.Id, input.Port, null);
                    inputMain.AgAkisList = input.AgAkisList;
                    inputList.Add(inputMain);
                    i++;

                    var oldConnect = this.node.NodesCanvas.Connects.Where(x => x.ToConnector == input).FirstOrDefault();
                    if (oldConnect != null)
                    {
                        connctesToDelete.Add(oldConnect);
                        ConnectViewModel newConnect = new ConnectViewModel(this.node.NodesCanvas, oldConnect.FromConnector);
                        newConnect.AgYuku = oldConnect.AgYuku;
                        newConnect.KabloKesitOnerisi = oldConnect.KabloKesitOnerisi;
                        newConnect.GucMiktari = oldConnect.GucMiktari;
                        newConnect.Uzunluk = oldConnect.Uzunluk;
                        newConnect.ToConnector = inputMain;
                        connctesToAdd.Add(newConnect);
                    }
                }

                int count = i;
                i = 0;
                foreach (var input in gucInputs)
                {
                    var inputMain = new ConnectorViewModel(input.NodesCanvas, newNode, input.Name, newNode.Point1.Addition(0, 30 + ((i + count) * 20)), input.UniqueId, null, null, null, null, input.GerilimTipiId, input.KullanimAmaciId,
                                    input.GirdiDuraganGerilimDegeri1, input.GirdiDuraganGerilimDegeri2, input.GirdiDuraganGerilimDegeri3, input.GirdiMinimumGerilimDegeri, input.GirdiMaksimumGerilimDegeri, input.GirdiTukettigiGucMiktari,
                                    input.CiktiDuraganGerilimDegeri, input.CiktiUrettigiGucKapasitesi, input.Label, input.TypeId, input.Id, input.Port, null);

                    inputList.Add(inputMain);
                    i++;

                    var oldConnect = this.node.NodesCanvas.Connects.Where(x => x.ToConnector == input).FirstOrDefault();
                    if (oldConnect != null)
                    {
                        connctesToDelete.Add(oldConnect);
                        ConnectViewModel newConnect = new ConnectViewModel(this.node.NodesCanvas, oldConnect.FromConnector);
                        newConnect.AgYuku = oldConnect.AgYuku;
                        newConnect.KabloKesitOnerisi = oldConnect.KabloKesitOnerisi;
                        newConnect.GucMiktari = oldConnect.GucMiktari;
                        newConnect.Uzunluk = oldConnect.Uzunluk;
                        newConnect.ToConnector = inputMain;
                        connctesToAdd.Add(newConnect);
                    }
                }

                i = 0;
                foreach (var output in agOutputs)
                {
                    var outputMain = new ConnectorViewModel(output.NodesCanvas, newNode, output.Name, newNode.Point1.Addition(80, 54 + (i * 20)), output.UniqueId, output.KapasiteId, output.MinKapasite, output.MaxKapasite, output.FizikselOrtamId, null, output.KullanimAmaciId, null, null, null, null, null, null, null, null, output.Label, output.TypeId, output.Id, output.Port)
                    {
                        Visible = null
                    };

                    outputMain.AgAkisList = output.AgAkisList;
                    outputMain.KalanKapasite = output.KalanKapasite;
                    i++;
                    outputList.Add(outputMain);

                    var oldConnect = this.node.NodesCanvas.Connects.Where(x => x.FromConnector == output).FirstOrDefault();
                    if (oldConnect != null)
                    {
                        connctesToDelete.Add(oldConnect);
                        ConnectViewModel newConnect = new ConnectViewModel(this.node.NodesCanvas, outputMain);
                        newConnect.AgYuku = oldConnect.AgYuku;
                        newConnect.KabloKesitOnerisi = oldConnect.KabloKesitOnerisi;
                        newConnect.GucMiktari = oldConnect.GucMiktari;
                        newConnect.Uzunluk = oldConnect.Uzunluk;
                        newConnect.ToConnector = oldConnect.ToConnector;
                        connctesToAdd.Add(newConnect);
                    }
                }

                count = i;
                i = 0;
                foreach (var output in gucOutputs)
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

                    var oldConnect = this.node.NodesCanvas.Connects.Where(x => x.FromConnector == output).FirstOrDefault();
                    if (oldConnect != null)
                    {
                        connctesToDelete.Add(oldConnect);
                        ConnectViewModel newConnect = new ConnectViewModel(this.node.NodesCanvas, outputMain);
                        newConnect.AgYuku = oldConnect.AgYuku;
                        newConnect.KabloKesitOnerisi = oldConnect.KabloKesitOnerisi;
                        newConnect.GucMiktari = oldConnect.GucMiktari;
                        newConnect.Uzunluk = oldConnect.Uzunluk;
                        newConnect.ToConnector = oldConnect.ToConnector;
                        connctesToAdd.Add(newConnect);
                    }
                }

                this.node.NodesCanvas.Connects.Remove(connctesToDelete);
                this.node.NodesCanvas.Connects.Add(connctesToAdd);

                newNode.InputList = inputList;
                newNode.OutputList = outputList;
                newNode.AddEmptyConnector();

                this.node.NodesCanvas.Nodes.Remove(this.node);
                this.node.NodesCanvas.Nodes.Add(newNode);
                this.group.InputList = newNode.InputList;
                this.group.OutputList = newNode.OutputList;
                ClosePopup();
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir arayüz seçiniz.";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }

        #region SelectionChangedEvents
        private void Node_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GirdiAgArayuzListBox.ItemsSource = null;
            GirdiGucArayuzListBox.ItemsSource = null;
            CiktiAgArayuzListBox.ItemsSource = null;
            CiktiGucArayuzListBox.ItemsSource = null;

            var groupedNode = ((NodeViewModel)NodeComboBox.SelectedItem);

            var agInputs = new List<ConnectorViewModel>();
            foreach(var input in groupedNode.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu).ToList())
            {
                if(!this.group.ExternalConnectList.Where(x => x.FromConnector == input || x.ToConnector == input).Any())
                {
                    if (!this.group.InternalConnectList.Where(x => x.FromConnector == input || x.ToConnector == input).Any())
                    {
                        agInputs.Add(input);
                    }
                }
            }
            GirdiAgArayuzListBox.ItemsSource = agInputs;


            var gucInputs = new List<ConnectorViewModel>();
            foreach (var input in groupedNode.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).ToList())
            {
                if (!this.group.ExternalConnectList.Where(x => x.FromConnector == input || x.ToConnector == input).Any())
                {
                    if (!this.group.InternalConnectList.Where(x => x.FromConnector == input || x.ToConnector == input).Any())
                    {
                        gucInputs.Add(input);
                    }
                }
            }
            GirdiGucArayuzListBox.ItemsSource = gucInputs;

            var agOutputs = new List<ConnectorViewModel>();
            foreach (var output in groupedNode.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu || x.TypeId == (int)TipEnum.UcBirimAgArayuzu).ToList())
            {
                if (!this.group.ExternalConnectList.Where(x => x.FromConnector == output || x.ToConnector == output).Any())
                {
                    if (!this.group.InternalConnectList.Where(x => x.FromConnector == output || x.ToConnector == output).Any())
                    {
                        agOutputs.Add(output);
                    }
                }
            }
            CiktiAgArayuzListBox.ItemsSource = agOutputs;

            var gucOutputs = new List<ConnectorViewModel>();
            foreach (var output in groupedNode.Transitions.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).ToList())
            {
                if (!this.group.ExternalConnectList.Where(x => x.FromConnector == output || x.ToConnector == output).Any())
                {
                    if (!this.group.InternalConnectList.Where(x => x.FromConnector == output || x.ToConnector == output).Any())
                    {
                        gucOutputs.Add(output);
                    }
                }
            }
            CiktiGucArayuzListBox.ItemsSource = gucOutputs;
        }
        #endregion
    }
}
