﻿using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Enums;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Models;
using AYP.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
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

        public bool isEditMode = false;
        public MainWindow MainWindow { get; set; }


        public UcBirimPopupWindow(UcBirim _ucBirim, bool fromNode)
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
            SetUcBirimTurList();
            DataContext = ucBirim;

            if(isEditMode)
            {
                DownloadButton.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadButton.Visibility = Visibility.Hidden;
            }

            if (fromNode)
            {
                GirdiAgArayuzuSayisi.IsEnabled = false;
                GirdiAgArayuzuSayisi.Opacity = 0.25;
                CiktiAgArayuzuSayisi.IsEnabled = false;
                CiktiAgArayuzuSayisi.Opacity = 0.25;
                GucArayuzuSayisi.IsEnabled = false;
                GucArayuzuSayisi.Opacity = 0.25;
            }
            else
            {
                GirdiAgArayuzuSayisi.IsEnabled = true;
                GirdiAgArayuzuSayisi.Opacity = 1;
                CiktiAgArayuzuSayisi.IsEnabled = true;
                CiktiAgArayuzuSayisi.Opacity = 1;
                GucArayuzuSayisi.IsEnabled = true;
                GucArayuzuSayisi.Opacity = 1;
            }

            if (ucBirim.UcBirimTurList.Count() == 0)
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

        private void ButtonUcBirimPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void Save_UcBirim(object sender, RoutedEventArgs e)
        {
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
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = this.MainWindow;
                    nfp.Show();

                    (Owner as MainWindow).ListUcBirim();
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
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, zorunlu alanları doldurunuz.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        #region OpenKatalogFileDialogEvent
        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {
                ucBirim.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = ucBirim.KatalogDosyaAdi;
                ucBirim.Katalog = File.ReadAllBytes(openFileDialog.FileName);
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
                ucBirim.SembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = ucBirim.SembolDosyaAdi;
                ucBirim.Sembol = File.ReadAllBytes(openFileDialog.FileName);
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
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, En Az Bir Uç Birim Tanımlayınız";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnDownloadKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath + "\\" + ucBirim.KatalogDosyaAdi;

                    try
                    {
                        File.WriteAllBytes(path, ucBirim.Katalog);
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = "İşlem Başarı ile Gerçekleştirildi.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                    catch (Exception exception)
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = "İşlem Başarısız Oldu.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();
                    }
                }
            }
        }

    }
}
