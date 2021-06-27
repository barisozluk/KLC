using AYP.DbContext.AYP.DbContexts;
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
    /// Interaction logic for UcBirimAgAkisPopupWindow.xaml
    /// </summary>
    public partial class UcBirimAgAkisPopupWindow : Window
    {
        IKodListeService kodListeService;

        ConnectorViewModel ucBirimAgArayuzu;
        AgAkis agAkis;
        private CheckBox checkedAgAkisRow = null;
        private decimal toplam = 0;

        public UcBirimAgAkisPopupWindow(ConnectorViewModel ucBirimAgArayuzu)
        {
            this.ucBirimAgArayuzu = ucBirimAgArayuzu;
            toplam = this.ucBirimAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();

            agAkis = new AgAkis();
            kodListeService = new KodListeService();
            InitializeComponent();

            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            if (this.ucBirimAgArayuzu.AgAkisList.Count > 0)
            {
                AgAkisDataGrid.ItemsSource = this.ucBirimAgArayuzu.AgAkisList;
                AgAkisDataGrid.Visibility = Visibility.Visible;
                AgAkisNoDataRow.Visibility = Visibility.Hidden;
            }
            else
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            foreach(var item in ucBirimAgArayuzu.AgAkisList)
            {
                if(item.VarisNoktasiList == null)
                {
                    item.VarisNoktasiList = this.ucBirimAgArayuzu.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim && x.UniqueId != this.ucBirimAgArayuzu.Node.UniqueId).ToList();
                }

                if (item.AgAkisTipiList == null)
                {
                    item.AgAkisTipiList = kodListeService.ListAgAkisTipi();
                }
            }

            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            SetAgAkisTipiList();
            SetVarisNoktasiList();
            DataContext = agAkis;

            Loaded += Window_Loaded;
        }

        #region WindowLoadedEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //RemoveFromDogrulamaPaneli();
        }
        #endregion

        #region PopupCloseEvents
        private void ClosePopup()
        {
            if (this.ucBirimAgArayuzu.Connect != null)
            {
                this.ucBirimAgArayuzu.Connect.AgYuku = toplam;
            }

            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonUcBirimAgAkisPopupClose_Click(object sender, RoutedEventArgs e)
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
            agAkis.AgAkisTipiId = agAkis.AgAkisTipiList[0].Id;
        }

        private void SetVarisNoktasiList()
        {
            var ucBirimList = this.ucBirimAgArayuzu.NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim && x.UniqueId != this.ucBirimAgArayuzu.Node.UniqueId).ToList();
            agAkis.VarisNoktasiList = ucBirimList.Where(y => y.InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count() > 0).ToList();

            UcBirimListBox.ItemsSource = agAkis.VarisNoktasiList;
        }
        #endregion

        #region TableEvents
        private void ButtonAddAgAkis_Click(object sender, RoutedEventArgs e)
        {
            List<NodeViewModel> selectedItems = new List<NodeViewModel>();
            if (UcBirimListBox.SelectionMode == SelectionMode.Multiple)
            {
                selectedItems = UcBirimListBox.SelectedItems.Cast<NodeViewModel>().ToList();
            }
            else if (UcBirimListBox.SelectionMode == SelectionMode.Single)
            {
                if(UcBirimListBox.SelectedItem != null)
                {
                    selectedItems.Add((NodeViewModel)UcBirimListBox.SelectedItem);
                }
                
            }

            var validationContext = new ValidationContext(agAkis, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAkis, validationContext, results, true))
            {
                if (selectedItems.Count() > 0)
                {
                    decimal total = 0;
                    if (checkedAgAkisRow != null)
                    {
                        var ctx = checkedAgAkisRow.DataContext;
                        var obj = (AgAkis)ctx;

                        total = (this.ucBirimAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum() + agAkis.Yuk) - obj.Yuk;

                    }
                    else
                    {
                        total = this.ucBirimAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum() + agAkis.Yuk;
                    }

                    if (this.ucBirimAgArayuzu.MaxKapasite >= total)
                    {
                        toplam = total;
                        MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

                        if (checkedAgAkisRow != null)
                        {
                            var ctx = checkedAgAkisRow.DataContext;
                            var obj = (AgAkis)ctx;
                            this.ucBirimAgArayuzu.AgAkisList.Remove(obj);
                            checkedAgAkisRow = null;
                        }

                        BtnAdd.Text = "Ekle";
                        BtnAdd.Margin = new Thickness(100, 0, 0, 0);

                        AgAkisDataGrid.ItemsSource = null;
                        agAkis.Id = Guid.NewGuid();
                        agAkis.AgAkisTipiAdi = agAkis.AgAkisTipiList.Where(x => x.Id == agAkis.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();

                        foreach (var selectedItem in selectedItems)
                        {
                            var keyValue = new KeyValuePair<Guid, string>(selectedItem.UniqueId, agAkis.VarisNoktasiList.Where(x => x.UniqueId == selectedItem.UniqueId).Select(s => s.Name).FirstOrDefault());
                            agAkis.VarisNoktasiIdNameList.Add(keyValue);
                        }

                        this.ucBirimAgArayuzu.AgAkisList.Add(agAkis);
                        ClearSelections();

                        AgAkisDataGrid.ItemsSource = this.ucBirimAgArayuzu.AgAkisList;
                        AgAkisDataGrid.Visibility = Visibility.Visible;
                        AgAkisNoDataRow.Visibility = Visibility.Hidden;

                        DataContext = null;
                        agAkis = new AgAkis();
                        agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
                        agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
                        SetAgAkisTipiList();
                        SetVarisNoktasiList();
                        DataContext = agAkis;
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Ağın iletebileceği yük kapasitesini (" + this.ucBirimAgArayuzu.MinKapasite + " - " + this.ucBirimAgArayuzu.MaxKapasite + " Mbps) aştınız!";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Varış noktası ekleyiniz!";
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
            BtnAdd.Text = "Güncelle";
            BtnAdd.Margin = new Thickness(80, 0, 0, 0);

            if (checkedAgAkisRow != null)
            {
                checkedAgAkisRow.IsChecked = false;
            }

            DataContext = null;
            checkedAgAkisRow = (CheckBox)sender;
            var ctx = checkedAgAkisRow.DataContext;
            agAkis = (AgAkis)((AgAkis)ctx).Clone();
            agAkis.Id = Guid.Empty;

            ClearSelections();

            DataContext = agAkis;
            foreach (var item in agAkis.VarisNoktasiIdNameList)
            {
                if (UcBirimListBox.SelectionMode == SelectionMode.Single)
                {
                    UcBirimListBox.SelectedItem = agAkis.VarisNoktasiList.Where(x => x.UniqueId == item.Key).FirstOrDefault();
                }
                else if (UcBirimListBox.SelectionMode == SelectionMode.Multiple)
                {
                    UcBirimListBox.SelectedItems.Add(agAkis.VarisNoktasiList.Where(x => x.UniqueId == item.Key).FirstOrDefault());
                }
            }
        }

        private void AgAkisRow_Unchecked(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            checkedAgAkisRow = null;
            DataContext = null;
            agAkis = new AgAkis();
            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            SetAgAkisTipiList();
            SetVarisNoktasiList();
            ClearSelections();
            DataContext = agAkis;
        }

        private void AgAkisDelete_AllRows(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            AgAkisDataGrid.ItemsSource = null;
            this.ucBirimAgArayuzu.AgAkisList.Clear();
            toplam = 0;
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            AgAkisDataGrid.ItemsSource = this.ucBirimAgArayuzu.AgAkisList;

            if (this.ucBirimAgArayuzu.AgAkisList.Count == 0)
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            checkedAgAkisRow = null;
            DataContext = null;
            agAkis = new AgAkis();
            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            SetAgAkisTipiList();
            SetVarisNoktasiList();
            ClearSelections();
            DataContext = agAkis;
        }

        private void AgAkisDelete_Row(object sender, RoutedEventArgs e)
        {
            BtnAdd.Text = "Ekle";
            BtnAdd.Margin = new Thickness(100, 0, 0, 0);

            var row = (Button)sender;
            var ctx = row.DataContext;
            var obj = (AgAkis)ctx;

            AgAkisDataGrid.ItemsSource = null;
            this.ucBirimAgArayuzu.AgAkisList.Remove(obj);
            toplam = this.ucBirimAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            AgAkisDataGrid.ItemsSource = this.ucBirimAgArayuzu.AgAkisList;

            if (this.ucBirimAgArayuzu.AgAkisList.Count == 0)
            {
                AgAkisDataGrid.Visibility = Visibility.Hidden;
                AgAkisNoDataRow.Visibility = Visibility.Visible;
            }

            checkedAgAkisRow = null;
            DataContext = null;
            agAkis = new AgAkis();
            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            agAkis.VarisNoktasiIdNameList = new List<KeyValuePair<Guid, string>>();
            SetAgAkisTipiList();
            SetVarisNoktasiList();
            ClearSelections();
            DataContext = agAkis;
        }
        #endregion

        #region AgAkisTipiChangedEvents
        private void AgAkisTipi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearSelections();

            if (agAkis.AgAkisTipiId == (int)AgAkisTipEnum.Unicast)
            {
                UcBirimListBox.SelectionMode = SelectionMode.Single;
            }
            else
            {
                UcBirimListBox.SelectionMode = SelectionMode.Multiple;
            }
        }
        #endregion

        #region ClearVarisNoktasiSelectionClear
        private void ClearSelections()
        {
            if (UcBirimListBox.SelectionMode == SelectionMode.Multiple)
            {
                UcBirimListBox.SelectedItems.Clear();
            }
            else if(UcBirimListBox.SelectionMode == SelectionMode.Single)
            {
                UcBirimListBox.SelectedItem = null;
            }
        }
        #endregion
    }
}
