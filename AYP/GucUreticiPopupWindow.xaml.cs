using AYP.DbContext.AYP.DbContexts;
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
using System.Text.RegularExpressions;
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

        private AYPContext context;

        private IGucUreticiService service;
        private IKodListeService kodListeService;

        GucUretici gucUretici;
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
                gucUretici = _gucUretici;
                isEditMode = true;
            }
            else
            {
                gucUretici = new GucUretici();
                isEditMode = false;
            }

            this.context = new AYPContext();
            service = new GucUreticiService(this.context);
            kodListeService = new KodListeService(this.context);
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

                DownloadButton.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(gucUretici.KatalogDosyaAdi))
                {
                    DownloadButton.IsEnabled = false;
                }
                else
                {
                    DownloadButton.IsEnabled = true;
                }

                if (fromNode)
                {
                    //GucUreticiTab
                    GirdiGucArayuzuSayisi.IsEnabled = false;
                    GirdiGucArayuzuSayisi.Opacity = 0.25;
                    CiktiGucArayuzuSayisi.IsEnabled = false;
                    CiktiGucArayuzuSayisi.Opacity = 0.25;
                    Sembol.IsEnabled = false;
                    Sembol.Opacity = 0.25;

                    //GucArayuzuTab
                    GucArayuzuEkleBtn.IsEnabled = false;
                    GucArayuzuEkleBtn.Opacity = 0.25;
                }
                else
                {
                    var mainWindow = Owner as MainWindow;
                    var nodes = mainWindow.ViewModel.NodesCanvas.Nodes.Items;

                    if (!nodes.Any(n => n.Id == gucUretici.Id))
                    {
                        fromNode = false;
                        //GucUreticiTab
                        GirdiGucArayuzuSayisi.IsEnabled = true;
                        GirdiGucArayuzuSayisi.Opacity = 1;
                        CiktiGucArayuzuSayisi.IsEnabled = true;
                        CiktiGucArayuzuSayisi.Opacity = 1;
                        Sembol.IsEnabled = true;
                        Sembol.Opacity = 1;

                        //GucArayuzuTab
                        GucArayuzuEkleBtn.IsEnabled = true;
                        GucArayuzuEkleBtn.Opacity = 1;
                    }
                    else
                    {
                        fromNode = true;
                        //GucUreticiTab
                        GirdiGucArayuzuSayisi.IsEnabled = false;
                        GirdiGucArayuzuSayisi.Opacity = 0.25;
                        CiktiGucArayuzuSayisi.IsEnabled = false;
                        CiktiGucArayuzuSayisi.Opacity = 0.25;
                        Sembol.IsEnabled = false;
                        Sembol.Opacity = 0.25;

                        //GucArayuzuTab
                        GucArayuzuEkleBtn.IsEnabled = false;
                        GucArayuzuEkleBtn.Opacity = 0.25;
                    }
                }
            }
            else
            {
                gucArayuzuList = new List<GucArayuzu>();

                DownloadButton.Visibility = Visibility.Hidden;

                GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
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

        #region TabTransitionEvents
        private void GucUreticiNextButton_Click(object sender, RoutedEventArgs e)
        {
            gucUretici.TipId = (int)TipEnum.GucUretici;

            var validationContext = new ValidationContext(gucUretici, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();


            if (Validator.TryValidateObject(gucUretici, validationContext, results, true))
            {
                if ((gucUretici.VerimlilikDegeri != 0 && gucUretici.VerimlilikDegeri != null) || (gucUretici.DahiliGucTuketimDegeri != 0 && gucUretici.DahiliGucTuketimDegeri != null))
                {
                    GucUreticiTab.IsSelected = false;
                    GucArayuzuTab.IsSelected = true;

                    WindowStartupLocation = WindowStartupLocation.Manual;
                    Top = 42;
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
                else
                {
                    if (gucUretici.VerimlilikDegeri == 0 || gucUretici.VerimlilikDegeri == null)
                    {
                        VerimlilikOrani.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    if (gucUretici.DahiliGucTuketimDegeri == 0 || gucUretici.DahiliGucTuketimDegeri == null)
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

                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, zorunlu alanları doldurunuz.";
                nfp.Owner = Owner;
                nfp.Show();
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

        }
        #endregion

        #region Save/UpdateEvent
        private void Save_GucUretici(object sender, RoutedEventArgs e)
        {
            if (gucArayuzuList.Count == gucUretici.CiktiGucArayuzuSayisi + gucUretici.GirdiGucArayuzuSayisi)
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    response = service.SaveGucUretici(gucUretici, gucArayuzuList);
                }
                else
                {
                    if (fromNode)
                    {
                        response = service.UpdateGucUretici(gucUretici, new List<GucArayuzu>());
                    }
                    else
                    {
                        response = service.UpdateGucUretici(gucUretici, gucArayuzuList);

                    }
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

        #region OpenKatalogFileDialogEvent
        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {
                gucUretici.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = gucUretici.KatalogDosyaAdi;
                gucUretici.Katalog = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        #endregion

        #region OpenSembolFileDialogEvent
        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\SembolKutuphanesi";
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
            gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
            if (gucArayuzu.KullanimAmaciList.Count() > 0)
            {
                gucArayuzu.KullanimAmaciId = gucArayuzu.KullanimAmaciList[0].Id;
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

            if (!isEditMode && string.IsNullOrEmpty(gucArayuzu.Port))
            {
                if (gucArayuzu.PortList.Count > 0)
                {
                    gucArayuzu.Port = gucArayuzu.PortList[0];
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
            if (!fromNode)
            {
                if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                {
                    ag2.IsEnabled = false; ag2.Opacity = 0.25;
                    ag4.IsEnabled = false; ag4.Opacity = 0.25;
                    ag6.IsEnabled = false; ag6.Opacity = 0.25;
                    ag8.IsEnabled = false; ag8.Opacity = 0.25;
                    ag10.IsEnabled = false; ag10.Opacity = 0.25;
                    ag12.IsEnabled = false; ag12.Opacity = 0.25;
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
                    ag16.IsEnabled = false; ag16.Opacity = 0.25;
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
            decimal value;

            if (!fromNode)
            {
                if (!string.IsNullOrEmpty(VerimlilikOrani.Text) && VerimlilikOrani.Text != " ")
                {
                    DahiliGucTuketimDegeri.IsEnabled = false;
                    DahiliGucTuketimDegeri.Opacity = 0.25;
                    var verimlilik = VerimlilikOrani.Text.Replace(".", ",");
                    value = decimal.Parse(verimlilik);
                    if (value > 100)
                    {
                        VerimlilikOrani.Text = "100";
                    }
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
            
                bool validMi = MaxMinGerilimValidation(sender, e);
                if (validMi)
                {
                    gucArayuzu.TipId = (int)TipEnum.GucUreticiGucArayuzu;

                    var validationContext = new ValidationContext(gucArayuzu, null, null);
                    var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                    if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
                    {
                        gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                        gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(fo => fo.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

                        if (!gucArayuzuList.Any(x => x.Port == gucArayuzu.Port && x.KullanimAmaciId == gucArayuzu.KullanimAmaciId))
                        {
                            if (checkedGucArayuzuRow != null)
                            {
                                var ctx = checkedGucArayuzuRow.DataContext;
                                var obj = (GucArayuzu)ctx;
                                gucArayuzuList.Remove(obj);
                                checkedGucArayuzuRow = null;
                            }

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

        private bool MaxMinGerilimValidation(object sender, RoutedEventArgs e)
        {
            bool validMi = true;

            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Girdi)
            {
                int maxGerilimDegeri;
                int minGerilimDegeri;
                ag8.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag10.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ag2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);

                if (!string.IsNullOrEmpty(ag2.Text) && ag2.Text != " ")
                {
                    if (!string.IsNullOrEmpty(ag8.Text) && ag8.Text != " ")
                    {
                        if (!string.IsNullOrEmpty(ag10.Text) && ag10.Text != " ")
                        {
                            maxGerilimDegeri = int.Parse(ag10.Text);
                            minGerilimDegeri = int.Parse(ag8.Text);
                            if (minGerilimDegeri > maxGerilimDegeri)
                            {
                                NotifyInfoPopup nfp = new NotifyInfoPopup();
                                nfp.msg.Text = "Minimum gerilim değeri maksimum gerilim değerinden büyük olamaz.";
                                nfp.Owner = Owner;
                                nfp.Show();
                                ag8.BorderBrush = new SolidColorBrush(Colors.Red);
                                ag10.BorderBrush = new SolidColorBrush(Colors.Red);
                                return false;
                            }
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Maksimum gerilim değerini tanımlayınız.";
                            nfp.Owner = Owner;
                            nfp.Show();
                            ag10.BorderBrush = new SolidColorBrush(Colors.Red);
                            return false;
                        }
                    }
                    if (!string.IsNullOrEmpty(ag10.Text) && ag10.Text != " ")
                    {
                        if (!string.IsNullOrEmpty(ag8.Text) && ag8.Text != " ")
                        {
                            maxGerilimDegeri = int.Parse(ag10.Text);
                            minGerilimDegeri = int.Parse(ag8.Text);
                            if (minGerilimDegeri > maxGerilimDegeri)
                            {
                                NotifyInfoPopup nfp = new NotifyInfoPopup();
                                nfp.msg.Text = "Minimum gerilim değeri maksimum gerilim değerinden büyük olamaz.";
                                nfp.Owner = Owner;
                                nfp.Show();
                                ag8.BorderBrush = new SolidColorBrush(Colors.Red);
                                ag10.BorderBrush = new SolidColorBrush(Colors.Red);
                                return false;

                            }
                        }
                        else
                        {
                            NotifyInfoPopup nfp = new NotifyInfoPopup();
                            nfp.msg.Text = "Minimum gerilim değerini tanımlayınız.";
                            nfp.Owner = Owner;
                            nfp.Show();
                            ag8.BorderBrush = new SolidColorBrush(Colors.Red);
                            return false;
                        }
                    }
                }
                else
                {
                    ag2.BorderBrush = new SolidColorBrush(Colors.Red);

                    validMi = false;
                }
            }
            else
            {
                validMi = true;
            }

            return validMi;
        }
        private void GucArayuzuRow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkedGucArayuzuRow != null)
            {
                checkedGucArayuzuRow.IsChecked = false;
            }

            checkedGucArayuzuRow = (CheckBox)sender;
            var ctx = checkedGucArayuzuRow.DataContext;
            gucArayuzu = (GucArayuzu)ctx;

            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void GucArayuzuRow_Unchecked(object sender, RoutedEventArgs e)
        {
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
    }
}
