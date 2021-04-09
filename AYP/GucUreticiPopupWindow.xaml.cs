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
        public bool isEditMode = false;

        private AYPContext context;

        private IGucUreticiService service;
        private IKodListeService kodListeService;

        private NotificationManager notificationManager;

        GucUretici gucUretici;
        GucArayuzu gucArayuzu;

        List<GucArayuzu> gucArayuzuList;
        private CheckBox checkedGucArayuzuRow = null;
        public GucUreticiPopupWindow(GucUretici _gucUretici, bool fromNode)
        {
            if(_gucUretici != null)
            {
                gucUretici = _gucUretici;
                isEditMode = true;
            }
            else
            {
                gucUretici = new GucUretici();
                gucArayuzu = new GucArayuzu();

                gucArayuzuList = new List<GucArayuzu>();
                isEditMode = false;
            }

            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new GucUreticiService(this.context);
            kodListeService = new KodListeService(this.context);
            InitializeComponent();
            SetGucUreticiTurList();

            DataContext = gucUretici;

            if (isEditMode)
            {
                DownloadButton.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadButton.Visibility = Visibility.Hidden;

                GucUreticiGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }

            if(fromNode)
            {
                GirdiGucArayuzuSayisi.IsEnabled = false;
                GirdiGucArayuzuSayisi.Opacity = 0.25;
                CiktiGucArayuzuSayisi.IsEnabled = false;
                CiktiGucArayuzuSayisi.Opacity = 0.25;
            }
            else
            {
                GirdiGucArayuzuSayisi.IsEnabled = true;
                GirdiGucArayuzuSayisi.Opacity = 1;
                CiktiGucArayuzuSayisi.IsEnabled = true;
                CiktiGucArayuzuSayisi.Opacity = 1;
            }

            if (gucUretici.GucUreticiTurList.Count() == 0)
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

        private void ButtonGucUreticiPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        #region TabTransitionEvents
        private void GucUreticiNextButton_Click(object sender, RoutedEventArgs e)
        {
            gucUretici.TipId = (int)TipEnum.GucUretici;

            var validationContext = new ValidationContext(gucUretici, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucUretici, validationContext, results, true))
            {
                GucUreticiTab.IsSelected = false;
                GucArayuzuTab.IsSelected = true;

                Width = 860;
                Height = 995;
                GucUreticiTab.Width = 426;
                GucArayuzuTab.Width = 426;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;

                GucArayuzuTab.DataContext = null;
                ListGerilimTipi();
                ListKullanimAmaciForGucArayuzu();
                GucArayuzuTab.DataContext = gucArayuzu;
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

if (memberName == "VerimlilikDegeri")
{
    VerimlilikOrani.BorderBrush = new SolidColorBrush(Colors.Red);
}

if (memberName == "DahiliGucTuketimDegeri")
{
    DahiliGucTuketimDegeri.BorderBrush = new SolidColorBrush(Colors.Red);
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
            GucArayuzuTab.IsSelected = false;
            GucUreticiTab.IsSelected = true;
            Width = 480;
            Height = 655;
            GucUreticiTab.Width = 236;
            GucArayuzuTab.Width = 236;
            
        }
        #endregion

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
                    response = service.UpdateGucUretici(gucUretici);
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
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Lütfen, bütün güç arayüzleri için veri girişi yapınız!";
                nfp.Owner = Owner;
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
            if (gucArayuzu.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
            {
                ag1.IsEnabled = false; ag1.Opacity = 0.25;
                ag2.IsEnabled = false; ag2.Opacity = 0.25;
                ag3.IsEnabled = false; ag3.Opacity = 0.25;
                ag4.IsEnabled = false; ag4.Opacity = 0.25;
                ag5.IsEnabled = false; ag5.Opacity = 0.25;
                ag6.IsEnabled = false; ag6.Opacity = 0.25;
                ag7.IsEnabled = false; ag7.Opacity = 0.25;
                ag8.IsEnabled = false; ag8.Opacity = 0.25;
                ag9.IsEnabled = false; ag9.Opacity = 0.25;
                ag10.IsEnabled = false; ag10.Opacity = 0.25;
                ag11.IsEnabled = false; ag11.Opacity = 0.25;
                ag12.IsEnabled = false; ag12.Opacity = 0.25;

                ag13.IsEnabled = true; ag13.Opacity = 1;
                ag14.IsEnabled = true; ag14.Opacity = 1;
                ag15.IsEnabled = true; ag15.Opacity = 1;
                ag16.IsEnabled = true; ag16.Opacity = 1;
            }
            else
            {
                ag1.IsEnabled = true; ag1.Opacity = 1;
                ag2.IsEnabled = true; ag2.Opacity = 1;
                ag3.IsEnabled = true; ag3.Opacity = 1;
                ag4.IsEnabled = true; ag4.Opacity = 1;
                ag5.IsEnabled = true; ag5.Opacity = 1;
                ag6.IsEnabled = true; ag6.Opacity = 1;
                ag7.IsEnabled = true; ag7.Opacity = 1;
                ag8.IsEnabled = true; ag8.Opacity = 1;
                ag9.IsEnabled = true; ag9.Opacity = 1;
                ag10.IsEnabled = true; ag10.Opacity = 1;
                ag11.IsEnabled = true; ag11.Opacity = 1;
                ag12.IsEnabled = true; ag12.Opacity = 1;

                ag13.IsEnabled = false; ag13.Opacity = 0.25;
                ag14.IsEnabled = false; ag14.Opacity = 0.25;
                ag15.IsEnabled = false; ag15.Opacity = 0.25;
                ag16.IsEnabled = false; ag16.Opacity = 0.25;
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
            if (!string.IsNullOrEmpty(VerimlilikOrani.Text) && VerimlilikOrani.Text != " ")
            {
                DahiliGucTuketimDegeri.IsEnabled = false;
                DahiliGucTuketimDegeri.Opacity = 0.25;
            }
            else
            {
                DahiliGucTuketimDegeri.IsEnabled = true;
                DahiliGucTuketimDegeri.Opacity = 1;
            }
        }

        private void DahiliGucTuketimDegeri_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(DahiliGucTuketimDegeri.Text) && DahiliGucTuketimDegeri.Text != " ")
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
        #endregion

        #region TableEvents
        private void ButtonAddGucArayuzu_Click(object sender, RoutedEventArgs e)
        {
            gucArayuzu.TipId = (int)TipEnum.GucUreticiGucArayuzu;

            var validationContext = new ValidationContext(gucArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(gucArayuzu, validationContext, results, true))
            {
                if (checkedGucArayuzuRow != null)
                {
                    var ctx = checkedGucArayuzuRow.DataContext;
                    var obj = (GucArayuzu)ctx;
                    gucArayuzuList.Remove(obj);
                    checkedGucArayuzuRow = null;
                }

                GucUreticiGucArayuzDataGrid.ItemsSource = null;

                gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(fo => fo.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

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
            GucArayuzuTab.DataContext = null;
            gucArayuzu = new GucArayuzu();
            ListGerilimTipi();
            ListKullanimAmaciForGucArayuzu();
            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void GucArayuzuDelete_Row(object sender, RoutedEventArgs e)
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
        #endregion
    }
}
