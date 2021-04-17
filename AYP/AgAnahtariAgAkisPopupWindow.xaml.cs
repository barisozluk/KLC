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
    /// Interaction logic for AgAnahtariAgAkisPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariAgAkisPopupWindow : Window
    {
        private AYPContext context;

        IKodListeService kodListeService;

        ConnectorViewModel agAnahtariAgArayuzu;
        AgAkis agAkis;
        private CheckBox checkedAgAkisRow = null;
        private decimal toplam = 0;

        public AgAnahtariAgAkisPopupWindow(ConnectorViewModel agAnahtariAgArayuzu)
        {
            this.agAnahtariAgArayuzu = agAnahtariAgArayuzu;
            toplam = this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();

            agAkis = new AgAkis();
            this.context = new AYPContext();
            kodListeService = new KodListeService(context);
            InitializeComponent();
            MainTitle.Content = "Ağ Akışı - " + toplam + " Mbps";

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
            SetAgArayuzuInputList();
            SetAgAkisTipiList();
            SetAgAkisProtokoluList();
            DataContext = agAkis;
        }

        #region PopupCloseEvents
        private void ClosePopup()
        {
            this.agAnahtariAgArayuzu.Connect.AgYuku = toplam;

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
            agAkis.AgAkisTipiId = agAkis.AgAkisTipiList[0].Id;
        }

        private void SetAgAkisProtokoluList()
        {
            agAkis.AgAkisProtokoluList = kodListeService.ListAgAkisProtokolu();
            agAkis.AgAkisProtokoluId = agAkis.AgAkisProtokoluList[0].Id;
        }
        private void SetAgArayuzuInputList()
        {
            agAkis.InputList = agAnahtariAgArayuzu.Node.InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).ToList(); 
            agAkis.IliskiliAgArayuzuId = agAkis.InputList[0].UniqueId;
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
                    decimal inputToplamAgAkisi = 0;
                    if (checkedAgAkisRow != null)
                    {
                        var ctx = checkedAgAkisRow.DataContext;
                        var obj = (AgAkis)ctx;

                        inputToplamAgAkisi = (this.agAnahtariAgArayuzu.AgAkisList.Where(x => x.IliskiliAgArayuzuId == agAkis.IliskiliAgArayuzuId).Select(s => s.Yuk).Sum() + agAkis.Yuk) - obj.Yuk;
                    }
                    else
                    {
                        inputToplamAgAkisi = this.agAnahtariAgArayuzu.AgAkisList.Where(x => x.IliskiliAgArayuzuId == agAkis.IliskiliAgArayuzuId).Select(s => s.Yuk).Sum() + agAkis.Yuk;
                    }

                    if (inputToplamAgYuku - inputToplamAgAkisi >= 0)
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
                            MainTitle.Content = "Ağ Akışı - " + toplam + " Mbps";

                            if (checkedAgAkisRow != null)
                            {
                                var ctx = checkedAgAkisRow.DataContext;
                                var obj = (AgAkis)ctx;
                                this.agAnahtariAgArayuzu.AgAkisList.Remove(obj);
                                checkedAgAkisRow = null;
                            }

                            AgAkisDataGrid.ItemsSource = null;
                            agAkis.Id = Guid.NewGuid();
                            agAkis.AgAkisProtokoluAdi = agAkis.AgAkisProtokoluList.Where(x => x.Id == agAkis.AgAkisProtokoluId).Select(s => s.Ad).FirstOrDefault();
                            agAkis.AgAkisTipiAdi = agAkis.AgAkisTipiList.Where(x => x.Id == agAkis.AgAkisTipiId).Select(s => s.Ad).FirstOrDefault();
                            agAkis.IliskiliAgArayuzuAdi = agAkis.InputList.Where(x => x.UniqueId == agAkis.IliskiliAgArayuzuId).Select(s => s.Label).FirstOrDefault();

                            this.agAnahtariAgArayuzu.AgAkisList.Add(agAkis);
                            SetToConnectorAgAkis();

                            AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;
                            AgAkisDataGrid.Visibility = Visibility.Visible;
                            AgAkisNoDataRow.Visibility = Visibility.Hidden;

                            DataContext = null;
                            agAkis = new AgAkis();
                            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
                            SetAgArayuzuInputList();
                            SetAgAkisProtokoluList();
                            SetAgAkisTipiList();
                            DataContext = agAkis;
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Ağın iletebileceği yük kapasitesini (" + this.agAnahtariAgArayuzu.MinKapasite + " - " + this.agAnahtariAgArayuzu.MaxKapasite + " Mbps) aştınız!";
                            nfp.Owner = Owner;
                            nfp.Show();
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Seçtiğiniz girdinin toplam ağ yükünü aştınız.";
                        nfp.Owner = Owner;
                        nfp.Show();
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
            agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
            SetAgArayuzuInputList();
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
                this.agAnahtariAgArayuzu.AgAkisList.Remove(obj);
                toplam = this.agAnahtariAgArayuzu.AgAkisList.Select(s => s.Yuk).Sum();
                MainTitle.Content = "Ağ Akışı - " + toplam + " Mbps";

                SetToConnectorAgAkis();
                AgAkisDataGrid.ItemsSource = this.agAnahtariAgArayuzu.AgAkisList;

                if (this.agAnahtariAgArayuzu.AgAkisList.Count == 0)
                {
                    AgAkisDataGrid.Visibility = Visibility.Hidden;
                    AgAkisNoDataRow.Visibility = Visibility.Visible;
                }

                checkedAgAkisRow = null;
                DataContext = null;
                agAkis = new AgAkis();
                agAkis.AgArayuzuId = this.agAnahtariAgArayuzu.UniqueId;
                SetAgArayuzuInputList();
                SetAgAkisTipiList();
                SetAgAkisProtokoluList();
                DataContext = agAkis;
        }
        #endregion

        private void SetToConnectorAgAkis()
        {
            this.agAnahtariAgArayuzu.Connect.ToConnector.AgAkisList.Clear();
            foreach (var agAkis in this.agAnahtariAgArayuzu.AgAkisList)
            {
                var temp = new AgAkis();
                temp.Id = Guid.NewGuid();
                temp.AgArayuzuId = this.agAnahtariAgArayuzu.Connect.ToConnector.UniqueId;
                temp.Yuk = agAkis.Yuk;
                temp.AgAkisProtokoluId = agAkis.AgAkisProtokoluId;
                temp.AgAkisTipiId = agAkis.AgAkisTipiId;

                this.agAnahtariAgArayuzu.Connect.ToConnector.AgAkisList.Add(temp);
            }
        }
    }
}
