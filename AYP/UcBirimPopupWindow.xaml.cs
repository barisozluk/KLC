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
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AYP
{
    /// <summary>
    /// Interaction logic for UcBirimPopupWindow.xaml
    /// </summary>
    public partial class UcBirimPopupWindow : Window
    {
        private IUcBirimService service;
        private IKodListeService kodListeService;

        UcBirim ucBirim;
        UcBirim oldUcBirim;
        AgArayuzu agArayuzu;
        GucArayuzu gucArayuzu;

        public List<AgArayuzu> agArayuzuList;
        private CheckBox checkedAgArayuzuRow = null;
        public List<GucArayuzu> gucArayuzuList;
        private CheckBox checkedGucArayuzuRow = null;

        public bool isEditMode = false;
        public bool fromNode = false;

        public UcBirimPopupWindow(UcBirim _ucBirim, bool fromNode)
        {

            this.fromNode = fromNode;

            agArayuzu = new AgArayuzu();
            gucArayuzu = new GucArayuzu();
            if (_ucBirim != null)
            {
                oldUcBirim = (UcBirim)_ucBirim.Clone();
                ucBirim = (UcBirim)_ucBirim.Clone();
                isEditMode = true;
            }
            else
            {
                ucBirim = new UcBirim();
                isEditMode = false;
            }

            service = new UcBirimService();
            kodListeService = new KodListeService();
            InitializeComponent();
            SetUcBirimTurList();
            UcBirimTab.DataContext = ucBirim;

            if (ucBirim.UcBirimTurList.Count() == 0)
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
                agArayuzuList = service.ListUcBirimAgArayuzu(ucBirim.Id);
                UcBirimAgArayuzDataGrid.ItemsSource = agArayuzuList;
                if (agArayuzuList.Count > 0)
                {
                    UcBirimAgArayuzDataGrid.Visibility = Visibility.Visible;
                    AgArayuzuNoDataRow.Visibility = Visibility.Hidden;
                }
                else
                {
                    UcBirimAgArayuzDataGrid.Visibility = Visibility.Hidden;
                    AgArayuzuNoDataRow.Visibility = Visibility.Visible;
                }

                gucArayuzuList = service.ListUcBirimGucArayuzu(ucBirim.Id);
                UcBirimGucArayuzDataGrid.ItemsSource = gucArayuzuList;
                if (gucArayuzuList.Count > 0)
                {
                    UcBirimGucArayuzDataGrid.Visibility = Visibility.Visible;
                    GucArayuzuNoDataRow.Visibility = Visibility.Hidden;
                }
                else
                {
                    UcBirimGucArayuzDataGrid.Visibility = Visibility.Hidden;
                    GucArayuzuNoDataRow.Visibility = Visibility.Visible;
                }

                if (string.IsNullOrEmpty(ucBirim.KatalogDosyaAdi))
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
                    //UcBirimTab
                    UcBirimTur.IsEnabled = false;
                    UcBirimTur.Opacity = 0.25;
                    StokNo.IsEnabled = false;
                    StokNo.Opacity = 0.25;
                    Tanim.IsEnabled = false;
                    Tanim.Opacity = 0.25;
                    Uretici.IsEnabled = false;
                    Uretici.Opacity = 0.25;
                    UreticiParcaNo.IsEnabled = false;
                    UreticiParcaNo.Opacity = 0.25;
                    GirdiAgArayuzuSayisi.IsEnabled = false;
                    GirdiAgArayuzuSayisi.Opacity = 0.25;
                    CiktiAgArayuzuSayisi.IsEnabled = false;
                    CiktiAgArayuzuSayisi.Opacity = 0.25;
                    GucArayuzuSayisi.IsEnabled = false;
                    GucArayuzuSayisi.Opacity = 0.25;
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

                    //AgArayuzuTab
                    AgArayuzuAdi.IsEnabled = false;
                    AgArayuzuAdi.Opacity = 0.25;
                    AgArayuzuKullanimAmaciList.IsEnabled = false;
                    AgArayuzuKullanimAmaciList.Opacity = 0.25;
                    AgArayuzuFizikselOrtamList.IsEnabled = false;
                    AgArayuzuFizikselOrtamList.Opacity = 0.25;
                    AgArayuzuKapasiteList.IsEnabled = false;
                    AgArayuzuKapasiteList.Opacity = 0.25;
                    AgArayuzuPortList.IsEnabled = false;
                    AgArayuzuPortList.Opacity = 0.25;
                    AgArayuzuEkleBtn.IsEnabled = false;
                    AgArayuzuEkleBtn.Opacity = 0.25;

                    foreach (var agArayuzu in agArayuzuList)
                    {
                        agArayuzu.IsEnabled = false;
                    }

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

                    if (!nodes.Any(n => n.Id == ucBirim.Id && n.TypeId == (int)TipEnum.UcBirim))
                    {
                        fromNode = false;
                        //UcBirimTab
                        UcBirimTur.IsEnabled = true;
                        UcBirimTur.Opacity = 1;
                        StokNo.IsEnabled = true;
                        StokNo.Opacity = 1;
                        Tanim.IsEnabled = true;
                        Tanim.Opacity = 1;
                        Uretici.IsEnabled = true;
                        Uretici.Opacity = 1;
                        UreticiParcaNo.IsEnabled = true;
                        UreticiParcaNo.Opacity = 1;
                        GirdiAgArayuzuSayisi.IsEnabled = true;
                        GirdiAgArayuzuSayisi.Opacity = 1;
                        CiktiAgArayuzuSayisi.IsEnabled = true;
                        CiktiAgArayuzuSayisi.Opacity = 1;
                        GucArayuzuSayisi.IsEnabled = true;
                        GucArayuzuSayisi.Opacity = 1;
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

                        //AgArayuzuTab
                        AgArayuzuAdi.IsEnabled = true;
                        AgArayuzuAdi.Opacity = 1;
                        AgArayuzuKullanimAmaciList.IsEnabled = true;
                        AgArayuzuKullanimAmaciList.Opacity = 1;
                        AgArayuzuFizikselOrtamList.IsEnabled = true;
                        AgArayuzuFizikselOrtamList.Opacity = 1;
                        AgArayuzuKapasiteList.IsEnabled = true;
                        AgArayuzuKapasiteList.Opacity = 1;
                        AgArayuzuPortList.IsEnabled = true;
                        AgArayuzuPortList.Opacity = 1;
                        AgArayuzuEkleBtn.IsEnabled = true;
                        AgArayuzuEkleBtn.Opacity = 1;

                        foreach (var agArayuzu in agArayuzuList)
                        {
                            agArayuzu.IsEnabled = true;
                        }

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
                        //UcBirimTab
                        UcBirimTur.IsEnabled = false;
                        UcBirimTur.Opacity = 0.25;
                        StokNo.IsEnabled = false;
                        StokNo.Opacity = 0.25;
                        Tanim.IsEnabled = false;
                        Tanim.Opacity = 0.25;
                        Uretici.IsEnabled = false;
                        Uretici.Opacity = 0.25;
                        UreticiParcaNo.IsEnabled = false;
                        UreticiParcaNo.Opacity = 0.25;
                        GirdiAgArayuzuSayisi.IsEnabled = false;
                        GirdiAgArayuzuSayisi.Opacity = 0.25;
                        CiktiAgArayuzuSayisi.IsEnabled = false;
                        CiktiAgArayuzuSayisi.Opacity = 0.25;
                        GucArayuzuSayisi.IsEnabled = false;
                        GucArayuzuSayisi.Opacity = 0.25;
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

                        //AgArayuzuTab
                        AgArayuzuAdi.IsEnabled = false;
                        AgArayuzuAdi.Opacity = 0.25;
                        AgArayuzuKullanimAmaciList.IsEnabled = false;
                        AgArayuzuKullanimAmaciList.Opacity = 0.25;
                        AgArayuzuFizikselOrtamList.IsEnabled = false;
                        AgArayuzuFizikselOrtamList.Opacity = 0.25;
                        AgArayuzuKapasiteList.IsEnabled = false;
                        AgArayuzuKapasiteList.Opacity = 0.25;
                        AgArayuzuPortList.IsEnabled = false;
                        AgArayuzuPortList.Opacity = 0.25;
                        AgArayuzuEkleBtn.IsEnabled = false;
                        AgArayuzuEkleBtn.Opacity = 0.25;

                        foreach (var agArayuzu in agArayuzuList)
                        {
                            agArayuzu.IsEnabled = false;
                        }

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
                agArayuzuList = new List<AgArayuzu>();
                gucArayuzuList = new List<GucArayuzu>();

                DownloadButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;

                UcBirimAgArayuzDataGrid.Visibility = Visibility.Hidden;
                AgArayuzuNoDataRow.Visibility = Visibility.Visible;

                UcBirimGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Save/UpdateEvent
        private void Save_UcBirim(object sender, RoutedEventArgs e)
        {
            if (gucArayuzuList.Count == ucBirim.GucArayuzuSayisi)
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    int totalCount = 0;

                    if (ucBirim.GirdiAgArayuzuSayisi.HasValue)
                    {
                        totalCount += ucBirim.GirdiAgArayuzuSayisi.Value;
                    }
                    if (ucBirim.CiktiAgArayuzuSayisi.HasValue)
                    {
                        totalCount += ucBirim.CiktiAgArayuzuSayisi.Value;
                    }

                    if (agArayuzuList.Count == totalCount)
                    {
                        response = service.SaveUcBirim(ucBirim, agArayuzuList, gucArayuzuList);
                        OpenResponseModal(response);
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Lütfen, sayısını tanımladığınız bütün ağ arayüzleri için veri girişi yapınız!";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
                else
                {
                    int totalCount = 0;

                    if (ucBirim.GirdiAgArayuzuSayisi.HasValue)
                    {
                        totalCount += ucBirim.GirdiAgArayuzuSayisi.Value;
                    }
                    if (ucBirim.CiktiAgArayuzuSayisi.HasValue)
                    {
                        totalCount += ucBirim.CiktiAgArayuzuSayisi.Value;
                    }

                    if (agArayuzuList.Count == totalCount)
                    {
                        response = service.UpdateUcBirim(ucBirim, agArayuzuList, gucArayuzuList);
                        OpenResponseModal(response);
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = "Lütfen, sayısını tanımladığınız bütün ağ arayüzleri için veri girişi yapınız!";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
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

        #region PopupCloseEvents
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
        #endregion

        #region BorderVlidates
        private void ValidateUcBirimFieldBorders()
        {
            UcBirimTur.BorderBrush = new SolidColorBrush(Colors.Transparent);
            StokNo.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Tanim.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Uretici.BorderBrush = new SolidColorBrush(Colors.Transparent);
            UreticiParcaNo.BorderBrush = new SolidColorBrush(Colors.Transparent);
            GirdiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            CiktiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            GucArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Katalog.BorderBrush = new SolidColorBrush(Colors.Transparent);
            Sembol.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void ValidateAgArayuzuFieldBorders()
        {
            AgArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void ValidateGucArayuzuFiledBorders()
        {
            GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag2.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag7.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag10.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag12.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
        #endregion

        #region TabTransitionEvents

        public void ShowAgArayuzuTab()
        {
            UcBirimTab.IsSelected = false;
            AgArayuzuTab.IsSelected = true;
            AgArayuzuTab.DataContext = null;
            ListKapasite();
            ListFizikselOrtam();
            ListKullanimAmaciForAgArayuzu();
            AgArayuzuTab.DataContext = agArayuzu;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 250;
            Left = 520;
            Width = 880;
            Height = 580;

            UcBirimTab.Width = 290;
            AgArayuzuTab.Width = 291;
            GucArayuzuTab.Width = 291;
        }

        private void UcBirimNextButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateUcBirimFieldBorders();
            ucBirim.TipId = (int)TipEnum.UcBirim;

            var validationContext = new ValidationContext(ucBirim, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(ucBirim, validationContext, results, true))
            {
                if ((!string.IsNullOrEmpty(GirdiAgArayuzuSayisi.Text) && Convert.ToInt32(GirdiAgArayuzuSayisi.Text) > 0) || (!string.IsNullOrEmpty(CiktiAgArayuzuSayisi.Text) && Convert.ToInt32(CiktiAgArayuzuSayisi.Text) > 0))
                {
                    if (oldUcBirim != null)
                    {
                        if (oldUcBirim.GucArayuzuSayisi > ucBirim.GucArayuzuSayisi || ucBirim.GirdiAgArayuzuSayisi < oldUcBirim.GirdiAgArayuzuSayisi || ucBirim.CiktiAgArayuzuSayisi < oldUcBirim.CiktiAgArayuzuSayisi)
                        {
                            bool gucArayuzuMu = false;
                            bool agArayuzuMu = false;
                            bool girdiMi = false;
                            bool ciktiMi = false;

                            if (oldUcBirim.GucArayuzuSayisi > ucBirim.GucArayuzuSayisi)
                            {
                                gucArayuzuMu = true;
                            }

                            if(ucBirim.GirdiAgArayuzuSayisi < oldUcBirim.GirdiAgArayuzuSayisi || ucBirim.CiktiAgArayuzuSayisi < oldUcBirim.CiktiAgArayuzuSayisi)
                            {
                                agArayuzuMu = true;

                                if(ucBirim.GirdiAgArayuzuSayisi < oldUcBirim.GirdiAgArayuzuSayisi)
                                {
                                    girdiMi = true;
                                }

                                if(ucBirim.CiktiAgArayuzuSayisi < oldUcBirim.CiktiAgArayuzuSayisi)
                                {
                                    ciktiMi = true;
                                }
                            }

                            this.IsEnabled = false;
                            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
                            blur.Radius = 2;
                            this.Effect = blur;
                            DeleteUcBirimArayuzPopupWindow popup = new DeleteUcBirimArayuzPopupWindow(agArayuzuMu, girdiMi, ciktiMi, gucArayuzuMu);
                            popup.Owner = this;
                            popup.ShowDialog();
                        }
                        else
                        {
                            ShowAgArayuzuTab();
                        }
                    }
                    else
                    {
                        ShowAgArayuzuTab();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(GirdiAgArayuzuSayisi.Text) || Convert.ToInt32(GirdiAgArayuzuSayisi.Text) == 0)
                    {
                        GirdiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    if (string.IsNullOrEmpty(CiktiAgArayuzuSayisi.Text) || Convert.ToInt32(CiktiAgArayuzuSayisi.Text) == 0)
                    {
                        CiktiAgArayuzuSayisi.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = "Girdi veya çıktı ağ arayüzü sayılarından en az biri 0' dan büyük olmalıdır.";
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
            }
        }

        private void AgArayuzuNextButton_Click(object sender, RoutedEventArgs e)
        {
            UcBirimTab.IsSelected = false;
            GucArayuzuTab.IsSelected = true;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 22;
            Left = 515;
            Width = 890;
            Height = 995;
            UcBirimTab.Width = 292;
            AgArayuzuTab.Width = 295;
            GucArayuzuTab.Width = 295;

            GucArayuzuTab.DataContext = null;
            ListKullanimAmaciForGucArayuzu();
            ListGerilimTipi();
            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void AgArayuzuPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            AgArayuzuTab.IsSelected = false;
            UcBirimTab.IsSelected = true;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 237;
            Left = 730;
            Width = 460;
            Height = 605;
            UcBirimTab.Width = 150;
            AgArayuzuTab.Width = 151;
            GucArayuzuTab.Width = 151;

            if (ucBirim.GirdiAgArayuzuSayisi + ucBirim.CiktiAgArayuzuSayisi == agArayuzuList.Count && ucBirim.GucArayuzuSayisi == gucArayuzuList.Count)
            {
                oldUcBirim = (UcBirim)ucBirim.Clone();
            }
        }

        private void GucArayuzuPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            GucArayuzuTab.IsSelected = false;
            AgArayuzuTab.IsSelected = true;

            AgArayuzuTab.DataContext = null;
            agArayuzu = new AgArayuzu();
            ListKapasite();
            ListFizikselOrtam();
            ListKullanimAmaciForAgArayuzu();
            AgArayuzuTab.DataContext = agArayuzu;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = 250;
            Left = 520;
            Width = 880;
            Height = 580;
            UcBirimTab.Width = 290;
            AgArayuzuTab.Width = 291;
            GucArayuzuTab.Width = 291;
        }
        #endregion

        #region KatalogFileEvents
        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {
                ucBirim.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = ucBirim.KatalogDosyaAdi;
                ucBirim.Katalog = File.ReadAllBytes(openFileDialog.FileName);

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
            ucBirim.KatalogDosyaAdi = null;
            Katalog.Text = ucBirim.KatalogDosyaAdi;
            ucBirim.Katalog = null;

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
                ucBirim.SembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = ucBirim.SembolDosyaAdi;
                ucBirim.Sembol = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        #endregion

        #region GetListEvents
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
                nfp.msg.Text = "Lütfen, en az bir uç birim tanımlayınız";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }

        private void ListKapasite()
        {
            agArayuzu.KapasiteList = kodListeService.ListKapasite();
            if (agArayuzu.KapasiteList.Count() > 0)
            {
                agArayuzu.KapasiteId = agArayuzu.KapasiteList[0].Id;
            }
        }

        private void ListFizikselOrtam()
        {
            agArayuzu.FizikselOrtamList = kodListeService.ListFizikselOrtam();
            if (agArayuzu.FizikselOrtamList.Count() > 0)
            {
                agArayuzu.FizikselOrtamId = agArayuzu.FizikselOrtamList[0].Id;
            }
        }

        private void ListKullanimAmaciForAgArayuzu()
        {
            agArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            if (agArayuzu.KullanimAmaciList.Count() > 0)
            {
                agArayuzu.KullanimAmaciId = agArayuzu.KullanimAmaciList[0].Id;
            }
        }

        private void ListKullanimAmaciForGucArayuzu()
        {
            gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci().Where(x => x.Id == 1).ToList();
            if (gucArayuzu.KullanimAmaciList.Count() > 0)
            {
                gucArayuzu.KullanimAmaciId = gucArayuzu.KullanimAmaciList[0].Id;
            }
        }

        private void ListPortForAgArayuzu()
        {
            agArayuzu.PortList = new List<string>();

            if (agArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                for (int i = 1; i <= ucBirim.CiktiAgArayuzuSayisi; i++)
                {
                    agArayuzu.PortList.Add("Port " + i);
                }
            }
            else
            {
                for (int i = 1; i <= ucBirim.GirdiAgArayuzuSayisi; i++)
                {
                    agArayuzu.PortList.Add("Port " + i);
                }
            }

            AgArayuzuPortList.ItemsSource = agArayuzu.PortList;

            if (string.IsNullOrEmpty(agArayuzu.Port))
            {
                if (agArayuzu.PortList.Count > 0)
                {
                    agArayuzu.Port = agArayuzu.PortList[0];
                    AgArayuzuPortList.SelectedItem = agArayuzu.Port;
                }
            }
        }

        private void ListPortForGucArayuzu()
        {
            gucArayuzu.PortList = new List<string>();

            for (int i = 1; i <= ucBirim.GucArayuzuSayisi; i++)
            {
                gucArayuzu.PortList.Add("Port " + i);
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
        #endregion

        #region DownloadKatalogEvent
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
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                    catch (Exception exception)
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = "İşlem Başarısız Oldu.";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                }
            }
        }
        #endregion

        #region KullanimAmaciSelectionChangedEvents
        private void AgArayuzuKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPortForAgArayuzu();
        }

        private void GucArayuzKullanimAmaci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        #region TableEvents
        private void ButtonAddAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            ValidateAgArayuzuFieldBorders();
            agArayuzu.TipId = (int)TipEnum.UcBirimAgArayuzu;

            var validationContext = new ValidationContext(agArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agArayuzu, validationContext, results, true))
            {
                agArayuzu.KL_KullanimAmaci = agArayuzu.KullanimAmaciList.Where(kal => kal.Id == agArayuzu.KullanimAmaciId).FirstOrDefault();
                agArayuzu.KL_Kapasite = agArayuzu.KapasiteList.Where(kl => kl.Id == agArayuzu.KapasiteId).FirstOrDefault();
                agArayuzu.KL_FizikselOrtam = agArayuzu.FizikselOrtamList.Where(fo => fo.Id == agArayuzu.FizikselOrtamId).FirstOrDefault();

                if (checkedAgArayuzuRow != null)
                {
                    var ctx = checkedAgArayuzuRow.DataContext;
                    var obj = (AgArayuzu)ctx;
                    agArayuzuList.Remove(obj);
                    checkedAgArayuzuRow = null;
                }

                if (!agArayuzuList.Any(x => x.Port == agArayuzu.Port && x.KullanimAmaciId == agArayuzu.KullanimAmaciId))
                {
                    BtnAgArayuzEkle.Text = "Ekle";
                    BtnAgArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

                    UcBirimAgArayuzDataGrid.ItemsSource = null;

                    agArayuzuList.Add(agArayuzu);
                    UcBirimAgArayuzDataGrid.ItemsSource = agArayuzuList;
                    UcBirimAgArayuzDataGrid.Visibility = Visibility.Visible;
                    AgArayuzuNoDataRow.Visibility = Visibility.Hidden;

                    AgArayuzuTab.DataContext = null;
                    agArayuzu = new AgArayuzu();
                    ListKapasite();
                    ListFizikselOrtam();
                    ListKullanimAmaciForAgArayuzu();
                    AgArayuzuTab.DataContext = agArayuzu;
                }
                else
                {
                    NotifyInfoPopup nfp = new NotifyInfoPopup();
                    nfp.msg.Text = agArayuzu.KL_KullanimAmaci.Ad + " kullanım amacı ile " + agArayuzu.Port + " için veri girilmiştir"; ;
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
                            AgArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Red);
                        }

                        if (memberName == "Port")
                        {
                            AgArayuzuPortList.BorderBrush = new SolidColorBrush(Colors.Red);
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

        private void AgArayuzuRow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkedAgArayuzuRow != null)
            {
                checkedAgArayuzuRow.IsChecked = false;
            }

            BtnAgArayuzEkle.Text = "Güncelle";
            BtnAgArayuzEkle.Margin = new Thickness(80, 0, 0, 0);

            AgArayuzuTab.DataContext = null;
            checkedAgArayuzuRow = (CheckBox)sender;
            var ctx = checkedAgArayuzuRow.DataContext;
            agArayuzu = (AgArayuzu)((AgArayuzu)ctx).Clone();

            AgArayuzuTab.DataContext = agArayuzu;
        }

        private void AgArayuzuRow_Unchecked(object sender, RoutedEventArgs e)
        {
            BtnAgArayuzEkle.Text = "Ekle";
            BtnAgArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

            checkedAgArayuzuRow = null;

            AgArayuzuTab.DataContext = null;
            agArayuzu = new AgArayuzu();
            ListKapasite();
            ListFizikselOrtam();
            ListKullanimAmaciForAgArayuzu();
            AgArayuzuTab.DataContext = agArayuzu;
        }

        private void AgArayuzuDelete_Row(object sender, RoutedEventArgs e)
        {
            if (!fromNode)
            {
                BtnAgArayuzEkle.Text = "Ekle";
                BtnAgArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

                var row = (Button)sender;
                var ctx = row.DataContext;
                var obj = (AgArayuzu)ctx;

                UcBirimAgArayuzDataGrid.ItemsSource = null;
                agArayuzuList.Remove(obj);
                UcBirimAgArayuzDataGrid.ItemsSource = agArayuzuList;

                if (agArayuzuList.Count == 0)
                {
                    UcBirimAgArayuzDataGrid.Visibility = Visibility.Hidden;
                    AgArayuzuNoDataRow.Visibility = Visibility.Visible;
                }

                checkedAgArayuzuRow = null;
                AgArayuzuTab.DataContext = null;
                agArayuzu = new AgArayuzu();
                ListKapasite();
                ListFizikselOrtam();
                ListKullanimAmaciForAgArayuzu();
                AgArayuzuTab.DataContext = agArayuzu;
            }
            else
            {
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Seçtiğiniz ağ arayüzü proje içerisinde kullanımda olduğu için silinemez.";
                nfp.Owner = Owner;
                nfp.Show();
            }
        }

        private void AgArayuzuDuplicate_Row(object sender, RoutedEventArgs e)
        {
            var row = (Button)sender;
            var ctx = row.DataContext;
            var obj = (AgArayuzu)ctx;

            var portList = new List<string>();
            if (obj.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                for (int i = 1; i <= ucBirim.CiktiAgArayuzuSayisi; i++)
                {
                    if (!agArayuzuList.Where(x => x.Port == "Port " + i && x.KullanimAmaciId == obj.KullanimAmaciId).Any())
                    {
                        portList.Add("Port " + i);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= ucBirim.GirdiAgArayuzuSayisi; i++)
                {
                    if (!agArayuzuList.Where(x => x.Port == "Port " + i && x.KullanimAmaciId == obj.KullanimAmaciId).Any())
                    {
                        portList.Add("Port " + i);
                    }
                }
            }

            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            DuplicateArayuzPopupWindow popup = new DuplicateArayuzPopupWindow((int)TipEnum.UcBirim, obj, null, portList);
            popup.Owner = this;
            popup.ShowDialog();
        }

        public void UpdateAgArayuzuTable()
        {
            UcBirimAgArayuzDataGrid.ItemsSource = null;
            UcBirimAgArayuzDataGrid.ItemsSource = agArayuzuList; ;
        }

        private void ButtonAddGucArayuzu_Click(object sender, RoutedEventArgs e)
        {
            ValidateGucArayuzuFiledBorders();
            bool validMi = GirdiGucArayuzuValidation();

            if (validMi)
            {
                gucArayuzu.TipId = (int)TipEnum.UcBirimGucArayuzu;

                var validationContext = new ValidationContext(gucArayuzu, null, null);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
                {
                    gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                    gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(gt => gt.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

                    if (checkedGucArayuzuRow != null)
                    {
                        var ctx = checkedGucArayuzuRow.DataContext;
                        var obj = (GucArayuzu)ctx;
                        gucArayuzuList.Remove(obj);
                        checkedGucArayuzuRow = null;
                    }

                    if (!gucArayuzuList.Any(x => x.Port == gucArayuzu.Port))
                    {
                        BtnGucArayuzEkle.Text = "Ekle";
                        BtnGucArayuzEkle.Margin = new Thickness(100, 0, 0, 0);

                        UcBirimGucArayuzDataGrid.ItemsSource = null;

                        gucArayuzuList.Add(gucArayuzu);
                        UcBirimGucArayuzDataGrid.ItemsSource = gucArayuzuList;
                        UcBirimGucArayuzDataGrid.Visibility = Visibility.Visible;
                        GucArayuzuNoDataRow.Visibility = Visibility.Hidden;

                        GucArayuzuTab.DataContext = null;
                        gucArayuzu = new GucArayuzu();
                        ListGerilimTipi();
                        ListKullanimAmaciForGucArayuzu();
                        GucArayuzuTab.DataContext = gucArayuzu;
                    }
                    else
                    {
                        NotifyInfoPopup nfp = new NotifyInfoPopup();
                        nfp.msg.Text = gucArayuzu.Port + " için veri girilmiştir"; ;
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

                UcBirimGucArayuzDataGrid.ItemsSource = null;
                gucArayuzuList.Remove(obj);
                UcBirimGucArayuzDataGrid.ItemsSource = gucArayuzuList;

                if (gucArayuzuList.Count == 0)
                {
                    UcBirimGucArayuzDataGrid.Visibility = Visibility.Hidden;
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

            for (int i = 1; i <= ucBirim.GucArayuzuSayisi; i++)
            {
                if (!gucArayuzuList.Where(x => x.Port == "Port " + i && x.KullanimAmaciId == obj.KullanimAmaciId).Any())
                {
                    portList.Add("Port " + i);
                }
            }

            this.IsEnabled = false;
            System.Windows.Media.Effects.BlurEffect blur = new System.Windows.Media.Effects.BlurEffect();
            blur.Radius = 2;
            this.Effect = blur;

            DuplicateArayuzPopupWindow popup = new DuplicateArayuzPopupWindow((int)TipEnum.UcBirim, null, obj, portList);
            popup.Owner = this;
            popup.ShowDialog();
        }

        public void UpdateGucArayuzuTable()
        {
            UcBirimGucArayuzDataGrid.ItemsSource = null;
            UcBirimGucArayuzDataGrid.ItemsSource = gucArayuzuList;
        }
        #endregion

        #region OpenResponseModalEvent
        private void OpenResponseModal(ResponseModel response)
        {
            if (!response.HasError)
            {
                NotifySuccessPopup nfp = new NotifySuccessPopup();
                nfp.msg.Text = response.Message;
                nfp.Owner = Owner;
                nfp.Show();

                (Owner as MainWindow).ListUcBirim();
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
        #endregion

    }
    
}
