using AYP.Calculations;
using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using AYP.ViewModel;
using AYP.ViewModel.Node;
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
    public partial class CableLengthPopupWindow : Window
    {


        public ConnectViewModel connect;

        public CableLengthPopupWindow(ConnectViewModel connect)
        {
            this.connect = connect;
            InitializeComponent();

            Uzunluk.Text = this.connect.Uzunluk.ToString();
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonCableLengthPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Save_CableLengthPopup(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Uzunluk.Text) && !String.IsNullOrWhiteSpace(Uzunluk.Text))
            {
                connect.Uzunluk = Convert.ToDecimal(Uzunluk.Text);
                ClosePopup();
            }
            else
            {
                Uzunluk.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void Calculate_CableLengthPopup(object sender, RoutedEventArgs e)
        {
            Calculation calculations = new Calculation();
            if(connect.FromConnector.TypeId == 3 || connect.FromConnector.TypeId == 8)
            { 
                KabloKesit.Text = (calculations.CableSuggestionCalculation((double)connect.ToConnector.GirdiTukettigiGucMiktari,(double)connect.ToConnector.GirdiDuraganGerilimDegeri1)).ToString();
                isiKaybi.Text = (calculations.HeatLossCalculation((double)connect.ToConnector.GirdiTukettigiGucMiktari, (double)connect.ToConnector.GirdiDuraganGerilimDegeri1, KabloTipiTur.SelectedIndex, (double)connect.Uzunluk)).ToString();
                GerilimDusumu.Text = (calculations.VoltageDropCalculation((double)connect.Uzunluk, (double)connect.ToConnector.GirdiTukettigiGucMiktari, KabloTipiTur.SelectedIndex, Convert.ToDouble(connect.FromConnector.CiktiDuraganGerilimDegeri))).ToString();
                if (BataryaKapasite.Text != "")
                    BeslemeSuresi.Text = (calculations.FeedingTimeCalculation((double)connect.FromConnector.CiktiUrettigiGucKapasitesi, Convert.ToDouble(connect.FromConnector.CiktiDuraganGerilimDegeri), Convert.ToDouble(BataryaKapasite.Text))).ToString();
            }
            else
            {
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Hesaplamalar güç üretici ve güç arayüzleri arasında yapılmalıdır.";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }

        #region NumberValidationEvents
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

    }
}
