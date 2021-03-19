using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariPopupWindow : Window
    {
        public bool isEditMode = false;

        private AYPContext context;

        private IAgAnahtariService service;

        AgAnahtari agAnahtari;

        private NotificationManager notificationManager;
        public AgAnahtariPopupWindow(AgAnahtari _agAnahtari)
        {
            if(_agAnahtari != null)
            {
                agAnahtari = _agAnahtari;
                isEditMode = true;
            }
            else
            {
                agAnahtari = new AgAnahtari();
                isEditMode = false;
            }

            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);
            InitializeComponent();
            SetAgAnahtariTurList();
            DataContext = agAnahtari;

            if (agAnahtari.AgAnahtariTurList.Count() == 0)
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

        private void ButtonAgAnahtariPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Save_AgAnahtari(object sender, RoutedEventArgs e)
        {
            agAnahtari.TipId = (int)TipEnum.AgAnahtari;

            var validationContext = new ValidationContext(agAnahtari, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAnahtari, validationContext, results, true))
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    response = service.SaveAgAnahtari(agAnahtari);
                }
                else
                {
                    response = service.UpdateAgAnahtari(agAnahtari);
                }

                if (!response.HasError)
                {
                    notificationManager.ShowSuccessMessage(response.Message);

                    (Owner as MainWindow).ListAgAnahtari();
                    ClosePopup();
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

                        if (memberName == "Katalog" || memberName == "KatalogDosyaAdi")
                        {
                            Katalog.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "Sembol" || memberName == "SembolDosyaAdi")
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
                agAnahtari.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = agAnahtari.KatalogDosyaAdi;
                agAnahtari.Katalog = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                agAnahtari.SembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = agAnahtari.SembolDosyaAdi;
                agAnahtari.Sembol = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void SetAgAnahtariTurList()
        {
            agAnahtari.AgAnahtariTurList = service.ListAgAnahtariTur();
            if (agAnahtari.AgAnahtariTurList.Count() > 0)
            {
                if (!isEditMode)
                {
                    agAnahtari.AgAnahtariTurId = agAnahtari.AgAnahtariTurList[0].Id;
                }
            }
            else
            {
                notificationManager.ShowWarningMessage("Lütfen, En Az Bir Ağ Anahtarı Türü Tanımlayınız!");
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
