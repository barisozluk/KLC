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
using System.Windows;
using System.Windows.Media;

namespace AYP
{
    /// <summary>
    /// Interaction logic for GucUreticiPopupWindow.xaml
    /// </summary>
    public partial class GucUreticiPopupWindow : Window
    {
        public byte[] selectedKatalogFile = null;
        public byte[] selectedSembolFile = null;
        public bool isEditMode = false;

        private AYPContext context;

        private IGucUreticiService service;

        private NotificationManager notificationManager;

        GucUretici gucUretici;
        public GucUreticiPopupWindow(GucUretici _gucUretici)
        {
            if(_gucUretici != null)
            {
                gucUretici = _gucUretici;
                isEditMode = true;
            }
            else
            {
                gucUretici = new GucUretici();
                isEditMode = false;
            }

            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new GucUreticiService(this.context);
            SetGucUreticiTurList();
            InitializeComponent();
            DataContext = gucUretici;
        }

        private void ButtonGucUreticiPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void Save_GucUretici(object sender, RoutedEventArgs e)
        {
            gucUretici.Katalog = this.selectedKatalogFile;
            gucUretici.Sembol = this.selectedSembolFile;
            gucUretici.TipId = (int)TipEnum.GucUretici;

            var validationContext = new ValidationContext(gucUretici, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucUretici, validationContext, results, true))
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    response = service.SaveGucUretici(gucUretici);
                }
                else
                {
                    response = service.UpdateGucUretici(gucUretici);
                }

                if (!response.HasError)
                {
                    notificationManager.ShowSuccessMessage(response.Message);

                    (Owner as MainWindow).ListGucUretici();
                    Close();
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
                        if (memberName == "GucUreticiTurId")
                        {
                            GucUreticiTur.BorderBrush = new SolidColorBrush(Colors.Red);
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

                        if (memberName == "GirdiGucArayuzuSayisi")
                        {
                            GirdiGucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "CiktiGucArayuzuSayisi")
                        {
                            CiktiGucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "VerimlilikDegeri")
                        {
                            VerimlilikOrani.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "DahiliGucTuketimDegeri")
                        {
                            DahiliGucTuketimDegeri.BorderBrush = new SolidColorBrush(Colors.Red);
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

        #region OpenKatalogFileDialogEvent
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

        #endregion

        #region OpenSembolFileDialogEvent
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

        #endregion

        private void SetGucUreticiTurList()
        {
            gucUretici.GucUreticiTurList = service.ListGucUreticiTur();
            if (gucUretici.GucUreticiTurList.Count() > 0)
            {
                if (!isEditMode)
                {
                    gucUretici.GucUreticiTurId = gucUretici.GucUreticiTurList[0].Id;
                }
            }
            else
            {
                notificationManager.ShowWarningMessage("Lütfen, En Az Bir Güç Üretici Türü Tanımlayınız!");
            }
        }
    }
}
