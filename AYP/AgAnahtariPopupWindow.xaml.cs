using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariPopupWindow : Window
    {
        AgAnahtari agAnahtari;

        public byte[] selectedKatalogFile = null;
        public byte[] selectedSembolFile = null;

        private AYPContext context;

        private IAgAnahtariService service;
        public AgAnahtariPopupWindow()
        {
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);
            agAnahtari = new AgAnahtari();

            SetAgAnahtariTurList();
            InitializeComponent();
            DataContext = agAnahtari;
        }

        private void ButtonAgAnahtariPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void Save_AgAnahtari(object sender, RoutedEventArgs e)
        {
            agAnahtari.Katalog = this.selectedKatalogFile;
            agAnahtari.Sembol = this.selectedSembolFile;
            agAnahtari.TipId = (int)TipEnum.AgAnahtari;

            NotificationManager notificationManager = new NotificationManager();

            var validationContext = new ValidationContext(agAnahtari, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAnahtari, validationContext, results, true))
            {
                var response = service.SaveAgAnahtari(agAnahtari);

                if (!response.HasError)
                {
                    notificationManager.ShowSuccessMessage(response.Message);

                    (Owner as MainWindow).ListAgAnahtari();
                    Hide();
                    Owner.IsEnabled = true;
                    Owner.Effect = null;
                }
                else
                {
                    notificationManager.ShowErrorMessage(response.Message);
                }
            }
            else
            {
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        if (memberName == "AgAnahtariTurId")
                        {
                            AgAnahtariTur.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "StokNo")
                        {
                            StokNo.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "Tanim")
                        {
                            Tanim.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "UreticiAdi")
                        {
                            Uretici.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "UreticiParcaNo")
                        {
                            UreticiParcaNo.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "GirdiAgArayuzuSayisi")
                        {
                            GirdiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "CiktiAgArayuzuSayisi")
                        {
                            CiktiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "GucArayuzuSayisi")
                        {
                            GucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "Katalog")
                        {
                            Katalog.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "Sembol")
                        {
                            Sembol.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }
        }

        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {

                Katalog.Text = Path.GetFileName(openFileDialog.FileName);
                this.selectedKatalogFile = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                Sembol.Text = Path.GetFileName(openFileDialog.FileName);
                this.selectedSembolFile = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void SetAgAnahtariTurList()
        {
            agAnahtari.AgAnahtariTurList = service.ListAgAnahtariTur();
            agAnahtari.AgAnahtariTurId = agAnahtari.AgAnahtariTurList[0].Id;
        }
    }
}
