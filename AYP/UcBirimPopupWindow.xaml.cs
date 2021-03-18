﻿using AYP.DbContext.AYP.DbContexts;
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
    /// Interaction logic for UcBirimPopupWindow.xaml
    /// </summary>
    public partial class UcBirimPopupWindow : Window
    {        
        private AYPContext context;

        private IUcBirimService service;

        private NotificationManager notificationManager;

        UcBirim ucBirim;

        public byte[] selectedKatalogFile = null;
        public byte[] selectedSembolFile = null;
        public bool isEditMode = false;

        public UcBirimPopupWindow(UcBirim _ucBirim)
        {

            if(_ucBirim != null)
            {
                ucBirim = _ucBirim;
                isEditMode = true;
            }
            else
            {
                ucBirim = new UcBirim();
                isEditMode = false;
            }

            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new UcBirimService(this.context);
            InitializeComponent();
            DataContext = ucBirim;

            SetUcBirimTurList();
        }

        private void ButtonUcBirimPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void Save_UcBirim(object sender, RoutedEventArgs e)
        {
            ucBirim.Katalog = this.selectedKatalogFile;
            ucBirim.Sembol = this.selectedSembolFile;
            ucBirim.TipId = (int)TipEnum.UcBirim;

            var validationContext = new ValidationContext(ucBirim, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(ucBirim, validationContext, results, true))
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    response = service.SaveUcBirim(ucBirim);
                }
                else
                {
                    response = service.UpdateUcBirim(ucBirim);
                }

                if (!response.HasError)
                {
                    notificationManager.ShowSuccessMessage(response.Message);

                    (Owner as MainWindow).ListUcBirim();
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
                        if (memberName == "UcBirimTurId")
                        {
                            UcBirimTur.BorderBrush = new SolidColorBrush(Colors.Red);
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

        private void SetUcBirimTurList()
        {   
            ucBirim.UcBirimTurList = service.ListUcBirimTur();

            if (ucBirim.UcBirimTurList.Count() > 0)
            {
                if (!isEditMode)
                {
                    ucBirim.UcBirimTurId = ucBirim.UcBirimTurList[0].Id;
                }
            }
            else
            {
                notificationManager.ShowWarningMessage("Lütfen, En Az Bir Uç Birim Türü Tanımlayınız!");
            }
        }
    }
}
