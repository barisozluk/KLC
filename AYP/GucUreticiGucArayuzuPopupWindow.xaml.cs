using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
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
    /// Interaction logic for UcBirimAgArayuzuWindow.xaml
    /// </summary>
    public partial class GucUreticiGucArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IGucUreticiService service;

        private IKodListeService kodListeService;

        private NotificationManager notificationManager;

        GucArayuzu gucArayuzu;
        public MainWindow MainWindow { get; set; }

        public GucUreticiGucArayuzuPopupWindow()
        {
            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new GucUreticiService(this.context);
            kodListeService = new KodListeService(this.context);
            gucArayuzu = new GucArayuzu();

            InitializeComponent();
            ListGucUretici();
            ListKullanimAmaci();
            ListGerilimTipi();
            DataContext = gucArayuzu;

            if (gucArayuzu.GucUreticiList.Count() == 0)
            {
                Loaded += (s, e) => ClosePopup();
            }
        }

        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonGucUreticiArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void GucUreticiGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                g1.IsEnabled = false; g1.Opacity = 0.25;
                g2.IsEnabled = false; g2.Opacity = 0.25;
                g3.IsEnabled = false; g3.Opacity = 0.25;
                g4.IsEnabled = false; g4.Opacity = 0.25;
                g5.IsEnabled = false; g5.Opacity = 0.25;
                g6.IsEnabled = false; g6.Opacity = 0.25;
                g7.IsEnabled = false; g7.Opacity = 0.25;
                g8.IsEnabled = false; g8.Opacity = 0.25;
                g9.IsEnabled = false; g9.Opacity = 0.25;
                g10.IsEnabled = false; g10.Opacity = 0.25;
                g11.IsEnabled = false; g11.Opacity = 0.25;
                g12.IsEnabled = false; g12.Opacity = 0.25;

                g13.IsEnabled = true; g13.Opacity = 1;
                g14.IsEnabled = true; g14.Opacity = 1;
                g15.IsEnabled = true; g15.Opacity = 1;
                g16.IsEnabled = true; g16.Opacity = 1;
            }
            else
            {
                g1.IsEnabled = true; g1.Opacity = 1;
                g2.IsEnabled = true; g2.Opacity = 1;
                g3.IsEnabled = true; g3.Opacity = 1;
                g4.IsEnabled = true; g4.Opacity = 1;
                g5.IsEnabled = true; g5.Opacity = 1;
                g6.IsEnabled = true; g6.Opacity = 1;
                g7.IsEnabled = true; g7.Opacity = 1;
                g8.IsEnabled = true; g8.Opacity = 1;
                g9.IsEnabled = true; g9.Opacity = 1;
                g10.IsEnabled = true; g10.Opacity = 1;
                g11.IsEnabled = true; g11.Opacity = 1;
                g12.IsEnabled = true; g12.Opacity = 1;

                g13.IsEnabled = false; g13.Opacity = 0.25;
                g14.IsEnabled = false; g14.Opacity = 0.25;
                g15.IsEnabled = false; g15.Opacity = 0.25;
                g16.IsEnabled = false; g16.Opacity = 0.25;
            }
        }

        private void Save_GucUreticiGucAarayuzu(object sender, RoutedEventArgs e)
        {
            gucArayuzu.TipId = (int)TipEnum.GucUreticiGucArayuzu;

            var validationContext = new ValidationContext(gucArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
            {
                var response = service.SaveGucUreticiGucArayuzu(gucArayuzu);

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

        private void ListGerilimTipi()
        {
            gucArayuzu.GerilimTipiList = kodListeService.ListGerilimTipi();
            if (gucArayuzu.GerilimTipiList.Count() > 0)
            {
                gucArayuzu.GerilimTipiId = gucArayuzu.GerilimTipiList[0].Id;
            }
        }

        private void ListKullanimAmaci()
        {
            gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            if (gucArayuzu.KullanimAmaciList.Count() > 0)
            {
                gucArayuzu.KullanimAmaciId = gucArayuzu.KullanimAmaciList[0].Id;
            }
        }

        private void ListGucUretici()
        {
            gucArayuzu.GucUreticiList = service.ListGucUretici();
            if (gucArayuzu.GucUreticiList.Count() > 0)
            {
                gucArayuzu.GucUreticiId = gucArayuzu.GucUreticiList[0].Id;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir güç üretici tanımlayınız.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    if (ch.Equals('.') || ch.Equals(','))
                    {
                        int seperatorCount = textBox.Text.Where(t => t.Equals('.') || t.Equals(',')).Count();

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
    }
}
