﻿using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using AYP.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariAgAkisPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariAgAkisPopupWindow : Window
    {
        IKodListeService kodListeService;

        ConnectorViewModel agAnahtariAgArayuzu;
        AgAkis agAkis;
        private CheckBox checkedAgAkisRow = null;
        private decimal toplam = 0;
        private List<GuidKodListModel> girdiList = new List<GuidKodListModel>();
        public AgAnahtariAgAkisPopupWindow(ConnectorViewModel agAnahtariAgArayuzu)
        {
            this.agAnahtariAgArayuzu = agAnahtariAgArayuzu;
            toplam = this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();

            agAkis = new AgAkis();
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            kodListeService = new KodListeService();
            InitializeComponent();
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            Loaded += Window_Loaded;
        }

        #region WindowLoadedEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in agAnahtariAgArayuzu.AgAkisList)
            {
                item.FromNodeName = this.agAnahtariAgArayuzu.NodesCanvas.Nodes.Items.Where(x => x.UniqueId == item.FromNodeUniqueId).Select(s => s.Name).FirstOrDefault();

                if (item.AgAkisTipiList == null)
                {
                    item.AgAkisTipiList = kodListeService.ListAgAkisTipi();
                }

                if (item.IliskiliAgArayuzuId != null)
                {
                    if (item.IliskiliAgArayuzuAdi == null)
                    {
                        item.IliskiliAgArayuzuAdi = agAnahtariAgArayuzu.Node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu && x.UniqueId == item.IliskiliAgArayuzuId.Value).Select(s => s.Label).FirstOrDefault();
                    }
                }
            }

            if (this.agAnahtariAgArayuzu.AgAkisList.Count > 0)
            {
                AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;
                AgAkisDataGrid.Visibility = Visibility.Visible;
                AgAkisNoDataRow.Visibility = Visibility.Hidden;
            }
            else
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
            SetAgAkisTipiList();
            //SetAgAkisProtokoluList();
            SetAgArayuzuInputList();
        }
        #endregion

        #region PopupCloseEvents
        private void ClosePopup()
        {
            if (this.agAnahtariAgArayuzu.Connect != null)
            {
                this.agAnahtariAgArayuzu.Connect.AgYuku = toplam;
            }

            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonAgAnahtariAgAkisPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        #endregion

        #region NumberValidationEvent
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    if (ch.Equals('.'))
                    {
                        int seperatorCount = textBox.Text.Where(t => t.Equals('.')).Count();

                        if (seperatorCount < 1)
                        {
                            if (textBox.Text.Length > 0)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
        #endregion

        #region GetListEvents

        private void SetAgAkisTipiList()
        {
            agAkis.AgAkisTipiList = kodListeService.ListAgAkisTipi();
            AgAkisTipi.ItemsSource = agAkis.AgAkisTipiList;
            agAkis.AgAkisTipiId = agAkis.AgAkisTipiList[0].Id;
            AgAkisTipi.SelectedItem = agAkis.AgAkisTipiList[0];
        }

        private void SetAgArayuzuInputList()
        {
            agAkis.InputList = agAnahtariAgArayuzu.Node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).ToList();
            GirdiComboBox.ItemsSource = agAkis.InputList;
            agAkis.IliskiliAgArayuzuId = agAkis.InputList[0].UniqueId;
            GirdiComboBox.SelectedItem = agAkis.InputList[0];
        }

        #endregion

        #region TableEvents
        private void ButtonAddAgAkis_Click(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(agAkis, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAkis, validationContext, results, true))
            {
                var inputToplamAgYuku = this.agAnahtariAgArayuzu.Node.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.AgAkisList.Select(k => k.Yuk).Sum()).FirstOrDefault();
                if (inputToplamAgYuku != 0)
                {
                    bool broadcastValid = true;
                    bool unicastValid = true;

                    if ((agAkis.AgAkisTipiId == (int)AgAkisTipEnum.Broadcast || agAkis.AgAkisTipiId == (int)AgAkisTipEnum.Multicast) && agAnahtariAgArayuzu.AgAkisList.Where(x => x.IliskiliAgArayuzuAgAkisId == agAkis.IliskiliAgArayuzuAgAkisId).Any())
                    {
                        broadcastValid = false;
                    }
                    else
                    {
                        decimal unicastTotal = 0;
                        if (agAkis.AgAkisTipiId == (int)AgAkisTipEnum.Unicast)
                        {
                            if(!string.IsNullOrEmpty(Yuk.Text))
                            {
                                agAkis.Yuk = Convert.ToDecimal(Yuk.Text);
                                var definedUnicastTotal = this.agAnahtariAgArayuzu.Node.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.AgAkisList.Where(a => a.Id == agAkis.IliskiliAgArayuzuAgAkisId).Select(k => k.Yuk).Sum()).FirstOrDefault();

                                unicastTotal = agAnahtariAgArayuzu.AgAkisList.Where(x => x.IliskiliAgArayuzuAgAkisId == agAkis.IliskiliAgArayuzuAgAkisId).Select(s => s.Yuk).FirstOrDefault();
                                unicastTotal += agAkis.Yuk;

                                if (unicastTotal > definedUnicastTotal)
                                {
                                    unicastValid = false;
                                }
                            }
                            else
                            {
                                agAkis.Yuk = 0;
                            }
                            
                        }
                    }
                    if(unicastValid && broadcastValid)
                    {
                        decimal total = 0;
                        if (checkedAgAkisRow != null)
                        {
                            var ctx = checkedAgAkisRow.DataContext;
                            var obj = (AgAkis)ctx;

                            total = (this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum() + agAkis.Yuk) - obj.Yuk;

                        }
                        else
                        {
                            total = this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum() + agAkis.Yuk;
                        }

                        if (this.agAnahtariAgArayuzu.MaxKapasite >= total)
                        {
                            toplam = total;
                            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

                            if (checkedAgAkisRow != null)
                            {
                                var ctx = checkedAgAkisRow.DataContext;
                                var obj = (AgAkis)ctx;
                                this.agAnahtariAgArayuzu.AgAkisList.Remove(obj);
                                checkedAgAkisRow = null;
                            }

                            BtnAdd.Text = "Ekle";
                            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

                            AgAkisDataGrid.ItemsSource = null;
                            agAkis.Id = Guid.NewGuid();
                            agAkis.AgAkisTipiAdi = agAkis.AgAkisTipiList.Where(x => x.Id == agAkis.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();
                            agAkis.IliskiliAgArayuzuAdi = agAkis.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.Label).FirstOrDefault();
                            agAkis.FromNodeName = this.agAnahtariAgArayuzu.NodesCanvas.Nodes.Items.Where(x => x.UniqueId == agAkis.FromNodeUniqueId).Select(s => s.Name).FirstOrDefault();

                            this.agAnahtariAgArayuzu.AgAkisList.Add(agAkis);

                            AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;
                            AgAkisDataGrid.Visibility = Visibility.Visible;
                            AgAkisNoDataRow.Visibility = Visibility.Hidden;

                            agAkis = new AgAkis();
                            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
                            GirdiComboBox.ItemsSource = null;
                            GelenAgAkisComboBox.ItemsSource = null;
                            AgAkisTipi.ItemsSource = null;
                            SetAgAkisTipiList();
                            SetAgArayuzuInputList();
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Ağın iletebileceği yük kapasitesi " + this.agAnahtariAgArayuzu.MinKapasite + " - " + this.agAnahtariAgArayuzu.MaxKapasite + " Mbps arasında olmalıdır!";
                            nfp.Owner = Owner;
                            nfp.Show();
                        }
                    }
                    else
                    {
                        if (!broadcastValid)
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Seçtiğiniz ağ akışı için daha önce kayıt yapılmıştır!";
                            nfp.Owner = Owner;
                            nfp.Show();
                        }
                        
                        if(!unicastValid)
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Unicast ağ akışının toplam kapasitesini aştınız!";
                            nfp.Owner = Owner;
                            nfp.Show();
                        }
                    }
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Seçtiğiniz girdi için ağ akışı tanımlaması yapılmadığı için, bu girdi için ağ akışı tanımlanamz.";
                    nfp.Owner = Owner;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "Yuk")
                        {
                            Yuk.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }
        private void AgAkisRow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkedAgAkisRow != null)
            {
                checkedAgAkisRow.IsChecked = false;
            }

            BtnAdd.Text = "Güncelle";
            BtnAdd.Margin = new Thickness(80, 0, 0, 0);

            checkedAgAkisRow = (CheckBox)sender;
            var ctx = checkedAgAkisRow.DataContext;
            agAkis = new AgAkis();
            agAkis = (AgAkis)((AgAkis)ctx).Clone();

            agAkis.InputList = agAnahtariAgArayuzu.Node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).ToList();
            GirdiComboBox.ItemsSource = agAkis.InputList;
            GirdiComboBox.SelectedItem = agAkis.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).FirstOrDefault();

            GelenAgAkisComboBox.SelectedItem = girdiList.Where(o => o.Id == agAnahtariAgArayuzu.AgAkisList.Where(x => x.Id == agAkis.Id).Select(s => s.IliskiliAgArayuzuAgAkisId).FirstOrDefault()).FirstOrDefault();

            agAkis.AgAkisTipiList = kodListeService.ListAgAkisTipi();
            AgAkisTipi.ItemsSource = agAkis.AgAkisTipiList;
            AgAkisTipi.SelectedItem = agAkis.AgAkisTipiList.Where(x => x.Id == agAkis.AgAkisTipiId).FirstOrDefault();

            Yuk.Text = agAkis.Yuk.ToString();
            foreach (var varisNoktasiIdName in agAnahtariAgArayuzu.AgAkisList.Where(x => x.Id == agAkis.Id).Select(s => s.VarisNoktasiIdNameList).FirstOrDefault())
            {
                agAkis.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
            }
            agAkis.Id = Guid.Empty;
        }

        private void AgAkisRow_Unchecked(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            checkedAgAkisRow = null;
            agAkis = new AgAkis();
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;

            GirdiComboBox.ItemsSource = null;
            GelenAgAkisComboBox.ItemsSource = null;
            AgAkisTipi.ItemsSource = null;
            SetAgAkisTipiList();
            SetAgArayuzuInputList();
        }

        private void AgAkisDelete_AllRows(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            AgAkisDataGrid.ItemsSource = null;
            this.agAnahtariAgArayuzu.AgAkisList.Clear();
            toplam = 0;
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;

            if (this.agAnahtariAgArayuzu.AgAkisList.Count == 0)
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            checkedAgAkisRow = null;
            agAkis = new AgAkis();
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
            GirdiComboBox.ItemsSource = null;
            GelenAgAkisComboBox.ItemsSource = null;
            AgAkisTipi.ItemsSource = null;

            SetAgAkisTipiList();
            SetAgArayuzuInputList();
        }

        private void AgAkisDelete_Row(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            var row = (Button)sender;
            var ctx = row.DataContext;
            var obj = (AgAkis)ctx;

            AgAkisDataGrid.ItemsSource = null;
            this.agAnahtariAgArayuzu.AgAkisList.Remove(obj);
            toplam = this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;

            if (this.agAnahtariAgArayuzu.AgAkisList.Count == 0)
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            checkedAgAkisRow = null;
            agAkis = new AgAkis();
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
            GirdiComboBox.ItemsSource = null;
            GelenAgAkisComboBox.ItemsSource = null;
            AgAkisTipi.ItemsSource = null;
            SetAgAkisTipiList();
            SetAgArayuzuInputList();
        }
        #endregion

        #region SelectionChangedEvents
        private void IliskiliGirdi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GirdiComboBox.ItemsSource != null)
            {
                agAkis.IliskiliAgArayuzuId = ((ConnectorViewModel)GirdiComboBox.SelectedItem).UniqueId;
                GelenAgAkisComboBox.ItemsSource = null;
                girdiList.Clear();
                var agAkisList = this.agAnahtariAgArayuzu.Node.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.AgAkisList).FirstOrDefault();

                if (agAkisList != null && agAkisList.Count()>0)
                {
                    agAkis.FromNodeUniqueId = agAkisList.First().FromNodeUniqueId;

                    foreach (var agAkisItem in agAkisList)
                    {

                        if (!((agAkisItem.AgAkisTipiId == (int)AgAkisTipEnum.Broadcast || agAkisItem.AgAkisTipiId == (int)AgAkisTipEnum.Multicast) && agAnahtariAgArayuzu.AgAkisList.Where(x => x.IliskiliAgArayuzuAgAkisId == agAkisItem.Id).Any()))
                        {
                            girdiList.Add(new GuidKodListModel
                            {
                                Ad = this.agAnahtariAgArayuzu.NodesCanvas.Nodes.Items.Where(x => x.UniqueId == agAkisItem.FromNodeUniqueId).Select(s => s.Name).FirstOrDefault() + " " + agAkisItem.AgAkisTipiAdi + " " + agAkisItem.Yuk + " mbps",
                                Id = agAkisItem.Id
                            });
                        }
                    }
                }
                else
                {
                    agAkis.FromNodeUniqueId = null;
                }

                GelenAgAkisComboBox.ItemsSource = girdiList;

                if (girdiList.Count > 0)
                {
                    GelenAgAkisComboBox.SelectedItem = girdiList.First();
                    agAkis.IliskiliAgArayuzuAgAkisId = girdiList.First().Id;
                }
                else
                {

                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Seçtiğiniz ağ arayüzü girdisine ait tanımlanmış bir ağ akışı bulunmamaktadır!";
                    nfp.Owner = Owner;
                    nfp.Show();

                    GelenAgAkisComboBox.SelectedItem = null;
                }
            }
        }

        private void GelenAgAkisi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GelenAgAkisComboBox.ItemsSource != null)
            {
                if (GelenAgAkisComboBox.SelectedItem != null)
                {
                    var selection = ((GuidKodListModel)GelenAgAkisComboBox.SelectedItem).Id;

                    var agAkisList = this.agAnahtariAgArayuzu.Node.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.AgAkisList).FirstOrDefault();
                    var selectedAgAkis = agAkisList.Where(x => x.Id == selection).FirstOrDefault();

                    AgAkisTipi.ItemsSource = agAkis.AgAkisTipiList;
                    agAkis.AgAkisTipiId = selectedAgAkis.AgAkisTipiId;
                    AgAkisTipi.SelectedItem = agAkis.AgAkisTipiList.Where(x => x.Id == selectedAgAkis.AgAkisTipiId).FirstOrDefault();

                    agAkis.Yuk = selectedAgAkis.Yuk;
                    Yuk.Text = selectedAgAkis.Yuk.ToString();

                    foreach (var varisNoktasiIdName in selectedAgAkis.VarisNoktasiIdNameList)
                    {
                        agAkis.VarisNoktasiIdNameList.Add(varisNoktasiIdName);
                    }

                    agAkis.IliskiliAgArayuzuAgAkisId = selectedAgAkis.Id;

                    if (selectedAgAkis.AgAkisTipiId == (int)AgAkisTipEnum.Unicast)
                    {
                        Yuk.IsEnabled = true;
                        Yuk.Opacity = 1;
                        AgAkisTipi.IsEnabled = false;
                        AgAkisTipi.Opacity = 0.25;
                    }
                    else
                    {
                        Yuk.IsEnabled = false;
                        Yuk.Opacity = 0.25;
                        AgAkisTipi.IsEnabled = false;
                        AgAkisTipi.Opacity = 0.25;
                    }
                }
                else
                {
                    agAkis.AgAkisTipiId = agAkis.AgAkisTipiList[0].Id;
                    AgAkisTipi.SelectedItem = agAkis.AgAkisTipiList[0];
                    agAkis.Yuk = 0;
                    Yuk.Text = "0";
                    agAkis.VarisNoktasiIdNameList.Clear();

                    Yuk.IsEnabled = false;
                    Yuk.Opacity = 0.25;
                    AgAkisTipi.IsEnabled = false;
                    AgAkisTipi.Opacity = 0.25;
                }
            }
            else
            {
                agAkis.Yuk = 0;
                Yuk.Text = "0";
                agAkis.VarisNoktasiIdNameList.Clear();

                Yuk.IsEnabled = false;
                Yuk.Opacity = 0.25;
                AgAkisTipi.IsEnabled = false;
                AgAkisTipi.Opacity = 0.25;
            }
        }
        #endregion

    }
}
