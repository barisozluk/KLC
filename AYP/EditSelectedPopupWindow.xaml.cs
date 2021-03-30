using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        private AYPContext context;
        
        private IUcBirimService serviceUcBirim;
        private IAgAnahtariService serviceAgAnahtari;
        private IGucUreticiService serviceGucUreticisi;

        public int cihazTur;
        public List<int> selectedIds;
        private string UreticiAdi = null;

        public MainWindow MainWindow { get; set; }

        public EditSelectedPopupWindow(int cihazTur, List<int> selectedIds)
        {

            context = new AYPContext();
            serviceUcBirim = new UcBirimService(context);
            serviceAgAnahtari = new AgAnahtariService(context);
            serviceGucUreticisi = new GucUreticiService(context);
            
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

        private void Save_TopluEdit(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            if(!string.IsNullOrEmpty(Uretici.Text))
            {
                if(cihazTur == (int)TipEnum.UcBirim)
                {
                    var response =  serviceUcBirim.SaveTopluEdit(selectedIds, Uretici.Text);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
                else if (cihazTur == (int)TipEnum.AgAnahtari)
                {
                    var response = serviceUcBirim.SaveTopluEdit(selectedIds, Uretici.Text);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
                else if (cihazTur == (int)TipEnum.GucUretici)
                {
                    var response =  serviceUcBirim.SaveTopluEdit(selectedIds, Uretici.Text);

                    if (!response.HasError)
                    {
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                        ClosePopup();
                    }
                    else
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = response.Message;
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
            }
            else
            {
                Uretici.BorderBrush = new SolidColorBrush(Colors.Red);
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, zorunlu alanları doldurunuz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }
    }
}
