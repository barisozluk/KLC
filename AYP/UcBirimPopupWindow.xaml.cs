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
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
        private AYPContext context;

        private IUcBirimService service;
        private IKodListeService kodListeService;

        UcBirim ucBirim;
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
                ucBirim = _ucBirim;
                isEditMode = true;
            }
            else
            {
                ucBirim = new UcBirim();
                isEditMode = false;
            }

            this.context = new AYPContext();
            service = new UcBirimService(this.context);
            kodListeService = new KodListeService(this.context);
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

                DownloadButton.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(ucBirim.KatalogDosyaAdi))
                {
                    DownloadButton.IsEnabled = false;
                }
                else
                {
                    DownloadButton.IsEnabled = true;
                }

                if (fromNode)
                {
                    //UcBirimTab
                    GirdiAgArayuzuSayisi.IsEnabled = false;
                    GirdiAgArayuzuSayisi.Opacity = 0.25;
                    CiktiAgArayuzuSayisi.IsEnabled = false;
                    CiktiAgArayuzuSayisi.Opacity = 0.25;
                    GucArayuzuSayisi.IsEnabled = false;
                    GucArayuzuSayisi.Opacity = 0.25;
                    Sembol.IsEnabled = false;
                    Sembol.Opacity = 0.25;

                    //AgArayuzuTab
                    AgArayuzuEkleBtn.IsEnabled = false;
                    AgArayuzuEkleBtn.Opacity = 0.25;

                    //GucArayuzuTab
                    GucArayuzuEkleBtn.IsEnabled = false;
                    GucArayuzuEkleBtn.Opacity = 0.25;
                }
                else
                {
                    var mainWindow = Owner as MainWindow;
                    var nodes = mainWindow.ViewModel.NodesCanvas.Nodes.Items;

                    if (!nodes.Any(n => n.Id == ucBirim.Id))
                    {
                        fromNode = false;
                        //UcBirimTab
                        GirdiAgArayuzuSayisi.IsEnabled = true;
                        GirdiAgArayuzuSayisi.Opacity = 1;
                        CiktiAgArayuzuSayisi.IsEnabled = true;
                        CiktiAgArayuzuSayisi.Opacity = 1;
                        GucArayuzuSayisi.IsEnabled = true;
                        GucArayuzuSayisi.Opacity = 1;
                        Sembol.IsEnabled = true;
                        Sembol.Opacity = 1;

                        //AgArayuzuTab
                        AgArayuzuEkleBtn.IsEnabled = true;
                        AgArayuzuEkleBtn.Opacity = 1;

                        //GucArayuzuTab
                        GucArayuzuEkleBtn.IsEnabled = true;
                        GucArayuzuEkleBtn.Opacity = 1;
                    }
                    else
                    {
                        fromNode = true;
                        //UcBirimTab
                        GirdiAgArayuzuSayisi.IsEnabled = false;
                        GirdiAgArayuzuSayisi.Opacity = 0.25;
                        CiktiAgArayuzuSayisi.IsEnabled = false;
                        CiktiAgArayuzuSayisi.Opacity = 0.25;
                        GucArayuzuSayisi.IsEnabled = false;
                        GucArayuzuSayisi.Opacity = 0.25;
                        Sembol.IsEnabled = false;
                        Sembol.Opacity = 0.25;

                        //AgArayuzuTab
                        AgArayuzuEkleBtn.IsEnabled = false;
                        AgArayuzuEkleBtn.Opacity = 0.25;

                        //GucArayuzuTab
                        GucArayuzuEkleBtn.IsEnabled = false;
                        GucArayuzuEkleBtn.Opacity = 0.25;
                    }
                }
            }
            else
            {
                agArayuzuList = new List<AgArayuzu>();
                gucArayuzuList = new List<GucArayuzu>();

                DownloadButton.Visibility = Visibility.Hidden;

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
                    if (fromNode)
                    {
                        response = service.UpdateUcBirim(ucBirim, new List<AgArayuzu>(), new List<GucArayuzu>());
                        OpenResponseModal(response);
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

        #region TabTransitionEvents
        private void UcBirimNextButton_Click(object sender, RoutedEventArgs e)
        {
            ucBirim.TipId = (int)TipEnum.UcBirim;

            var validationContext = new ValidationContext(ucBirim, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(ucBirim, validationContext, results, true))
            {
                if ((ucBirim.GirdiAgArayuzuSayisi.HasValue && ucBirim.GirdiAgArayuzuSayisi.Value > 0) || (ucBirim.CiktiAgArayuzuSayisi.HasValue && ucBirim.CiktiAgArayuzuSayisi.Value > 0))
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
            Top = 42;
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
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\SembolKutuphanesi";
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

            if (!isEditMode && string.IsNullOrEmpty(agArayuzu.Port))
            {
                if (agArayuzu.PortList.Count > 0)
                {
                    agArayuzu.Port = agArayuzu.PortList[0];
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

        #region TableEvents
        private void ButtonAddAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            agArayuzu.TipId = (int)TipEnum.UcBirimAgArayuzu;

            var validationContext = new ValidationContext(agArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agArayuzu, validationContext, results, true))
            {
                agArayuzu.KL_KullanimAmaci = agArayuzu.KullanimAmaciList.Where(kal => kal.Id == agArayuzu.KullanimAmaciId).FirstOrDefault();
                agArayuzu.KL_Kapasite = agArayuzu.KapasiteList.Where(kl => kl.Id == agArayuzu.KapasiteId).FirstOrDefault();
                agArayuzu.KL_FizikselOrtam = agArayuzu.FizikselOrtamList.Where(fo => fo.Id == agArayuzu.FizikselOrtamId).FirstOrDefault();

                if (!agArayuzuList.Any(x => x.Port == agArayuzu.Port && x.KullanimAmaciId == agArayuzu.KullanimAmaciId))
                {
                    if (checkedAgArayuzuRow != null)
                    {
                        var ctx = checkedAgArayuzuRow.DataContext;
                        var obj = (AgArayuzu)ctx;
                        agArayuzuList.Remove(obj);
                        checkedAgArayuzuRow = null;
                    }

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
        private void AgArayuzuRow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkedAgArayuzuRow != null)
            {
                checkedAgArayuzuRow.IsChecked = false;
            }

            checkedAgArayuzuRow = (CheckBox)sender;
            var ctx = checkedAgArayuzuRow.DataContext;
            agArayuzu = (AgArayuzu)ctx;

            AgArayuzuTab.DataContext = agArayuzu;
        }

        private void AgArayuzuRow_Unchecked(object sender, RoutedEventArgs e)
        {
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
            bool validMi = MaxMinGerilimValidation(sender, e);
            if (validMi)
            {
                gucArayuzu.TipId = (int)TipEnum.UcBirimGucArayuzu;

                var validationContext = new ValidationContext(gucArayuzu, null, null);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
                {
                    gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                    gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(gt => gt.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

                    if (!gucArayuzuList.Any(x => x.Port == gucArayuzu.Port))
                    {
                        if (checkedGucArayuzuRow != null)
                        {
                            var ctx = checkedGucArayuzuRow.DataContext;
                            var obj = (GucArayuzu)ctx;
                            gucArayuzuList.Remove(obj);
                            checkedGucArayuzuRow = null;
                        }

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

        private bool MaxMinGerilimValidation(object sender, RoutedEventArgs e)
        {
            int maxGerilimDegeri;
            int minGerilimDegeri;
            ag8.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag10.BorderBrush = new SolidColorBrush(Colors.Transparent);
            ag2.BorderBrush = new SolidColorBrush(Colors.Transparent);
            GucArayuzuAdi.BorderBrush = new SolidColorBrush(Colors.Transparent);

            bool validMi = true;
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

                UcBirimGucArayuzDataGrid.ItemsSource = null;
                gucArayuzuList.Remove(obj);
                UcBirimGucArayuzDataGrid.ItemsSource = gucArayuzuList;

                if (gucArayuzuList.Count == 0)
                {
                    UcBirimGucArayuzDataGrid.Visibility = Visibility.Hidden;
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
