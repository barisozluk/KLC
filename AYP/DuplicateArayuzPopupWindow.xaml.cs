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
    public partial class DuplicateArayuzPopupWindow : Window
    {
        AgArayuzu selectedAgArayuzu;
        GucArayuzu selectedGucArayuzu;
        int cihazTipId = 0;
        public DuplicateArayuzPopupWindow(int cihazTipId, AgArayuzu selectedAgArayuzu, GucArayuzu selectedGucArayuzu, List<string> portList)
        {
            this.selectedAgArayuzu = selectedAgArayuzu;
            this.selectedGucArayuzu = selectedGucArayuzu;
            this.cihazTipId = cihazTipId;

            InitializeComponent();
            PortListBox.ItemsSource = portList;
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonDuplicateArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
            
        }

        private void Save_DuplicateArayuzPopup(object sender, RoutedEventArgs e)
        {
            var selectedItems = PortListBox.SelectedItems.Cast<string>();

            if (selectedAgArayuzu != null)
            {
                int count = 1;
                foreach(var selectedItem in selectedItems)
                {
                    AgArayuzu arayuz = new AgArayuzu();
                    arayuz.Adi = selectedAgArayuzu.Adi + " " + count;
                    arayuz.Port = selectedItem;
                    arayuz.FizikselOrtamId = selectedAgArayuzu.FizikselOrtamId;
                    arayuz.KapasiteId = selectedAgArayuzu.KapasiteId;
                    arayuz.KullanimAmaciId = selectedAgArayuzu.KullanimAmaciId;
                    arayuz.KL_KullanimAmaci = selectedAgArayuzu.KullanimAmaciList.Where(kal => kal.Id == selectedAgArayuzu.KullanimAmaciId).FirstOrDefault();
                    arayuz.KL_Kapasite = selectedAgArayuzu.KapasiteList.Where(kl => kl.Id == selectedAgArayuzu.KapasiteId).FirstOrDefault();
                    arayuz.KL_FizikselOrtam = selectedAgArayuzu.FizikselOrtamList.Where(fo => fo.Id == selectedAgArayuzu.FizikselOrtamId).FirstOrDefault();
                    arayuz.KullanimAmaciList = selectedAgArayuzu.KullanimAmaciList;
                    arayuz.FizikselOrtamList = selectedAgArayuzu.FizikselOrtamList;
                    arayuz.KapasiteList = selectedAgArayuzu.KapasiteList;

                    if (this.cihazTipId == (int)TipEnum.UcBirim)
                    {
                        UcBirimPopupWindow popup = Owner as UcBirimPopupWindow;
                        arayuz.TipId = (int)TipEnum.UcBirimAgArayuzu;
                        popup.agArayuzuList.Add(arayuz);
                        popup.UpdateAgArayuzuTable();
                    }
                    else if(this.cihazTipId == (int)TipEnum.AgAnahtari)
                    {
                        AgAnahtariPopupWindow popup = Owner as AgAnahtariPopupWindow;
                        arayuz.TipId = (int)TipEnum.AgAnahtariAgArayuzu;
                        popup.agArayuzuList.Add(arayuz);
                        popup.UpdateAgArayuzuTable();
                    }

                    count++;
                }
            }
            else
            {
                int count = 1;
                foreach (var selectedItem in selectedItems)
                {
                    GucArayuzu arayuz = new GucArayuzu();
                    arayuz.Adi = selectedGucArayuzu.Adi + " " + count;
                    arayuz.Port = selectedItem;
                    arayuz.CiktiDuraganGerilimDegeri = selectedGucArayuzu.CiktiDuraganGerilimDegeri;
                    arayuz.CiktiUrettigiGucKapasitesi = selectedGucArayuzu.CiktiUrettigiGucKapasitesi;
                    arayuz.GirdiDuraganGerilimDegeri1 = selectedGucArayuzu.GirdiDuraganGerilimDegeri1;
                    arayuz.GirdiDuraganGerilimDegeri2 = selectedGucArayuzu.GirdiDuraganGerilimDegeri2;
                    arayuz.GirdiDuraganGerilimDegeri3 = selectedGucArayuzu.GirdiDuraganGerilimDegeri3;
                    arayuz.GirdiMaksimumGerilimDegeri = selectedGucArayuzu.GirdiMaksimumGerilimDegeri;
                    arayuz.GirdiMinimumGerilimDegeri = selectedGucArayuzu.GirdiMinimumGerilimDegeri;
                    arayuz.GirdiTukettigiGucMiktari = selectedGucArayuzu.GirdiTukettigiGucMiktari;
                    arayuz.GerilimTipiId = selectedGucArayuzu.GerilimTipiId;
                    arayuz.KullanimAmaciId = selectedGucArayuzu.KullanimAmaciId;
                    arayuz.KL_KullanimAmaci = selectedGucArayuzu.KullanimAmaciList.Where(kal => kal.Id == selectedGucArayuzu.KullanimAmaciId).FirstOrDefault();
                    arayuz.KL_GerilimTipi = selectedGucArayuzu.GerilimTipiList.Where(gt => gt.Id == selectedGucArayuzu.GerilimTipiId).FirstOrDefault();
                    arayuz.KullanimAmaciList = selectedGucArayuzu.KullanimAmaciList;
                    arayuz.GerilimTipiList = selectedGucArayuzu.GerilimTipiList;

                    if (this.cihazTipId == (int)TipEnum.UcBirim)
                    {
                        UcBirimPopupWindow popup = Owner as UcBirimPopupWindow;
                        arayuz.TipId = (int)TipEnum.UcBirimGucArayuzu;
                        popup.gucArayuzuList.Add(arayuz);
                        popup.UpdateGucArayuzuTable();

                    }
                    else if (this.cihazTipId == (int)TipEnum.AgAnahtari)
                    {
                        AgAnahtariPopupWindow popup = Owner as AgAnahtariPopupWindow;
                        arayuz.TipId = (int)TipEnum.AgAnahtariGucArayuzu;
                        popup.gucArayuzuList.Add(arayuz);
                        popup.UpdateGucArayuzuTable();
                    }
                    else if (this.cihazTipId == (int)TipEnum.GucUretici)
                    {
                        GucUreticiPopupWindow popup = Owner as GucUreticiPopupWindow;
                        arayuz.TipId = (int)TipEnum.GucUreticiGucArayuzu;
                        popup.gucArayuzuList.Add(arayuz);
                        popup.UpdateGucArayuzuTable();
                    }

                    count++;
                }
            }

            ClosePopup();
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
