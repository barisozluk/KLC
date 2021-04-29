using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
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
        private AYPContext context;

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
            this.context = new AYPContext();
            kodListeService = new KodListeService(context);
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

            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            SetAgAkisTipiList();
            SetAgAkisProtokoluList();
            DataContext = agAkis;

            Loaded += Window_Loaded;
        }

        #region WindowLoadedEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RemoveFromDogrulamaPaneli();
        }
        #endregion

        #region PopupCloseEvents
        private void ClosePopup()
        {
            this.ucBirimAgArayuzu.Connect.AgYuku = toplam;

            if (toplam == 0)
            {
                AddToDogrulamaPaneli(ucBirimAgArayuzu.Node.Name + "/" + ucBirimAgArayuzu.Label + " için ağ akışı tanımlayınız!");
            }

            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void AddToDogrulamaPaneli(string mesaj)
        {
            DogrulamaModel dogrulama = new DogrulamaModel();
            dogrulama.Mesaj = mesaj;
            dogrulama.MesajTipi = "AgAkis";
            dogrulama.Connector = ucBirimAgArayuzu;
            (Owner as MainWindow).DogrulamaDataGrid.Items.Add(dogrulama);
        }

        private void RemoveFromDogrulamaPaneli()
        {
            if ((Owner as MainWindow).DogrulamaDataGrid.Items.Count > 0)
            {
                DogrulamaModel deletedObj = null;

                foreach (var item in (Owner as MainWindow).DogrulamaDataGrid.Items)
                {
                    if ((item as DogrulamaModel).Connector == ucBirimAgArayuzu || (item as DogrulamaModel).MesajTipi == "AgAkis")
                    {
                        deletedObj = (item as DogrulamaModel);
                        break;
                    }
                }

                (Owner as MainWindow).DogrulamaDataGrid.Items.Remove(deletedObj);
            }
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

        private void SetAgAkisProtokoluList()
        {
            agAkis.AgAkisProtokoluList = kodListeService.ListAgAkisProtokolu();
            agAkis.AgAkisProtokoluId = agAkis.AgAkisProtokoluList[0].Id;
        }
        #endregion

        #region TableEvents
        private void ButtonAddAgAkis_Click(object sender, RoutedEventArgs e)
        {
            var validationContext = new ValidationContext(agAkis, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAkis, validationContext, results, true))
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

                    AgAkisDataGrid.ItemsSource = null;
                    agAkis.Id = Guid.NewGuid();
                    agAkis.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluList.Where(x => x.Id == agAkis.AgAkisProtokoluId).Select(s => s.Ad).FirstOrDefault();
                    agAkis.AgAkisTipiAdi = agAkis.AgAkisTipiList.Where(x => x.Id == agAkis.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();

                    this.ucBirimAgArayuzu.AgAkisList.Add(agAkis);
                    SetToConnectorAgAkis();

                    AgAkisDataGrid.ItemsSource = this.ucBirimAgArayuzu.AgAkisList;
                    AgAkisDataGrid.Visibility = Visibility.Visible;
                    AgAkisNoDataRow.Visibility = Visibility.Hidden;

                    DataContext = null;
                    agAkis = new AgAkis();
                    agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
                    SetAgAkisProtokoluList();
                    SetAgAkisTipiList();
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

            DataContext = null;
            checkedAgAkisRow = (CheckBox)sender;
            var ctx = checkedAgAkisRow.DataContext;
            agAkis = (AgAkis)ctx;
            agAkis.Id = Guid.Empty;
            DataContext = agAkis;
        }

        private void AgAkisRow_Unchecked(object sender, RoutedEventArgs e)
        {
            checkedAgAkisRow = null;
            DataContext = null;
            agAkis = new AgAkis();
            agAkis.AgArayuzuId = this.ucBirimAgArayuzu.UniqueId;
            SetAgAkisTipiList();
            SetAgAkisProtokoluList();
            DataContext = agAkis;
        }

        private void AgAkisDelete_Row(object sender, RoutedEventArgs e)
        {

            var row = (Button)sender;
            var ctx = row.DataContext;
            var obj = (AgAkis)ctx;

            AgAkisDataGrid.ItemsSource = null;
            this.ucBirimAgArayuzu.AgAkisList.Remove(obj);
            toplam = this.ucBirimAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();
            MainTitle.Content = "Ağ Akışı - " + toplam.ToString("0.##") + " Mbps";

            SetToConnectorAgAkis();
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
            SetAgAkisTipiList();
            SetAgAkisProtokoluList();
            DataContext = agAkis;
        }
        #endregion

        private void SetToConnectorAgAkis()
        {
            this.ucBirimAgArayuzu.Connect.ToConnector.AgAkisList = new List<AgAkis>();
            this.ucBirimAgArayuzu.Connect.ToConnector.AgAkisList.Clear();
            foreach (var agAkis in this.ucBirimAgArayuzu.AgAkisList)
            {
                var temp = new AgAkis();
                temp.Id = Guid.NewGuid();
                temp.AgArayuzuId = this.ucBirimAgArayuzu.Connect.ToConnector.UniqueId;
                temp.Yuk = agAkis.Yuk;
                temp.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                temp.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluAdi;
                temp.AgAkisTipiId = agAkis.AgAkisTipiId;
                temp.AgAkisTipiAdi = agAkis.AgAkisTipiAdi;

                this.ucBirimAgArayuzu.Connect.ToConnector.AgAkisList.Add(temp);
            }
        }
    }
}
