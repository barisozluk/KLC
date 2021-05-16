using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
    public partial class EditSelectedPopupWindow : Window
    {        
        private IUcBirimService serviceUcBirim;
        private IAgAnahtariService serviceAgAnahtari;
        private IGucUreticiService serviceGucUreticisi;

        public int cihazTur;
        public List<int> selectedIds;

        private byte[] sembol = null;
        private string sembolDosyaAdi = null;

        public EditSelectedPopupWindow(int cihazTur, List<int> selectedIds)
        {

            serviceUcBirim = new UcBirimService();
            serviceAgAnahtari = new AgAnahtariService();
            serviceGucUreticisi = new GucUreticiService();
            
            InitializeComponent();

            this.cihazTur = cihazTur;
            this.selectedIds = selectedIds;
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonTopluEditPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
            
        }

        #region OpenSembolFileDialogEvent
        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\SEMA_Data\\StreamingAssets\\AYP\\SembolKutuphanesi";
            openFileDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SembolKutuphanesi");
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                sembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = sembolDosyaAdi;
                sembol = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        #endregion

        private void Save_TopluEdit(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Uretici.Text) && !string.IsNullOrEmpty(UreticiParcaNo.Text) && !string.IsNullOrEmpty(Sembol.Text))
            {
                if (cihazTur == (int)TipEnum.UcBirim)
                {
                    var response = serviceUcBirim.SaveTopluEdit(selectedIds, Uretici.Text, UreticiParcaNo.Text, sembolDosyaAdi, sembol);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();

                        (Owner as MainWindow).ListUcBirim();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
                else if (cihazTur == (int)TipEnum.AgAnahtari)
                {
                    var response = serviceAgAnahtari.SaveTopluEdit(selectedIds, Uretici.Text, UreticiParcaNo.Text, sembolDosyaAdi, sembol);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();

                        (Owner as MainWindow).ListAgAnahtari();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
                else if (cihazTur == (int)TipEnum.GucUretici)
                {
                    var response = serviceGucUreticisi.SaveTopluEdit(selectedIds, Uretici.Text, UreticiParcaNo.Text, sembolDosyaAdi, sembol);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();

                        (Owner as MainWindow).ListGucUretici();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Uretici.Text))
                {
                    Uretici.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (string.IsNullOrEmpty(UreticiParcaNo.Text))
                {
                    UreticiParcaNo.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (string.IsNullOrEmpty(Sembol.Text))
                {
                    Sembol.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
        }
    }
}
