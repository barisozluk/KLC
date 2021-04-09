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
    /// Interaction logic for AgAnahtariPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariPopupWindow : Window
    {
        public bool isEditMode = false;

        private AYPContext context;

        private IAgAnahtariService service;
        private IKodListeService kodListeService;

        AgAnahtari agAnahtari;
        AgArayuzu agArayuzu;
        GucArayuzu gucArayuzu;

        List<AgArayuzu> agArayuzuList;
        private CheckBox checkedAgArayuzuRow = null;
        List<GucArayuzu> gucArayuzuList;
        private CheckBox checkedGucArayuzuRow = null;

        private NotificationManager notificationManager;

        public AgAnahtariPopupWindow(AgAnahtari _agAnahtari, bool fromNode)
        {
            if(_agAnahtari != null)
            {
                agAnahtari = _agAnahtari;
                isEditMode = true;
            }
            else
            {
                agAnahtari = new AgAnahtari();
                agArayuzu = new AgArayuzu();
                gucArayuzu = new GucArayuzu();

                agArayuzuList = new List<AgArayuzu>();
                gucArayuzuList = new List<GucArayuzu>();
                isEditMode = false;
            }

            this.notificationManager = new NotificationManager();
            this.context = new AYPContext();
            service = new AgAnahtariService(this.context);
            kodListeService = new KodListeService(this.context);
            InitializeComponent();
            SetAgAnahtariTurList();
            AgAnahtariTab.DataContext = agAnahtari;

            if (isEditMode)
            {
                DownloadButton.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadButton.Visibility = Visibility.Hidden;

                AgAnahtariAgArayuzDataGrid.Visibility = Visibility.Hidden;
                AgArayuzuNoDataRow.Visibility = Visibility.Visible;

                AgAnahtariGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
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

            if (agAnahtari.AgAnahtariTurList.Count() == 0)
            {
                Loaded += (s, e) => ClosePopup();
            }
        }

        private void Save_AgAnahtari(object sender, RoutedEventArgs e)
        {
            if (gucArayuzuList.Count == agAnahtari.GucArayuzuSayisi)
            {
                var response = new ResponseModel();

                if (!isEditMode)
                {
                    if ((agAnahtari.GirdiAgArayuzuSayisi != null && agAnahtari.GirdiAgArayuzuSayisi != 0) || (agAnahtari.CiktiAgArayuzuSayisi != null && agAnahtari.CiktiAgArayuzuSayisi != 0))
                    {
                        int totalCount = 0;
                        if (agAnahtari.GirdiAgArayuzuSayisi != null && agAnahtari.GirdiAgArayuzuSayisi != 0)
                        {
                            totalCount += agAnahtari.GirdiAgArayuzuSayisi.Value;
                        }
                        if (agAnahtari.CiktiAgArayuzuSayisi != null && agAnahtari.CiktiAgArayuzuSayisi != 0)
                        {
                            totalCount += agAnahtari.CiktiAgArayuzuSayisi.Value;
                        }

                        if (agArayuzuList.Count == totalCount)
                        {
                            response = service.SaveAgAnahtari(agAnahtari, agArayuzuList, gucArayuzuList);
                        }
                        else
                        {
                            NotifyWarningPopup nfp = new NotifyWarningPopup();
                            nfp.msg.Text = "Lütfen, bütün sayısını tanımladığınız ağ arayüzleri için veri girişi yapınız!";
                            nfp.Owner = Owner;
                            nfp.Show();
                        }
                    }
                    else
                    {
                        response = service.SaveAgAnahtari(agAnahtari, new List<AgArayuzu>(), gucArayuzuList);
                    }
                }
                else
                {
                    response = service.UpdateAgAnahtari(agAnahtari);
                }

                if (!response.HasError)
                {
                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = response.Message;
                    nfp.Owner = Owner;
                    nfp.Show();

                    (Owner as MainWindow).ListAgAnahtari();
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

        #region PopupCloseEvents
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
        #endregion

        #region TabTransitionEvents
        private void AgAnahtariNextButton_Click(object sender, RoutedEventArgs e)
        {
            agAnahtari.TipId = (int)TipEnum.AgAnahtari;

            var validationContext = new ValidationContext(agAnahtari, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agAnahtari, validationContext, results, true))
            {
                AgAnahtariTab.IsSelected = false;

                if ((agAnahtari.GirdiAgArayuzuSayisi != null && agAnahtari.GirdiAgArayuzuSayisi != 0) || (agAnahtari.CiktiAgArayuzuSayisi != null && agAnahtari.CiktiAgArayuzuSayisi != 0))
                {
                    AgArayuzuTab.IsSelected = true;

                    Width = 850;
                    Height = 580;

                    AgAnahtariTab.Width = 280;
                    AgArayuzuTab.Width = 281;
                    GucArayuzuTab.Width = 281;

                    AgArayuzuTab.DataContext = null;
                    ListKapasite();
                    ListFizikselOrtam();
                    ListKullanimAmaciForAgArayuzu();
                    AgArayuzuTab.DataContext = agArayuzu;
                }
                else
                {
                    GucArayuzuTab.IsSelected = true;

                    Width = 860;
                    Height = 995;
                    AgAnahtariTab.Width = 282;
                    AgArayuzuTab.Width = 285;
                    GucArayuzuTab.Width = 285;

                    GucArayuzuTab.DataContext = null;
                    ListGerilimTipi();
                    ListKullanimAmaciForGucArayuzu();
                    GucArayuzuTab.DataContext = gucArayuzu;
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

        private void AgArayuzuNextButton_Click(object sender, RoutedEventArgs e)
        {
            AgAnahtariTab.IsSelected = false;
            GucArayuzuTab.IsSelected = true;

            Width = 860;
            Height = 995;
            AgAnahtariTab.Width = 282;
            AgArayuzuTab.Width = 285;
            GucArayuzuTab.Width = 285;

            GucArayuzuTab.DataContext = null;
            ListKullanimAmaciForGucArayuzu();
            ListGerilimTipi();
            GucArayuzuTab.DataContext = gucArayuzu;
        }

        private void AgArayuzuPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            AgArayuzuTab.IsSelected = false;
            AgAnahtariTab.IsSelected = true;

            Width = 460;
            Height = 605;
            AgAnahtariTab.Width = 150;
            AgArayuzuTab.Width = 151;
            GucArayuzuTab.Width = 151;
        }

        private void GucArayuzuPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            GucArayuzuTab.IsSelected = false;

            if ((agAnahtari.GirdiAgArayuzuSayisi != null && agAnahtari.GirdiAgArayuzuSayisi != 0) || (agAnahtari.CiktiAgArayuzuSayisi != null && agAnahtari.CiktiAgArayuzuSayisi != 0))
            {
                AgArayuzuTab.IsSelected = true;

                AgArayuzuTab.DataContext = null;
                ListKapasite();
                ListFizikselOrtam();
                ListKullanimAmaciForAgArayuzu();
                AgArayuzuTab.DataContext = agArayuzu;

                Width = 850;
                Height = 580;
                AgAnahtariTab.Width = 280;
                AgArayuzuTab.Width = 281;
                GucArayuzuTab.Width = 281;
            }
            else
            {
                AgAnahtariTab.IsSelected = true;
                Width = 460;
                Height = 605;
                AgAnahtariTab.Width = 150;
                AgArayuzuTab.Width = 151;
                GucArayuzuTab.Width = 151;
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
                agAnahtari.KatalogDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Katalog.Text = agAnahtari.KatalogDosyaAdi;
                agAnahtari.Katalog = File.ReadAllBytes(openFileDialog.FileName);
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
                agAnahtari.SembolDosyaAdi = Path.GetFileName(openFileDialog.FileName);
                Sembol.Text = agAnahtari.SembolDosyaAdi;
                agAnahtari.Sembol = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
        #endregion

        #region GetListEvents
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
                NotifyInfoPopup nfp = new NotifyInfoPopup();
                nfp.msg.Text = "Lütfen, en az bir ağ anahtarı tanımlayınız.";
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
            gucArayuzu.KullanimAmaciList = kodListeService.ListKullanimAmaci();
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
                for (int i = 1; i <= agAnahtari.CiktiAgArayuzuSayisi; i++)
                {
                    agArayuzu.PortList.Add("Port " + i);
                }
            }
            else
            {
                for (int i = 1; i <= agAnahtari.GirdiAgArayuzuSayisi; i++)
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

            for (int i = 1; i <= agAnahtari.GucArayuzuSayisi; i++)
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

        #region NumberValidationEvents
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
                    string path = fbd.SelectedPath + "\\" + agAnahtari.KatalogDosyaAdi;

                    try
                    {
                        File.WriteAllBytes(path, agAnahtari.Katalog);
                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = "İşlem başarı ile gerçekleştirildi.";
                        nfp.Owner = Owner;
                        nfp.Show();
                    }
                    catch (Exception exception)
                    {
                        NotifyWarningPopup nfp = new NotifyWarningPopup();
                        nfp.msg.Text = "İşlem başarısız oldu.";
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

        #region TableEvents
        private void ButtonAddAgArayuzu_Click(object sender, RoutedEventArgs e)
        {
            agArayuzu.TipId = (int)TipEnum.AgAnahtariAgArayuzu;

            var validationContext = new ValidationContext(agArayuzu, null, null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(agArayuzu, validationContext, results, true))
            {
                if (checkedAgArayuzuRow != null)
                {
                    var ctx = checkedAgArayuzuRow.DataContext;
                    var obj = (AgArayuzu)ctx;
                    agArayuzuList.Remove(obj);
                    checkedAgArayuzuRow = null;
                }

                AgAnahtariAgArayuzDataGrid.ItemsSource = null;

                agArayuzu.KL_KullanimAmaci = agArayuzu.KullanimAmaciList.Where(kal => kal.Id == agArayuzu.KullanimAmaciId).FirstOrDefault();
                agArayuzu.KL_Kapasite = agArayuzu.KapasiteList.Where(kl => kl.Id == agArayuzu.KapasiteId).FirstOrDefault();
                agArayuzu.KL_FizikselOrtam = agArayuzu.FizikselOrtamList.Where(fo => fo.Id == agArayuzu.FizikselOrtamId).FirstOrDefault();

                agArayuzuList.Add(agArayuzu);
                AgAnahtariAgArayuzDataGrid.ItemsSource = agArayuzuList;
                AgAnahtariAgArayuzDataGrid.Visibility = Visibility.Visible;
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
            AgArayuzuTab.DataContext = null;
            agArayuzu = new AgArayuzu();
            ListKapasite();
            ListFizikselOrtam();
            ListKullanimAmaciForAgArayuzu();
            AgArayuzuTab.DataContext = agArayuzu;
        }

        private void AgArayuzuDelete_Row(object sender, RoutedEventArgs e)
        {
            var row = (Button)sender;
            var ctx = row.DataContext;            
            var obj = (AgArayuzu)ctx;

            AgAnahtariAgArayuzDataGrid.ItemsSource = null;
            agArayuzuList.Remove(obj);
            AgAnahtariAgArayuzDataGrid.ItemsSource = agArayuzuList;

            if (agArayuzuList.Count == 0)
            {
                AgAnahtariAgArayuzDataGrid.Visibility = Visibility.Hidden;
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

        private void ButtonAddGucArayuzu_Click(object sender, RoutedEventArgs e)
        {
            gucArayuzu.TipId = (int)TipEnum.AgAnahtariGucArayuzu;

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

                AgAnahtariGucArayuzDataGrid.ItemsSource = null;

                gucArayuzu.KL_KullanimAmaci = gucArayuzu.KullanimAmaciList.Where(kal => kal.Id == gucArayuzu.KullanimAmaciId).FirstOrDefault();
                gucArayuzu.KL_GerilimTipi = gucArayuzu.GerilimTipiList.Where(fo => fo.Id == gucArayuzu.GerilimTipiId).FirstOrDefault();

                gucArayuzuList.Add(gucArayuzu);
                AgAnahtariGucArayuzDataGrid.ItemsSource = gucArayuzuList;
                AgAnahtariGucArayuzDataGrid.Visibility = Visibility.Visible;
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

            AgAnahtariGucArayuzDataGrid.ItemsSource = null;
            gucArayuzuList.Remove(obj);
            AgAnahtariGucArayuzDataGrid.ItemsSource = gucArayuzuList;

            if (gucArayuzuList.Count == 0)
            {
                AgAnahtariGucArayuzDataGrid.Visibility = Visibility.Hidden;
                GucArayuzuNoDataRow.Visibility = Visibility.Visible;
            }
        }
        #endregion

    }
}
