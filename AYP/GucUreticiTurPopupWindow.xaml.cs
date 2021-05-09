using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for GucUreticiTurPopupWindow.xaml
    /// </summary>
    public partial class GucUreticiTurPopupWindow : Window
    {
        private IGucUreticiService service;

        GucUreticiTur gucUreticiTur;

        public MainWindow MainWindow { get; set; }

        public GucUreticiTurPopupWindow()
        {
            service = new GucUreticiService();
            gucUreticiTur = new GucUreticiTur();

            InitializeComponent();
            DataContext = gucUreticiTur;
        }

        private void Save_GucUreticiTur(object sender, RoutedEventArgs e)
        {
            NotificationManager notificationManager = new NotificationManager();

            var validationContext = new ValidationContext(gucUreticiTur, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucUreticiTur, validationContext, results, true))
            {
                var response = service.SaveGucUreticiTur(gucUreticiTur);

                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();

                    Close();
                    Owner.IsEnabled = true;
                    Owner.Effect = null;
                }
                else
                {
                    NotifyWarningPopup nfp = new NotifyWarningPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();
                }
            }
            else
            {
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "Ad")
                        {
                            Ad.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, zorunlu alanları doldurunuz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }


        private void ButtonGucUreticiTurPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }
    }
}
