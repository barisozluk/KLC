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
using System.Text.RegularExpressions;
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
    public partial class UcBirimGucArayuzuPopupWindow : Window
    {
        private AYPContext context;

        private IUcBirimService service;

        private IKodListeService kodListeService;

        private NotificationManager notificationManager;

        GucArayuzu gucArayuzu;
        public MainWindow MainWindow { get; set; }


        public UcBirimGucArayuzuPopupWindow()
        {
            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new UcBirimService(this.context);
            kodListeService = new KodListeService(this.context);
            gucArayuzu = new GucArayuzu();

            InitializeComponent();
            ListUcBirim();
            ListKullanimAmaci();
            ListGerilimTipi();
            DataContext = gucArayuzu;

            if (gucArayuzu.UcBirimList.Count() == 0)
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

        private void ButtonUcBirimGucArayuzPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void UcBirimGucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                ug1.IsEnabled = false; ug1.Opacity = 0.25;
                ug2.IsEnabled = false; ug2.Opacity = 0.25;
                ug3.IsEnabled = false; ug3.Opacity = 0.25;
                ug4.IsEnabled = false; ug4.Opacity = 0.25;
                ug5.IsEnabled = false; ug5.Opacity = 0.25;
                ug6.IsEnabled = false; ug6.Opacity = 0.25;
                ug7.IsEnabled = false; ug7.Opacity = 0.25;
                ug8.IsEnabled = false; ug8.Opacity = 0.25;
                ug9.IsEnabled = false; ug9.Opacity = 0.25;
                ug10.IsEnabled = false; ug10.Opacity = 0.25;
                ug11.IsEnabled = false; ug11.Opacity = 0.25;
                ug12.IsEnabled = false; ug12.Opacity = 0.25;

                ug13.IsEnabled = true; ug13.Opacity = 1;
                ug14.IsEnabled = true; ug14.Opacity = 1;
                ug15.IsEnabled = true; ug15.Opacity = 1;
                ug16.IsEnabled = true; ug16.Opacity = 1;
            }
            else
            {
                ug1.IsEnabled = true; ug1.Opacity = 1;
                ug2.IsEnabled = true; ug2.Opacity = 1;
                ug3.IsEnabled = true; ug3.Opacity = 1;
                ug4.IsEnabled = true; ug4.Opacity = 1;
                ug5.IsEnabled = true; ug5.Opacity = 1;
                ug6.IsEnabled = true; ug6.Opacity = 1;
                ug7.IsEnabled = true; ug7.Opacity = 1;
                ug8.IsEnabled = true; ug8.Opacity = 1;
                ug9.IsEnabled = true; ug9.Opacity = 1;
                ug10.IsEnabled = true; ug10.Opacity = 1;
                ug11.IsEnabled = true; ug11.Opacity = 1;
                ug12.IsEnabled = true; ug12.Opacity = 1;

                ug13.IsEnabled = false; ug13.Opacity = 0.25;
                ug14.IsEnabled = false; ug14.Opacity = 0.25;
                ug15.IsEnabled = false; ug15.Opacity = 0.25;
                ug16.IsEnabled = false; ug16.Opacity = 0.25;
            }
        }

        private void Save_UcBirimGucAarayuzu(object sender, RoutedEventArgs e)
        {
            gucArayuzu.TipId = (int)TipEnum.UcBirimGucArayuzu;

            var validationContext = new ValidationContext(gucArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
            {
                var response = service.SaveUcBirimGucArayuzu(gucArayuzu);

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

        private void ListUcBirim()
        {
            gucArayuzu.UcBirimList = service.ListUcBirim();
            if (gucArayuzu.UcBirimList.Count() > 0)
            {
                gucArayuzu.UcBirimId = gucArayuzu.UcBirimList[0].Id;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, En Az Bir Uç Birim Tanımlayınız";
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
