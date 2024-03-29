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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AYP
{
    /// <summary>
    /// Interaction logic for GucUreticiPopupWindow.xaml
    /// </summary>
    public partial class GucUreticiPopupWindow : Window
    {
        private IGucUreticiService service;
        private IKodListeService kodListeService;

        GucUretici gucUretici;
        GucUretici oldGucUretici;
        GucArayuzu gucArayuzu;

        public List<GucArayuzu> gucArayuzuList;
        private CheckBox checkedGucArayuzuRow = null;

        public bool isEditMode = false;
        public bool fromNode = false;

        public GucUreticiPopupWindow(GucUretici _gucUretici, bool fromNode)
        {
            this.fromNode = fromNode;

            gucArayuzu = new GucArayuzu();
            if (_gucUretici != null)
            {
                oldGucUretici = (GucUretici)_gucUretici.Clone();
                gucUretici = (GucUretici)_gucUretici.Clone();
                isEditMode = true;
            }
            else
            {
                gucUretici = new GucUretici();
                gucUretici.GirdiGucArayuzuSayisi = 0;
                isEditMode = false;
            }

            service = new GucUreticiService();
            kodListeService = new KodListeService();
            InitializeComponent();
            SetGucUreticiTurList();
            GucUreticiTab.DataContext = gucUretici;

            if (gucUretici.GucUreticiTurList.Count() == 0)
            {
                Loaded += (s, e) => ClosePopup();
            }

            Loaded += Window_Loaded;
        }

        #region WindowLoadedEvent
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEditMode)
            {
                gucArayuzuList = service.ListGucUreticiGucArayuzu(gucUretici.Id);
                GucUreticiGucArayuzDataGrid.ItemsSource = gucArayuzuList;
                if (gucArayuzuList.Count > 0)
                {
                    GucUreticiGucArayuzDataGrid.Visibility = Visibility.Visible;
                    GucArayuzuNoDataRow.Visibility = Visibility.Hidden;
                }
                else
                {
                    GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                    GucArayuzuNoDataRow.Visibility = Visibility.Visible;
                }

                if (string.IsNullOrEmpty(gucUretici.KatalogDosyaAdi))
                {
                    DownloadButton.Visibility = Visibility.Hidden;
                    DeleteButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    DownloadButton.Visibility = Visibility.Visible;
                    DeleteButton.Visibility = Visibility.Visible;
                }

                if (fromNode)
                {
                    //GucUreticiTab
                    GucUreticiTur.IsEnabled = false;
                    GucUreticiTur.Opacity = 0.25;
                    StokNo.IsEnabled = false;
                    StokNo.Opacity = 0.25;
                    Tanim.IsEnabled = false;
                    Tanim.Opacity = 0.25;
                    Uretici.IsEnabled = false;
                    Uretici.Opacity = 0.25;
                    UreticiParcaNo.IsEnabled = false;
                    UreticiParcaNo.Opacity = 0.25;
                    GirdiGucArayuzuSayisi.IsEnabled = false;
                    GirdiGucArayuzuSayisi.Opacity = 0.25;
                    CiktiGucArayuzuSayisi.IsEnabled = false;
                    CiktiGucArayuzuSayisi.Opacity = 0.25;
                    VerimlilikOrani.IsEnabled = false;
                    VerimlilikOrani.Opacity = 0.25;
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    Katalog.IsEnabled = false;
                    Katalog.Opacity = 0.25;
                    OpenKatalogBtn.IsEnabled = false;
                    OpenKatalogBtn.Opacity = 0.25;
                    Sembol.IsEnabled = false;
                    Sembol.Opacity = 0.25;
                    OpenSembolBtn.IsEnabled = false;
                    OpenSembolBtn.Opacity = 0.25;
                    DownloadButton.IsEnabled = true;
                    DownloadButton.Opacity = 1;
                    DeleteButton.IsEnabled = false;
                    DeleteButton.Opacity = 0.25;

                    //GucArayuzuTab
                    GucArayuzuAdi.IsEnabled = false;
                    GucArayuzuAdi.Opacity = 0.25;
                    GucArayuzuKullanimAmaciList.IsEnabled = false;
                    GucArayuzuKullanimAmaciList.Opacity = 0.25;
                    GucArayuzuGerilimTipiList.IsEnabled = false;
                    GucArayuzuGerilimTipiList.Opacity = 0.25;
                    ag2.IsEnabled = false;
                    ag2.Opacity = 0.25;
                    ag4.IsEnabled = false;
                    ag4.Opacity = 0.25;
                    ag6.IsEnabled = false;
                    ag6.Opacity = 0.25;
                    ag8.IsEnabled = false;
                    ag8.Opacity = 0.25;
                    ag10.IsEnabled = false;
                    ag10.Opacity = 0.25;
                    ag12.IsEnabled = false;
                    ag12.Opacity = 0.25;
                    ag14.IsEnabled = false;
                    ag14.Opacity = 0.25;
                    ag16.IsEnabled = false;
                    ag16.Opacity = 0.25;
                    GucArayuzuPortList.IsEnabled = false;
                    GucArayuzuPortList.Opacity = 0.25;
                    GucArayuzuEkleBtn.IsEnabled = false;
                    GucArayuzuEkleBtn.Opacity = 0.25;

                    foreach (var gucArayuzu in gucArayuzuList)
                    {
                        gucArayuzu.IsEnabled = false;
                    }

                    SaveBtn.IsEnabled = false;
                    SaveBtn.Opacity = 0.25;
                }
                else
                {
                    var mainWindow = Owner as MainWindow;
                    var nodes = mainWindow.ViewModel.NodesCanvas.Nodes.Items;

                    if (!nodes.Any(n => n.Id == gucUretici.Id && n.TypeId == (int)TipEnum.GucUretici))
                    {
                        fromNode = false;
                        //GucUreticiTab
                        GucUreticiTur.IsEnabled = true;
                        GucUreticiTur.Opacity = 1;
                        StokNo.IsEnabled = true;
                        StokNo.Opacity = 1;
                        Tanim.IsEnabled = true;
                        Tanim.Opacity = 1;
                        Uretici.IsEnabled = true;
                        Uretici.Opacity = 1;
                        UreticiParcaNo.IsEnabled = true;
                        UreticiParcaNo.Opacity = 1;
                        GirdiGucArayuzuSayisi.IsEnabled = true;
                        GirdiGucArayuzuSayisi.Opacity = 1;
                        CiktiGucArayuzuSayisi.IsEnabled = true;
                        CiktiGucArayuzuSayisi.Opacity = 1;
                        VerimlilikOrani.IsEnabled = true;
                        VerimlilikOrani.Opacity = 1;
                        DahiliGucTuketimDegeri.IsEnabled = true;
                        DahiliGucTuketimDegeri.Opacity = 1;
                        Katalog.IsEnabled = true;
                        Katalog.Opacity = 1;
                        OpenKatalogBtn.IsEnabled = true;
                        OpenKatalogBtn.Opacity = 1;
                        Sembol.IsEnabled = true;
                        Sembol.Opacity = 1;
                        OpenSembolBtn.IsEnabled = true;
                        OpenSembolBtn.Opacity = 1;
                        DownloadButton.IsEnabled = true;
                        DownloadButton.Opacity = 1;
                        DeleteButton.IsEnabled = true;
                        DeleteButton.Opacity = 1;

                        //GucArayuzuTab
                        GucArayuzuAdi.IsEnabled = true;
                        GucArayuzuAdi.Opacity = 1;
                        GucArayuzuKullanimAmaciList.IsEnabled = true;
                        GucArayuzuKullanimAmaciList.Opacity = 1;
                        GucArayuzuGerilimTipiList.IsEnabled = true;
                        GucArayuzuGerilimTipiList.Opacity = 1;
                        ag2.IsEnabled = true;
                        ag2.Opacity = 1;
                        ag4.IsEnabled = true;
                        ag4.Opacity = 1;
                        ag6.IsEnabled = true;
                        ag6.Opacity = 1;
                        ag8.IsEnabled = true;
                        ag8.Opacity = 1;
                        ag10.IsEnabled = true;
                        ag10.Opacity = 1;
                        ag12.IsEnabled = true;
                        ag12.Opacity = 1;
                        ag14.IsEnabled = true;
                        ag14.Opacity = 1;
                        ag16.IsEnabled = true;
                        ag16.Opacity = 1;
                        GucArayuzuPortList.IsEnabled = true;
                        GucArayuzuPortList.Opacity = 1;
                        GucArayuzuEkleBtn.IsEnabled = true;
                        GucArayuzuEkleBtn.Opacity = 1;

                        foreach (var gucArayuzu in gucArayuzuList)
                        {
                            gucArayuzu.IsEnabled = true;
                        }

                        SaveBtn.IsEnabled = true;
                        SaveBtn.Opacity = 1;
                    }
                    else
                    {
                        fromNode = true;
                        //GucUreticiTab
                        GucUreticiTur.IsEnabled = false;
                        GucUreticiTur.Opacity = 0.25;
                        StokNo.IsEnabled = false;
                        StokNo.Opacity = 0.25;
                        Tanim.IsEnabled = false;
                        Tanim.Opacity = 0.25;
                        Uretici.IsEnabled = false;
                        Uretici.Opacity = 0.25;
                        UreticiParcaNo.IsEnabled = false;
                        UreticiParcaNo.Opacity = 0.25;
                        GirdiGucArayuzuSayisi.IsEnabled = false;
                        GirdiGucArayuzuSayisi.Opacity = 0.25;
                        CiktiGucArayuzuSayisi.IsEnabled = false;
                        CiktiGucArayuzuSayisi.Opacity = 0.25;
                        VerimlilikOrani.IsEnabled = false;
                        VerimlilikOrani.Opacity = 0.25;
                        DahiliGucTuketimDegeri.IsEnabled = false;
                        DahiliGucTuketimDegeri.Opacity = 0.25;
                        Katalog.IsEnabled = false;
                        Katalog.Opacity = 0.25;
                        OpenKatalogBtn.IsEnabled = false;
                        OpenKatalogBtn.Opacity = 0.25;
                        Sembol.IsEnabled = false;
                        Sembol.Opacity = 0.25;
                        OpenSembolBtn.IsEnabled = false;
                        OpenSembolBtn.Opacity = 0.25;
                        DownloadButton.IsEnabled = true;
                        DownloadButton.Opacity = 1;
                        DeleteButton.IsEnabled = false;
                        DeleteButton.Opacity = 0.25;

                        //GucArayuzuTab
                        GucArayuzuAdi.IsEnabled = false;
                        GucArayuzuAdi.Opacity = 0.25;
                        GucArayuzuKullanimAmaciList.IsEnabled = false;
                        GucArayuzuKullanimAmaciList.Opacity = 0.25;
                        GucArayuzuGerilimTipiList.IsEnabled = false;
                        GucArayuzuGerilimTipiList.Opacity = 0.25;
                        ag2.IsEnabled = false;
                        ag2.Opacity = 0.25;
                        ag4.IsEnabled = false;
                        ag4.Opacity = 0.25;
                        ag6.IsEnabled = false;
                        ag6.Opacity = 0.25;
                        ag8.IsEnabled = false;
                        ag8.Opacity = 0.25;
                        ag10.IsEnabled = false;
                        ag10.Opacity = 0.25;
                        ag12.IsEnabled = false;
                        ag12.Opacity = 0.25;
                        ag14.IsEnabled = false;
                        ag14.Opacity = 0.25;
                        ag16.IsEnabled = false;
                        ag16.Opacity = 0.25;
                        GucArayuzuPortList.IsEnabled = false;
                        GucArayuzuPortList.Opacity = 0.25;
                        GucArayuzuEkleBtn.IsEnabled = false;
                        GucArayuzuEkleBtn.Opacity = 0.25;

                        foreach (var gucArayuzu in gucArayuzuList)
                        {
                            gucArayuzu.IsEnabled = false;
                        }

                        SaveBtn.IsEnabled = false;
                        SaveBtn.Opacity = 0.25;
                    }
                }
            }
            else
            {
                gucArayuzuList = new List<GucArayuzu>();

                DownloadButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;

                GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }

            if (isEditMode && !fromNode)
            {
                if (!string.IsNullOrEmpty(VerimlilikOrani.Text) && VerimlilikOrani.Text != " ")
                {
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    gucUretici.DahiliGucTuketimDegeri = null;

                    VerimlilikOrani.IsEnabled = true;
                    VerimlilikOrani.Opacity = 1;
                }
                else
                {
                    DahiliGucTuketimDegeri.IsEnabled = true;
                    DahiliGucTuketimDegeri.Opacity = 1;

                    VerimlilikOrani.IsEnabled = false;
                    VerimlilikOrani.Opacity = 0.25;
                    gucUretici.VerimlilikDegeri = null;
                }

                if (gucUretici.GucUreticiTurId == 1)
                {
                    VerimlilikOrani.IsEnabled = false;
                    VerimlilikOrani.Opacity = 0.25;
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    GirdiGucArayuzuSayisiRequiredLabel.Content = "";
                    GirdiGucArayuzuSayisi.IsEnabled = false;
                    GirdiGucArayuzuSayisi.Opacity = 0.25;
                }
                else
                {
                    VerimlilikOrani.IsEnabled = true;
                    VerimlilikOrani.Opacity = 1;
                    DahiliGucTuketimDegeri.IsEnabled = true;
                    DahiliGucTuketimDegeri.Opacity = 1;
                    GirdiGucArayuzuSayisiRequiredLabel.Content = "*";
                    GirdiGucArayuzuSayisi.IsEnabled = true;
                    GirdiGucArayuzuSayisi.Opacity = 1;
                }
            }
        }
        #endregion

        #region PopupCloseEvents
        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonGucUreticiPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        #endregion

        #region BorderVlidates
        private void ValidateGucUreticiiFieldBorders()
        {
            GucUreticiTur.BorderBrush = new SolidColorBrush(Colors.Transparent);
            StokNo.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Tanim.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Uretici.BorderBrush = new SolidColorBrush(Colors.Transparent);
            UreticiParcaNo.BorderBrush = new SolidColorBrush(Colors.Transparent);
            GirdiGucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            CiktiGucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            VerimlilikOrani.BorderBrush = new SolidColorBrush(Colors.Transparent);
            DahiliGucTuketimDegeri.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Katalog.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Sembol.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void ValidateGucArayuzuFiledBorders()
        {
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi)
            {
                GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag7.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag10.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag12.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag14.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag16.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }
        #endregion

        #region TabTransitionEvents

        public void ShowGucUreticiTab()
        {
            GucUreticiTab.IsSelected = false;
            GucArayuzuTab.IsSelected = true;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 22;
            Left = 515;
            Width = 890;
            Height = 995;
            GucUreticiTab.Width = 441;
            GucArayuzuTab.Width = 441;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            GucArayuzuTab.DataContext = null;
            ListGerilimTipi();
            ListKullanimAmaciForGucArayuzu();
            GucArayuzuTab.DataContext = gucArayuzu;


        }

        private void GucUreticiNextButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateGucUreticiiFieldBorders();
            gucUretici.TipId = (int)TipEnum.GucUretici;         

            var validationContext = new ValidationContext(gucUretici, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucUretici, validationContext, results, true))
            {
                if ((!string.IsNullOrEmpty(VerimlilikOrani.Text) && Convert.ToDecimal(VerimlilikOrani.Text) > 0 && Convert.ToDecimal(VerimlilikOrani.Text) <= 100)
                    || (!string.IsNullOrEmpty(DahiliGucTuketimDegeri.Text) && Convert.ToDecimal(DahiliGucTuketimDegeri.Text) != 0))
                {
                    if (oldGucUretici != null)
                    {
                        if (gucUretici.GirdiGucArayuzuSayisi < oldGucUretici.GirdiGucArayuzuSayisi || gucUretici.CiktiGucArayuzuSayisi < oldGucUretici.CiktiGucArayuzuSayisi)
                        {
                            bool girdiMi = false;
                            bool ciktiMi = false;

                            if (gucUretici.GirdiGucArayuzuSayisi < oldGucUretici.GirdiGucArayuzuSayisi)
                            {
                                girdiMi = true;
                            }

                            if (gucUretici.CiktiGucArayuzuSayisi < oldGucUretici.CiktiGucArayuzuSayisi)
                            {
                                ciktiMi = true;
                            }

                            this.IsEnabled = false;
                            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                            blur.Radius = 2;
                            this.Effect = blur;
                            DeleteGucUreticiGucArayuzuPopupWindow popup = new DeleteGucUreticiGucArayuzuPopupWindow(girdiMi, ciktiMi);
                            popup.Owner = this;
                            popup.ShowDialog();
                        }
                        else
                        {
                            ShowGucUreticiTab();
                        }
                    }
                    else
                    {
                        ShowGucUreticiTab();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(VerimlilikOrani.Text) || Convert.ToDecimal(VerimlilikOrani.Text) == 0 && Convert.ToDecimal(VerimlilikOrani.Text) > 100)
                    {
                        VerimlilikOrani.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    if (string.IsNullOrEmpty(DahiliGucTuketimDegeri.Text) || Convert.ToDecimal(DahiliGucTuketimDegeri.Text) == 0)
                    {
                        DahiliGucTuketimDegeri.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
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
                            Style style = Application.Current.FindResource("StyleTextBox") as Style;
                            StokNo.Style = style;
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

        private void GucArayuzuPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 212;
            Left = 720;
            GucArayuzuTab.IsSelected = false;
            GucUreticiTab.IsSelected = true;
            Width = 480;
            Height = 655;
            GucUreticiTab.Width = 236;
            GucArayuzuTab.Width = 236;

            if ((gucUretici.GirdiGucArayuzuSayisi.HasValue ? gucUretici.GirdiGucArayuzuSayisi : 0) + gucUretici.CiktiGucArayuzuSayisi == gucArayuzuList.Count)
            {
                oldGucUretici = (GucUretici)gucUretici.Clone();
            }
        }
        #endregion

        #region Save/UpdateEvent
        private void Save_GucUretici(object sender, RoutedEventArgs e)
        {
            if (gucArayuzuList.Count == gucUretici.CiktiGucArayuzuSayisi + (gucUretici.GirdiGucArayuzuSayisi.HasValue ? gucUretici.GirdiGucArayuzuSayisi : 0))
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    response = service.SaveGucUretici(gucUretici, gucArayuzuList);
                }
                else
                {
                    response = service.UpdateGucUretici(gucUretici, gucArayuzuList);
                }

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
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, bütün güç arayüzleri için veri girişi yapınız!";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }
        #endregion

        #region KatalogFileEvents
        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {
                gucUretici.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = gucUretici.KatalogDosyaAdi;
                gucUretici.Katalog = File.ReadAllBytes(openFileDialog.FileName);

                DownloadButton.Visibility = Visibility.Visible;
                DownloadButton.Opacity = 1;
                DownloadButton.IsEnabled = true;

                DeleteButton.Visibility = Visibility.Visible;
                DeleteButton.Opacity = 1;
                DeleteButton.IsEnabled = true;
            }
        }

        private void BtnDeleteKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            gucUretici.KatalogDosyaAdi = null;
            Katalog.Text = gucUretici.KatalogDosyaAdi;
            gucUretici.Katalog = null;

            DownloadButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        #endregion

        #region OpenSembolFileDialogEvent
        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\SEMA_Data\\StreamingAssets\\AYP\\SembolKutuphanesi";
            openFileDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SembolKutuphanesi");
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                gucUretici.SembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = gucUretici.SembolDosyaAdi;
                gucUretici.Sembol = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        #endregion

        #region GetListEvents
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
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir güç üreici tanımlayınız.";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }

        private void ListKullanimAmaciForGucArayuzu()
        {
            if (gucUretici.GirdiGucArayuzuSayisi == null)
            {
                gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
                gucArayuzu.KullanimAmaciList = gucArayuzu.KullanimAmaciList.Where(x => x.Id != (int)KullanimAmaciEnum.Girdi).ToList();
            }
            else
            {
                gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            }

            if (gucArayuzu.KullanimAmaciList.Count() > 0)
            {
                if(gucUretici.GirdiGucArayuzuSayisi == null)
                {
                    gucArayuzu.KullanimAmaciId = (int)KullanimAmaciEnum.Cikti;
                }
                else
                {
                    gucArayuzu.KullanimAmaciId = (int)KullanimAmaciEnum.Girdi;
                }
            }
        }

        private void ListPortForGucArayuzu()
        {
            gucArayuzu.PortList = new List<string>();

            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                for (int i = 1; i <= gucUretici.CiktiGucArayuzuSayisi; i++)
                {
                    gucArayuzu.PortList.Add("Port " + i);
                }
            }
            else
            {
                for (int i = 1; i <= gucUretici.GirdiGucArayuzuSayisi; i++)
                {
                    gucArayuzu.PortList.Add("Port " + i);
                }
            }

            GucArayuzuPortList.ItemsSource = gucArayuzu.PortList;

            if (string.IsNullOrEmpty(gucArayuzu.Port))
            {
                if (gucArayuzu.PortList.Count > 0)
                {
                    gucArayuzu.Port = gucArayuzu.PortList[0];
                    GucArayuzuPortList.SelectedItem = gucArayuzu.Port;
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

        #endregion

        #region KullanimAmaciSelectionChangedEvent
        private void GucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                ag1.Content = "Girdi Durağan Gerilim Değeri 1";
                ag11.Content = "Girdi Tükettiği Güç Miktarı";
                ag13.Content = "Çıktı Durağan Gerilim Değeri *";
                ag15.Content = "Çıktı Ürettiği Güç Kapasitesi *";
            }
            else
            {
                ag1.Content = "Girdi Durağan Gerilim Değeri 1 *";
                ag11.Content = "Girdi Tükettiği Güç Miktarı *";
                ag13.Content = "Çıktı Durağan Gerilim Değeri";
                ag15.Content = "Çıktı Ürettiği Güç Kapasitesi";
            }

            if (!fromNode)
            {
                if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                {
                    ag2.IsEnabled = false; ag2.Opacity = 0.25;
                    gucArayuzu.GirdiDuraganGerilimDegeri1 = null;

                    ag4.IsEnabled = false; ag4.Opacity = 0.25;
                    gucArayuzu.GirdiDuraganGerilimDegeri2 = null;

                    ag6.IsEnabled = false; ag6.Opacity = 0.25;
                    gucArayuzu.GirdiDuraganGerilimDegeri3 = null;

                    ag8.IsEnabled = false; ag8.Opacity = 0.25;
                    gucArayuzu.GirdiMinimumGerilimDegeri = null;

                    ag10.IsEnabled = false; ag10.Opacity = 0.25;
                    gucArayuzu.GirdiMaksimumGerilimDegeri = null;

                    ag12.IsEnabled = false; ag12.Opacity = 0.25;
                    gucArayuzu.GirdiTukettigiGucMiktari = null;

                    ag14.IsEnabled = true; ag14.Opacity = 1;
                    ag16.IsEnabled = true; ag16.Opacity = 1;
                }
                else
                {
                    ag2.IsEnabled = true; ag2.Opacity = 1;
                    ag4.IsEnabled = true; ag4.Opacity = 1;
                    ag6.IsEnabled = true; ag6.Opacity = 1;
                    ag8.IsEnabled = true; ag8.Opacity = 1;
                    ag10.IsEnabled = true; ag10.Opacity = 1;
                    ag12.IsEnabled = true; ag12.Opacity = 1;
                    ag14.IsEnabled = false; ag14.Opacity = 0.25;
                    gucArayuzu.CiktiDuraganGerilimDegeri = null;

                    ag16.IsEnabled = false; ag16.Opacity = 0.25;
                    gucArayuzu.CiktiUrettigiGucKapasitesi = null;
                }
            }

            ListPortForGucArayuzu();
        }
        #endregion

        #region NumberValidationEvent
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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

        private void NegativeDecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    if (ch.Equals('.') || ch.Equals(',') || ch.Equals('-'))
                    {
                        if (ch.Equals('-'))
                        {
                            int seperatorCount = textBox.Text.Where(t => t.Equals('-')).Count();

                            if (seperatorCount == 0 && textBox.Text.Length == 0)
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
                            int seperatorCount = textBox.Text.Where(t => t.Equals('.') || t.Equals(',')).Count();

                            if (seperatorCount < 1)
                            {
                                if (textBox.Text.Length > 0 && textBox.Text[0] != '-')
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
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(textBox.Text) && textBox.Text[0] == '-')
                    {
                        if (textBox.Text.Length == 1)
                        {
                            if (e.Text == "1")
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
                        e.Handled = false;
                    }
                }
            }
        }
        #endregion

        #region DownloadKatalogEvent
        private void BtnDownloadKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath + "\\" + gucUretici.KatalogDosyaAdi;

                    try
                    {
                        File.WriteAllBytes(path, gucUretici.Katalog);
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = "İşlem başarı ile gerçekleştirildi";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                    catch (Exception exception)
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = "İşlem başarısız oldu";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
            }
        }

        #endregion

        #region TextChangedEvents
        private void VerimlilikOrani_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!fromNode)
            {
                if (!string.IsNullOrEmpty(VerimlilikOrani.Text) && VerimlilikOrani.Text != " ")
                {
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    gucUretici.DahiliGucTuketimDegeri = null;
                }
                else
                {
                    DahiliGucTuketimDegeri.IsEnabled = true;
                    DahiliGucTuketimDegeri.Opacity = 1;
                }
            }
        }

        private void DahiliGucTuketimDegeri_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!fromNode)
            {
                if (!string.IsNullOrEmpty(DahiliGucTuketimDegeri.Text) && DahiliGucTuketimDegeri.Text != " ")
                {
                    VerimlilikOrani.IsEnabled = false;
                    VerimlilikOrani.Opacity = 0.25;
                    gucUretici.VerimlilikDegeri = null;
                }
                else
                {
                    VerimlilikOrani.IsEnabled = true;
                    VerimlilikOrani.Opacity = 1;
                }
            }
        }
        #endregion

        #region TableEvents
        private void ButtonAddGucArayuzu_Click(object sender, RoutedEventArgs e)
        {
            ValidateGucArayuzuFiledBorders();
            bool validMi = false;

            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                validMi = CiktiGucArayuzuValidation();
            }
            else
            {
                validMi = GirdiGucArayuzuValidation();
            }

            if (validMi)
            {
                gucArayuzu.TipId = (int)TipEnum.GucUreticiGucArayuzu;

                var validationContext = new ValidationContext(gucArayuzu, null, null);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
                {
                    gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                    gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(fo => fo.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

                    bool isUpdate = false;
                    if (checkedGucArayuzuRow != null)
                    {
                        isUpdate = true;

                        var ctx = checkedGucArayuzuRow.DataContext;
                        var obj = (GucArayuzu)ctx;
                        gucArayuzuList.Remove(obj);
                        checkedGucArayuzuRow = null;
                    }

                    if (!gucArayuzuList.Any(x => x.Port == gucArayuzu.Port && x.KullanimAmaciId == gucArayuzu.KullanimAmaciId))
                    {
                        if ((int)KullanimAmaciEnum.Cikti == gucArayuzu.KullanimAmaciId)
                        {
                            if (gucUretici.GirdiGucArayuzuSayisi > 0)
                            {
                                decimal? totalGirdiKapasitesi = gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Select(s => s.GirdiTukettigiGucMiktari).Sum();
                                decimal? totalCiktiKapasitesi = gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).Select(s => s.CiktiUrettigiGucKapasitesi).Sum();

                                if (gucUretici.VerimlilikDegeri.HasValue && gucUretici.VerimlilikDegeri != 0)
                                {
                                    totalGirdiKapasitesi = (gucUretici.VerimlilikDegeri * totalGirdiKapasitesi) / 100;
                                }
                                else
                                {
                                    totalGirdiKapasitesi = totalGirdiKapasitesi - gucUretici.DahiliGucTuketimDegeri;
                                }

                                if (totalGirdiKapasitesi.Value <= 0)
                                {
                                    validMi = false;

                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Lütfen, çıktı tanımlamak için en az bir girdi arayüzü tanımlayınız!";
                                    nfp.Owner = Owner;
                                    nfp.Show();
                                }
                                else
                                {
                                    if ((gucArayuzu.CiktiUrettigiGucKapasitesi + totalCiktiKapasitesi) > totalGirdiKapasitesi)
                                    {
                                        validMi = false;

                                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                                        nfp.msg.Text = "Tanımlanan toplam girdi kapasitesini aştınız!";
                                        nfp.Owner = Owner;
                                        nfp.Show();
                                    }
                                }
                            }
                            else
                            {
                                validMi = true;
                            }
                        }
                        else
                        {
                            bool flag = false;
                            
                                if(gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Count() + 1 == gucUretici.GirdiGucArayuzuSayisi)
                                {
                                    flag = true;
                                }
                            
                            if (flag)
                            {
                                decimal? totalCiktiKapasitesi = gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).Select(s => s.CiktiUrettigiGucKapasitesi).Sum();
                                decimal? totalGirdiKapasitesi = gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi).Select(s => s.GirdiTukettigiGucMiktari).Sum();
                                totalGirdiKapasitesi += gucArayuzu.GirdiTukettigiGucMiktari;

                                if (totalGirdiKapasitesi >= totalCiktiKapasitesi)
                                {
                                    validMi = true;
                                }
                                else
                                {
                                    validMi = false;

                                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                                    nfp.msg.Text = "Toplam girdi kapasitesi, çıktı kapasitesinden büyük olmalıdır.";
                                    nfp.Owner = Owner;
                                    nfp.Show();
                                }
                            }
                        }

                        if (validMi)
                        {
                            BtnGucArayuzEkle.Text = "Ekle";
                            BtnGucArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

                            GucUreticiGucArayuzDataGrid.ItemsSource = null;

                            gucArayuzuList.Add(gucArayuzu);
                            GucUreticiGucArayuzDataGrid.ItemsSource = gucArayuzuList;
                            GucUreticiGucArayuzDataGrid.Visibility = Visibility.Visible;
                            GucArayuzuNoDataRow.Visibility = Visibility.Hidden;

                            GucArayuzuTab.DataContext = null;
                            gucArayuzu = new GucArayuzu();
                            ListGerilimTipi();
                            ListKullanimAmaciForGucArayuzu();
                            GucArayuzuTab.DataContext = gucArayuzu;
                        }
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = gucArayuzu.KL_KullanimAmaci.Ad + " kullanım amacı ile " + gucArayuzu.Port + " için veri girilmiştir"; ;
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
                else
                {
                    foreach (var result in results)
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            if (memberName == "Adi")
                            {
                                GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "Port")
                            {
                                GucArayuzuPortList.BorderBrush = new SolidColorBrush(Colors.Red);
                            }
                        }
                    }
                }
            }

        }

        private bool GirdiGucArayuzuValidation()
        {
            bool validMi = true;

            if (string.IsNullOrEmpty(ag2.Text) || Convert.ToDecimal(ag2.Text) == 0)
            {
                validMi = false;
                ag2.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (string.IsNullOrEmpty(ag12.Text) || Convert.ToDecimal(ag12.Text) == 0)
            {
                validMi = false;
                ag12.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (validMi)
            {
                if (!string.IsNullOrEmpty(ag10.Text) && Convert.ToDecimal(ag10.Text) > 0)
                {
                    if (!string.IsNullOrEmpty(ag8.Text) && Convert.ToDecimal(ag8.Text) > 0)
                    {
                        if (Convert.ToDecimal(ag8.Text) >= Convert.ToDecimal(ag10.Text))
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Minimum gerilim değeri, maksimum gerilim değerine eşit veya büyük olamaz!";
                            nfp.Owner = Owner;
                            nfp.Show();

                            validMi = false;
                            ag8.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                    else
                    {
                        validMi = false;
                        ag8.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                }
            }

            if (validMi)
            {
                if (!string.IsNullOrEmpty(ag8.Text) && Convert.ToDecimal(ag8.Text) > 0)
                {
                    if (!string.IsNullOrEmpty(ag10.Text) && Convert.ToDecimal(ag10.Text) > 0)
                    {
                        if (Convert.ToDecimal(ag10.Text) <= Convert.ToDecimal(ag8.Text))
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Maksimum gerilim değeri, minimum gerilim değerine eşit veya küçük olamaz!";
                            nfp.Owner = Owner;
                            nfp.Show();

                            validMi = false;
                            ag10.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                    else
                    {
                        validMi = false;
                        ag10.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                }
            }

            return validMi;
        }

        private bool CiktiGucArayuzuValidation()
        {
            bool validMi = true;

            if (string.IsNullOrEmpty(ag14.Text) || Convert.ToDecimal(ag14.Text) == 0)
            {
                validMi = false;
                ag14.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (string.IsNullOrEmpty(ag16.Text) || Convert.ToDecimal(ag16.Text) == 0)
            {
                validMi = false;
                ag16.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            return validMi;
        }

        private void GucArayuzuRow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkedGucArayuzuRow != null)
            {
                checkedGucArayuzuRow.IsChecked = false;
            }

            BtnGucArayuzEkle.Text = "Güncelle";
            BtnGucArayuzEkle.Margin = new Thickness(80, 0, 0, 0);

            GucArayuzuTab.DataContext = null;
            checkedGucArayuzuRow = (CheckBox)sender;
            var ctx = checkedGucArayuzuRow.DataContext;
            gucArayuzu = (GucArayuzu)((GucArayuzu)ctx).Clone();
            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void GucArayuzuRow_Unchecked(object sender, RoutedEventArgs e)
        {
            BtnGucArayuzEkle.Text = "Ekle";
            BtnGucArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

            checkedGucArayuzuRow = null;

            GucArayuzuTab.DataContext = null;
            gucArayuzu = new GucArayuzu();
            ListGerilimTipi();
            ListKullanimAmaciForGucArayuzu();
            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void GucArayuzuDelete_Row(object sender, RoutedEventArgs e)
        {
            if (!fromNode)
            {
                BtnGucArayuzEkle.Text = "Ekle";
                BtnGucArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

                var row = (Button)sender;
                var ctx = row.DataContext;
                var obj = (GucArayuzu)ctx;

                GucUreticiGucArayuzDataGrid.ItemsSource = null;
                gucArayuzuList.Remove(obj);
                GucUreticiGucArayuzDataGrid.ItemsSource = gucArayuzuList;

                if (gucArayuzuList.Count == 0)
                {
                    GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                    GucArayuzuNoDataRow.Visibility = Visibility.Visible;
                }

                checkedGucArayuzuRow = null;

                GucArayuzuTab.DataContext = null;
                gucArayuzu = new GucArayuzu();
                ListGerilimTipi();
                ListKullanimAmaciForGucArayuzu();
                GucArayuzuTab.DataContext = gucArayuzu;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Seçtiğiniz güç arayüzü proje içerisinde kullanımda olduğu için silinemez.";
                nfp.Owner = Owner;
                nfp.Show();
            }

        }

        private void GucArayuzuDuplicate_Row(object sender, RoutedEventArgs e)
        {
            var row = (Button)sender;
            var ctx = row.DataContext;
            var obj = (GucArayuzu)ctx;

            var portList = new List<string>();

            if (obj.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                for (int i = 1; i <= gucUretici.CiktiGucArayuzuSayisi; i++)
                {
                    if (!gucArayuzuList.Where(x => x.Port == "Port " + i && x.KullanimAmaciId == obj.KullanimAmaciId).Any())
                    {
                        portList.Add("Port " + i);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= gucUretici.GirdiGucArayuzuSayisi; i++)
                {
                    if (!gucArayuzuList.Where(x => x.Port == "Port " + i && x.KullanimAmaciId == obj.KullanimAmaciId).Any())
                    {
                        portList.Add("Port " + i);
                    }
                }
            }

            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            DuplicateArayuzPopupWindow popup = new DuplicateArayuzPopupWindow((int)TipEnum.GucUretici, null, obj, portList);
            popup.Owner = this;
            popup.ShowDialog();
        }

        public void UpdateGucArayuzuTable()
        {
            GucUreticiGucArayuzDataGrid.ItemsSource = null;
            GucUreticiGucArayuzDataGrid.ItemsSource = gucArayuzuList;
        }
        #endregion

        #region GucUreticiTurSelectionChanged
        private void GucUreticiTur_SelectionChanged(object sender, EventArgs e)
        {
            if (GucUreticiTab.DataContext != null)
            {
                if (gucUretici.GucUreticiTurId == 1)
                {
                    GucUreticiTab.DataContext = null;
                    gucUretici.VerimlilikDegeri = 100;
                    gucUretici.DahiliGucTuketimDegeri = null;
                    gucUretici.GirdiGucArayuzuSayisi = null;
                    GucUreticiTab.DataContext = gucUretici;

                    VerimlilikOrani.IsEnabled = false;
                    VerimlilikOrani.Opacity = 0.25;
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    GirdiGucArayuzuSayisiRequiredLabel.Content = "";
                    GirdiGucArayuzuSayisi.IsEnabled = false;
                    GirdiGucArayuzuSayisi.Opacity = 0.25;

                    gucArayuzuList = gucArayuzuList.Where(x => x.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti).ToList();
                }
                else
                {
                    GucUreticiTab.DataContext = null;
                    gucUretici.VerimlilikDegeri = null;
                    gucUretici.DahiliGucTuketimDegeri = null;
                    gucUretici.GirdiGucArayuzuSayisi = 0;
                    GucUreticiTab.DataContext = gucUretici;

                    VerimlilikOrani.IsEnabled = true;
                    VerimlilikOrani.Opacity = 1;
                    DahiliGucTuketimDegeri.IsEnabled = true;
                    DahiliGucTuketimDegeri.Opacity = 1;
                    GirdiGucArayuzuSayisiRequiredLabel.Content = "*";
                    GirdiGucArayuzuSayisi.IsEnabled = true;
                    GirdiGucArayuzuSayisi.Opacity = 1;

                    gucArayuzuList = gucArayuzuList.Where(x => x.KullanimAmaciId != (int)KullanimAmaciEnum.Cikti).ToList();
                }
            }
        }
        #endregion
    }
}
