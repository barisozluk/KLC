using AYP.Enums;
using AYP.Models;
using AYP.ViewModel;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace AYP
{
    /// <summary>
    /// Interaction logic for RaporBilgileriPopupWindow.xaml
    /// </summary>
    public partial class RaporBilgileriPopupWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RaporModel model;
        byte[] capture;
        string raporTipi;
        public RaporBilgileriPopupWindow(string raporTipi, byte[] capture)
        {
            this.capture = capture;
            this.raporTipi = raporTipi;

            InitializeComponent();
            model = new RaporModel();
            DataContext = model;
        }

        #region PopupCloseEvents
        private void ClosePopup()
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ButtonRaporBilgileriPopupClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        #endregion

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
           
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            
        }

        private void UygulaButton_Click(object sender, RoutedEventArgs e)
        {
            int val1;
            bool parsed = Int32.TryParse(Yil1.Text, out val1);
            int val2;
            bool parsed2 = Int32.TryParse(Ay1.Text, out val2);
            int val3;
            bool parsed3 = Int32.TryParse(Gun1.Text, out val3);
            bool validmi = true;
            bool validmi2 = true;
            if (parsed && parsed2 && parsed3)
            {
                if (!string.IsNullOrEmpty(Yil1.Text) && !string.IsNullOrWhiteSpace(Yil1.Text) && Convert.ToInt32(Yil1.Text) <= DateTime.Now.Year + 20 && Convert.ToInt32(Yil1.Text) >= 2020 && !string.IsNullOrWhiteSpace(Yil1.Text))
                {
                    Gun1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Ay1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Convert.ToInt32(Yil1.Text);
                    if (!string.IsNullOrEmpty(Ay1.Text) && !string.IsNullOrWhiteSpace(Ay1.Text) && Convert.ToInt32(Ay1.Text) <= 12 && Convert.ToInt32(Ay1.Text) > 0)
                    {
                        Gun1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Ay1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Convert.ToInt32(Ay1.Text);
                        if (Convert.ToInt32(Ay1.Text) == 1 || Convert.ToInt32(Ay1.Text) == 3 || Convert.ToInt32(Ay1.Text) == 5 || Convert.ToInt32(Ay1.Text) == 7 || Convert.ToInt32(Ay1.Text) == 8 || Convert.ToInt32(Ay1.Text) == 10 || Convert.ToInt32(Ay1.Text) == 12)
                        {
                            if (!string.IsNullOrEmpty(Gun1.Text) && !string.IsNullOrWhiteSpace(Gun1.Text) && Convert.ToInt32(Gun1.Text) <= 31 && Convert.ToInt32(Gun1.Text) > 0)
                            {
                                Convert.ToInt32(Gun1.Text);
                                model.Tarih = new DateTime(Convert.ToInt32(Yil1.Text), Convert.ToInt32(Ay1.Text), Convert.ToInt32(Gun1.Text));
                            }
                            else
                            {
                                Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                                validmi = false;
                            }
                        }
                        else if (Convert.ToInt32(Ay1.Text) == 2)
                        {
                            if (!string.IsNullOrEmpty(Gun1.Text) && !string.IsNullOrWhiteSpace(Gun1.Text) && Convert.ToInt32(Gun1.Text) <= 29 && Convert.ToInt32(Gun1.Text) > 0)
                            {
                                Convert.ToInt32(Gun1.Text);
                                model.Tarih = new DateTime(Convert.ToInt32(Yil1.Text), Convert.ToInt32(Ay1.Text), Convert.ToInt32(Gun1.Text));
                            }
                            else
                            {
                                Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                                validmi = false;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Gun1.Text) && !string.IsNullOrWhiteSpace(Gun1.Text) && Convert.ToInt32(Gun1.Text) <= 30 && Convert.ToInt32(Gun1.Text) > 0)
                            {
                                Convert.ToInt32(Gun1.Text);
                                model.Tarih = new DateTime(Convert.ToInt32(Yil1.Text), Convert.ToInt32(Ay1.Text), Convert.ToInt32(Gun1.Text));
                            }
                            else
                            {
                                Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                                validmi = false;
                            }
                        }

                    }
                    else
                    {
                        Gun1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                        Yil1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        validmi = false;
                    }
                }
                else
                {
                    Gun1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Ay1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Yil1.BorderBrush = new SolidColorBrush(Colors.Red);
                    validmi = false;
                }
            }
            else
            {
                Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                Yil1.BorderBrush = new SolidColorBrush(Colors.Red);
                validmi = false;
            }

            if(string.IsNullOrEmpty(Yil2.Text) && string.IsNullOrWhiteSpace(Yil2.Text) && string.IsNullOrEmpty(Ay2.Text) && string.IsNullOrWhiteSpace(Ay2.Text) && string.IsNullOrEmpty(Gun2.Text) && string.IsNullOrWhiteSpace(Gun2.Text))
            {
                validmi2 = true;
                Gun2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                Ay2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                
            }
            else
            {
                if (!string.IsNullOrEmpty(Yil2.Text) && !string.IsNullOrWhiteSpace(Yil2.Text) && Convert.ToInt32(Yil2.Text) <= DateTime.Now.Year + 20 && Convert.ToInt32(Yil2.Text) >= 2020)
                {
                    validmi2 = false;
                    Convert.ToInt32(Yil2.Text);
                    Gun2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Ay2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    if (!string.IsNullOrEmpty(Ay2.Text) && !string.IsNullOrWhiteSpace(Ay2.Text) && Convert.ToInt32(Ay2.Text) <= 12 && Convert.ToInt32(Ay2.Text) > 0)
                    {
                        Convert.ToInt32(Ay2.Text);
                        Gun2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Ay2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        if (Convert.ToInt32(Ay2.Text) == 1 || Convert.ToInt32(Ay2.Text) == 3 || Convert.ToInt32(Ay2.Text) == 5 || Convert.ToInt32(Ay2.Text) == 7 || Convert.ToInt32(Ay2.Text) == 8 || Convert.ToInt32(Ay2.Text) == 10 || Convert.ToInt32(Ay2.Text) == 12)
                        {
                            if (!string.IsNullOrEmpty(Gun2.Text) && !string.IsNullOrWhiteSpace(Gun2.Text) && Convert.ToInt32(Gun2.Text) <= 31 && Convert.ToInt32(Gun2.Text) > 0)
                            {
                                Convert.ToInt32(Gun2.Text);
                                model.DegistirmeTarihi = new DateTime(Convert.ToInt32(Yil2.Text), Convert.ToInt32(Ay2.Text), Convert.ToInt32(Gun2.Text));
                                validmi2 = true;
                            }
                            else
                            {
                                Gun2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                            }

                        }
                        else if (Convert.ToInt32(Ay2.Text) == 2)
                        {
                            if (!string.IsNullOrEmpty(Gun2.Text) && !string.IsNullOrWhiteSpace(Gun2.Text) && Convert.ToInt32(Gun2.Text) <= 29 && Convert.ToInt32(Gun2.Text) > 0)
                            {
                                Convert.ToInt32(Gun2.Text);
                                model.DegistirmeTarihi = new DateTime(Convert.ToInt32(Yil2.Text), Convert.ToInt32(Ay2.Text), Convert.ToInt32(Gun2.Text));
                                validmi2 = true;
                            }
                            else
                            {
                                Gun2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Gun2.Text) && !string.IsNullOrWhiteSpace(Gun2.Text) && Convert.ToInt32(Gun2.Text) <= 30 && Convert.ToInt32(Gun2.Text) > 0)
                            {
                                Convert.ToInt32(Gun2.Text);
                                model.DegistirmeTarihi = new DateTime(Convert.ToInt32(Yil2.Text), Convert.ToInt32(Ay2.Text), Convert.ToInt32(Gun2.Text));
                                validmi2 = true;
                            }
                            else
                            {
                                Gun2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Ay2.BorderBrush = new SolidColorBrush(Colors.Red);
                                Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                            }
                        }
                    }
                    else
                    {
                        Gun2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        Ay2.BorderBrush = new SolidColorBrush(Colors.Red);
                        Yil2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    }
                }
                else
                {
                    Gun2.BorderBrush = new SolidColorBrush(Colors.Red);
                    Ay2.BorderBrush = new SolidColorBrush(Colors.Red);
                    Yil2.BorderBrush = new SolidColorBrush(Colors.Red);
                    validmi2 = false;
                }
            }

            
            if (validmi && validmi2)
            {
                var validationContext = new ValidationContext(model, null, null);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                if (Validator.TryValidateObject(model, validationContext, results, true))
                {
                    if (model.Tarih.HasValue)
                    {
                        if (raporTipi == "Ağ Planlama Raporu")
                        {
                            AgPlanlamaRaporuOlustur();
                        }
                        else
                        {
                            GucPlanlamaRaporuOlustur();
                        }
                    }
                    else
                    {
                        //Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                        //Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                        //Yil1.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                }
                else
                {
                    if (!model.Tarih.HasValue)
                    {
                        //Gun1.BorderBrush = new SolidColorBrush(Colors.Red);
                        //Ay1.BorderBrush = new SolidColorBrush(Colors.Red);
                        //Yil1.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    foreach (var result in results)
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            if (memberName == "GizlilikDerecesi")
                            {
                                GizlilikDerecesi.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "YazimOrtami")
                            {
                                YazimOrtami.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "Hazirlayan")
                            {
                                Hazirlayan.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "KontrolEden")
                            {
                                KontrolEden.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "Onaylayan")
                            {
                                Onaylayan.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "DilKodu")
                            {
                                DilKodu.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "DokumanTanimi")
                            {
                                DokumanTanimi.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "Bolum")
                            {
                                Bolum.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "RevizyonKodu")
                            {
                                RevizyonKodu.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "DokumanKodu")
                            {
                                DokumanKodu.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "DokumanParcaNo")
                            {
                                DokumanParcaNo.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "Degistiren")
                            {
                                Degistiren.BorderBrush = new SolidColorBrush(Colors.Red);
                            }

                            if (memberName == "SayfaBoyutu")
                            {
                                SayfaBoyutu.BorderBrush = new SolidColorBrush(Colors.Red);
                            }
                        }

                    }
                }
            }
        }


        private void AgPlanlamaRaporuOlustur()
        {
            var NodesCanvas = (Owner as MainWindow).ViewModel.NodesCanvas;

            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;

                    var img = iText.IO.Image.ImageDataFactory.Create(capture);
                    Image pdfImg = new Image(img);

                    var ucBirimler = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim).ToList();
                    var agAnahtarlari = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.AgAnahtari).ToList();
                    var grouplar = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.Group).ToList();
                    var internalConnectList = new List<ConnectViewModel>();
                    var externalConnectList = new List<ConnectViewModel>();

                    foreach (var group in grouplar)
                    {
                        var nodeList = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.NodeList).FirstOrDefault();
                        var temp = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.InternalConnectList).FirstOrDefault();
                        var temp2 = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.ExternalConnectList).FirstOrDefault();

                        if (temp != null)
                        {
                            foreach (var item in temp)
                            {
                                internalConnectList.Add(item);
                            }
                        }

                        if (temp2 != null)
                        {
                            foreach (var item in temp2)
                            {
                                externalConnectList.Add(item);
                            }
                        }

                        if (nodeList != null)
                        {
                            foreach (var node in nodeList)
                            {
                                if (node.TypeId == (int)TipEnum.UcBirim)
                                {
                                    ucBirimler.Add(node);
                                }
                                else if (node.TypeId == (int)TipEnum.AgAnahtari)
                                {
                                    agAnahtarlari.Add(node);
                                }
                            }
                        }
                    }

                    var kenarAgAnahtarlari = agAnahtarlari.Where(x => x.TurAd == "Kenar").ToList();
                    var toplamaAgAnahtarlari = agAnahtarlari.Where(x => x.TurAd == "Toplama").ToList();
                    var omurgaAgAnahtarlari = agAnahtarlari.Where(x => x.TurAd == "Omurga").ToList();

                    int pageNumber = 1;
                    var outputStream = new FileStream(path + "\\Ağ Planlama Raporu.pdf", FileMode.Create);
                    PdfDocument pdf = new PdfDocument(new PdfWriter(outputStream));
                    Document doc = new Document(pdf, PageSize.A4);
                    doc.SetFontProvider(new DefaultFontProvider(true, true, true));

                    float totalHeight = doc.GetPdfDocument().GetDefaultPageSize().GetHeight();
                    float headerHeight = SetRaporHeader(doc);
                    float footerHeight = SetRaporFooter(doc, pageNumber);

                    Paragraph header = new Paragraph("SEMA - AYP AĞ PLANLAMA RAPORU");
                    header.SetFontFamily(new string[] { "Arial", "serif" });
                    header.SetFontSize(16);
                    header.SetBold();
                    header.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    header.SetMarginTop(275);
                    doc.Add(header);
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                    float emptyHeight = totalHeight - (footerHeight + headerHeight);

                    pageNumber = SetUcBirimTable(doc, ucBirimler, pageNumber, emptyHeight);
                    if (ucBirimler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    pageNumber = SetAgAnahtariTable(doc, agAnahtarlari, pageNumber, emptyHeight);
                    if (agAnahtarlari.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    List<ConnectViewModel> connectList = new List<ConnectViewModel>();
                    foreach (var connect in NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in internalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in externalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (ucBirimler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    pageNumber = SetKenarToUcBirimTable(doc, connectList, pageNumber, emptyHeight);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    connectList = new List<ConnectViewModel>();
                    foreach (var connect in NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (toplamaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (toplamaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in internalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (toplamaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (toplamaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in externalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (toplamaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (toplamaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    pageNumber = SetToplamaToKenarTable(doc, connectList, pageNumber, emptyHeight);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    connectList = new List<ConnectViewModel>();
                    foreach (var connect in NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (omurgaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (omurgaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in internalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (omurgaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (omurgaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in externalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (omurgaAgAnahtarlari.Contains(connect.ToConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.FromConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                        if (omurgaAgAnahtarlari.Contains(connect.FromConnector.Node))
                        {
                            if (kenarAgAnahtarlari.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    pageNumber = SetOmurgaToKenarTable(doc, connectList, pageNumber, emptyHeight, pdfImg);
                    if (connectList.Count > 0)
                    {
                        doc.GetPdfDocument().SetDefaultPageSize(new PageSize(pdfImg.GetImageWidth() + 25, pdfImg.GetImageHeight() + 50));
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        //SetRaporHeader(doc);
                        //SetRaporFooter(doc, pageNumber);
                    }

                    pdfImg.SetMargins(25,25,10,25);
                    doc.Add(pdfImg);
                    Paragraph captureDef = new Paragraph("Şekil 1 - Ağ Planlama Çalışma Alanı Ekran Görüntüsü");
                    captureDef.SetFontFamily(new string[] { "Arial", "serif" });
                    captureDef.SetFontSize(11);
                    captureDef.SetBold();
                    captureDef.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    doc.Add(captureDef);
                    doc.Close();

                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = "Rapor başarı ile oluşturuldu.";
                    nfp.Owner = Owner;
                    nfp.Show();

                    this.ClosePopup();
                }
            }
        }

        private int SetUcBirimTable(Document doc, List<NodeViewModel> ucBirimler, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph ucBirimHeader = new Paragraph("1. Uç Birimler");
            ucBirimHeader.SetFontFamily(new string[] { "Arial", "serif" });
            ucBirimHeader.SetFontSize(11);
            ucBirimHeader.SetBold();
            ucBirimHeader.SetMarginTop(50);
            doc.Add(ucBirimHeader);
            tempHeight = tempHeight - 50;

            if (ucBirimler.Count == 0)
            {
                Paragraph noUcBirimParagraph = new Paragraph("Uç Birim Bulunmamaktadır.");
                noUcBirimParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noUcBirimParagraph.SetFontSize(11);
                noUcBirimParagraph.SetMarginTop(25);
                doc.Add(noUcBirimParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;
                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (ucBirimler.Count > 0)
            {
                Table table = new Table(new float[7]);
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < ucBirimler.Count; j++)
                {
                    Cell c = new Cell();
                    c.SetMinWidth(120);
                    c.Add(new Paragraph(ucBirimler[row].Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetMinWidth(180);
                    c.Add(new Paragraph(ucBirimler[row].Tanim));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].StokNo));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(ucBirimler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(ucBirimler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(ucBirimler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[7]);
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight < (currentTableHeight - previousTableHeight))
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    tableTotal.SetMarginTop(25);
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Uç Birim Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(ucBirimler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                }
                else
                {
                    doc.Add(table);

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Uç Birim Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(ucBirimler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);
                }
            }

            return pageNumber;
        }

        private int SetAgAnahtariTable(Document doc, List<NodeViewModel> agAnahtarlari, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph agAnahtariHeader = new Paragraph("2. Ağ Anahtarları");
            agAnahtariHeader.SetFontFamily(new string[] { "Arial", "serif" });
            agAnahtariHeader.SetFontSize(11);
            agAnahtariHeader.SetBold();
            agAnahtariHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(agAnahtariHeader);

            if (agAnahtarlari.Count == 0)
            {
                Paragraph noAgAnahtariParagraph = new Paragraph("Ağ Anahtarı Bulunmamaktadır.");
                noAgAnahtariParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noAgAnahtariParagraph.SetFontSize(11);
                noAgAnahtariParagraph.SetMarginTop(25);
                doc.Add(noAgAnahtariParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;
                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (agAnahtarlari.Count > 0)
            {
                Table table = new Table(new float[7]);
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < agAnahtarlari.Count; j++)
                {
                    Cell c = new Cell();
                    c.SetMinWidth(120);
                    c.Add(new Paragraph(agAnahtarlari[row].Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetMinWidth(180);
                    c.Add(new Paragraph(agAnahtarlari[row].Tanim));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].StokNo));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(agAnahtarlari[row].InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(agAnahtarlari[row].OutputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(agAnahtarlari[row].InputList.Where(x => x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[7]);
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight < (currentTableHeight - previousTableHeight))
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    tableTotal.SetMarginTop(25);
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Ağ Anahtarı Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetBold();
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(agAnahtarlari.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);
                }
                else
                {
                    doc.Add(table);

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Ağ Anahtarı Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetBold();
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(agAnahtarlari.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);
                }
            }

            return pageNumber;
        }

        private int SetKenarToUcBirimTable(Document doc, List<ConnectViewModel> connectList, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph kenarUcBirimHeader = new Paragraph("3. Kenar Ağ Anahtarları ve Bağlı Olduğu Uç Birimler");
            kenarUcBirimHeader.SetFontFamily(new string[] { "Arial", "serif" });
            kenarUcBirimHeader.SetFontSize(11);
            kenarUcBirimHeader.SetBold();
            kenarUcBirimHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(kenarUcBirimHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun bağlantı bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(25);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Uç Birim Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < connectList.Count; j++)
                {
                    Cell c = new Cell();
                    if (connectList[row].FromConnector.Node.TypeId == (int)TipEnum.UcBirim)
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    if (connectList[row].FromConnector.Node.TypeId == (int)TipEnum.AgAnahtari)
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    tableRow++;
                    row++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[] { 300, 300, 193 });
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight >= (currentTableHeight - previousTableHeight))
                {
                    doc.Add(table);
                }
            }

            return pageNumber;
        }

        private int SetToplamaToKenarTable(Document doc, List<ConnectViewModel> connectList, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph toplamaKenarHeader = new Paragraph("4. Toplama Ağ Anahtarları ve Bağlı Olduğu Kenar Ağ Anahtarları");
            toplamaKenarHeader.SetFontFamily(new string[] { "Arial", "serif" });
            toplamaKenarHeader.SetFontSize(11);
            toplamaKenarHeader.SetBold();
            toplamaKenarHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(toplamaKenarHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun bağlantı bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(25);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Toplama Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Kenar Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < connectList.Count; j++)
                {
                    Cell c = new Cell();
                    if (connectList[row].FromConnector.Node.TurAd == "Toplama")
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    if (connectList[row].FromConnector.Node.TurAd == "Kenar")
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[] { 300, 300, 193 });
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight >= (currentTableHeight - previousTableHeight))
                {
                    doc.Add(table);
                }
            }

            return pageNumber;
        }

        private int SetOmurgaToKenarTable(Document doc, List<ConnectViewModel> connectList, int pageNumber, float emptyHeight, Image pdfImg)
        {
            float tempHeight = emptyHeight;

            Paragraph omurgaKenarHeader = new Paragraph("5. Omurga Ağ Anahtarları ve Bağlı Olduğu Kenar Ağ Anahtarları");
            omurgaKenarHeader.SetFontFamily(new string[] { "Arial", "serif" });
            omurgaKenarHeader.SetFontSize(11);
            omurgaKenarHeader.SetBold();
            omurgaKenarHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(omurgaKenarHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun bağlantı bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(25);
                doc.Add(noBaglantiParagraph);

                doc.GetPdfDocument().SetDefaultPageSize(new PageSize(pdfImg.GetImageWidth() + 25, pdfImg.GetImageHeight() + 50));
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                //SetRaporHeader(doc);
                //SetRaporFooter(doc, pageNumber);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Omurga Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Kenar Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < connectList.Count; j++)
                {
                    Cell c = new Cell();
                    if (connectList[row].FromConnector.Node.TurAd == "Omurga")
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    if (connectList[row].FromConnector.Node.TurAd == "Kenar")
                    {
                        c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    }
                    else
                    {
                        c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    }

                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[] { 300, 300, 193 });
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight >= (currentTableHeight - previousTableHeight))
                {
                    doc.Add(table);
                }
            }

            return pageNumber;
        }

        private void GucPlanlamaRaporuOlustur()
        {
            var NodesCanvas = (Owner as MainWindow).ViewModel.NodesCanvas;

            var grouplar = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.Group).ToList();
            var internalConnectList = new List<ConnectViewModel>();
            var externalConnectList = new List<ConnectViewModel>();

            var gucTuketiciler = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.UcBirim || x.TypeId == (int)TipEnum.AgAnahtari).ToList();
            var gucUreticiler = NodesCanvas.Nodes.Items.Where(x => x.TypeId == (int)TipEnum.GucUretici).ToList();

            foreach (var group in grouplar)
            {
                var nodeList = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.NodeList).FirstOrDefault();
                var temp = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.InternalConnectList).FirstOrDefault();
                var temp2 = NodesCanvas.GroupList.Where(x => x.UniqueId == group.UniqueId).Select(s => s.ExternalConnectList).FirstOrDefault();

                if (nodeList != null)
                {
                    foreach (var node in nodeList)
                    {
                        if (node.TypeId != (int)TipEnum.GucUretici)
                        {
                            gucTuketiciler.Add(node);
                        }
                        else
                        {
                            gucUreticiler.Add(node);
                        }
                    }
                }

                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        internalConnectList.Add(item);
                    }
                }

                if (temp2 != null)
                {
                    foreach (var item in temp2)
                    {
                        externalConnectList.Add(item);
                    }
                }
            }

            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string path = fbd.SelectedPath;

                    var img = iText.IO.Image.ImageDataFactory.Create(capture);
                    Image pdfImg = new Image(img);

                    int pageNumber = 1;
                    var outputStream = new FileStream(path + "\\Güç Planlama Raporu.pdf", FileMode.Create);
                    PdfDocument pdf = new PdfDocument(new PdfWriter(outputStream));
                    Document doc = new Document(pdf, PageSize.A4);
                    doc.SetFontProvider(new DefaultFontProvider(true, true, true));

                    float totalHeight = doc.GetPdfDocument().GetDefaultPageSize().GetHeight();
                    float headerHeight = SetRaporHeader(doc);
                    float footerHeight = SetRaporFooter(doc, pageNumber);

                    Paragraph header = new Paragraph("SEMA - AYP GÜÇ PLANLAMA RAPORU");
                    header.SetFontFamily(new string[] { "Arial", "serif" });
                    header.SetFontSize(16);
                    header.SetBold();
                    header.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    header.SetMarginTop(275);
                    doc.Add(header);
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                    float emptyHeight = totalHeight - (footerHeight + headerHeight);

                    pageNumber = SetGucTuketicilerTable(doc, gucTuketiciler, pageNumber, emptyHeight);
                    if (gucTuketiciler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    pageNumber = SetGucUreticilerTable(doc, gucUreticiler, pageNumber, emptyHeight);
                    if (gucUreticiler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }

                    List<ConnectViewModel> connectList = new List<ConnectViewModel>();
                    foreach (var connect in NodesCanvas.Connects.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (gucUreticiler.Contains(connect.FromConnector.Node))
                        {
                            if (gucTuketiciler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in internalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (gucUreticiler.Contains(connect.FromConnector.Node))
                        {
                            if (gucTuketiciler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    foreach (var connect in externalConnectList.OrderBy(o => o.ToConnector.Node.Name))
                    {
                        if (gucUreticiler.Contains(connect.FromConnector.Node))
                        {
                            if (gucTuketiciler.Contains(connect.ToConnector.Node))
                            {
                                connectList.Add(connect);
                            }
                        }
                    }

                    pageNumber = SetGucUreticiToGucTuketici(doc, connectList, pageNumber, emptyHeight, pdfImg);
                    if (connectList.Count > 0)
                    {
                        doc.GetPdfDocument().SetDefaultPageSize(new PageSize(pdfImg.GetImageWidth() + 25, pdfImg.GetImageHeight() + 50));
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        //SetRaporHeader(doc);
                        //SetRaporFooter(doc, pageNumber);
                    }

                    pdfImg.SetMargins(25, 25, 10, 25);
                    doc.Add(pdfImg);

                    Paragraph captureDef = new Paragraph("Şekil 1 - Güç Planlama Çalışma Alanı Ekran Görüntüsü");
                    captureDef.SetFontFamily(new string[] { "Arial", "serif" });
                    captureDef.SetFontSize(11);
                    captureDef.SetBold();
                    captureDef.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    doc.Add(captureDef);

                    doc.Close();

                    NotifySuccessPopup nfp = new NotifySuccessPopup();
                    nfp.msg.Text = "Rapor başarı ile oluşturuldu.";
                    nfp.Owner = Owner;
                    nfp.Show();

                    this.ClosePopup();
                }
            }
        }

        private int SetGucUreticilerTable(Document doc, List<NodeViewModel> gucUreticiler, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph gucUreticiHeader = new Paragraph("2. Güç Üreticiler");
            gucUreticiHeader.SetFontFamily(new string[] { "Arial", "serif" });
            gucUreticiHeader.SetFontSize(11);
            gucUreticiHeader.SetBold();
            gucUreticiHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(gucUreticiHeader);

            if (gucUreticiler.Count == 0)
            {
                Paragraph noGucUreticiParagraph = new Paragraph("Güç Üretici Bulunmamaktadır.");
                noGucUreticiParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noGucUreticiParagraph.SetFontSize(11);
                noGucUreticiParagraph.SetMarginTop(25);
                doc.Add(noGucUreticiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (gucUreticiler.Count > 0)
            {
                Table table = new Table(new float[6]);
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < gucUreticiler.Count; j++)
                {
                    Cell c = new Cell();
                    c.SetMinWidth(120);
                    c.Add(new Paragraph(gucUreticiler[row].Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetMinWidth(180);
                    c.Add(new Paragraph(gucUreticiler[row].Tanim));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].StokNo));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(gucUreticiler[row].InputList.Where(x => x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(gucUreticiler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[6]);
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight < (currentTableHeight - previousTableHeight))
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    tableTotal.SetMarginTop(25);
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Güç Üretici Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(gucUreticiler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                }
                else
                {
                    doc.Add(table);

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Güç Üretici Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(gucUreticiler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);
                }
            }

            return pageNumber;
        }

        private int SetGucTuketicilerTable(Document doc, List<NodeViewModel> gucTuketiciler, int pageNumber, float emptyHeight)
        {
            var tempHeight = emptyHeight;

            Paragraph gucTuketiciHeader = new Paragraph("1. Güç Tüketiciler");
            gucTuketiciHeader.SetFontFamily(new string[] { "Arial", "serif" });
            gucTuketiciHeader.SetFontSize(11);
            gucTuketiciHeader.SetBold();
            gucTuketiciHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(gucTuketiciHeader);

            if (gucTuketiciler.Count == 0)
            {
                Paragraph noGucTuketiciParagraph = new Paragraph("Güç Tüketici Bulunmamaktadır.");
                noGucTuketiciParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noGucTuketiciParagraph.SetFontSize(11);
                noGucTuketiciParagraph.SetMarginTop(25);
                doc.Add(noGucTuketiciParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                SetRaporHeader(doc);
                SetRaporFooter(doc, pageNumber);
            }

            if (gucTuketiciler.Count > 0)
            {
                Table table = new Table(new float[7]);
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.SetMinWidth(120);
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.SetMinWidth(180);
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.SetWidth(35);
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.SetWidth(35);
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.SetWidth(35);
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < gucTuketiciler.Count; j++)
                {
                    Cell c = new Cell();
                    c.SetMinWidth(120);
                    c.Add(new Paragraph(gucTuketiciler[row].Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetMinWidth(180);
                    c.Add(new Paragraph(gucTuketiciler[row].Tanim));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].StokNo));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(gucTuketiciler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu || x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(gucTuketiciler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu || x.TypeId == (int)TipEnum.AgAnahtariAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.SetWidth(35);
                    c.Add(new Paragraph(gucTuketiciler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimGucArayuzu || x.TypeId == (int)TipEnum.AgAnahtariGucArayuzu || x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[7]);
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight < (currentTableHeight - previousTableHeight))
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    tableTotal.SetMarginTop(25);
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Güç Tüketici Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(gucTuketiciler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                }
                else
                {
                    doc.Add(table);

                    Table tableTotal = new Table(new float[] { 400, 393 });
                    Cell c1 = new Cell();
                    c1.Add(new Paragraph("Toplam Güç Tüketici Sayısı"));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c1.SetBold();
                    tableTotal.AddCell(c1);
                    c1 = new Cell();
                    c1.Add(new Paragraph(gucTuketiciler.Count.ToString()));
                    c1.SetFontFamily(new string[] { "Arial", "serif" });
                    c1.SetFontSize(11);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c1);
                    doc.Add(tableTotal);
                }
            }

            return pageNumber;
        }

        private int SetGucUreticiToGucTuketici(Document doc, List<ConnectViewModel> connectList, int pageNumber, float emptyHeight, Image pdfImg)
        {
            var tempHeight = emptyHeight;

            Paragraph gucUreticiTuketiciHeader = new Paragraph("3. Güç Üreticiler ve Bağlı Olduğu Güç Tüketiciler");
            gucUreticiTuketiciHeader.SetFontFamily(new string[] { "Arial", "serif" });
            gucUreticiTuketiciHeader.SetFontSize(11);
            gucUreticiTuketiciHeader.SetBold();
            gucUreticiTuketiciHeader.SetMarginTop(50);
            tempHeight = tempHeight - 50;
            doc.Add(gucUreticiTuketiciHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun bağlantı bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Arial", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(25);
                doc.Add(noBaglantiParagraph);

                doc.GetPdfDocument().SetDefaultPageSize(new PageSize(pdfImg.GetImageWidth() + 25, pdfImg.GetImageHeight() + 50));
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                pageNumber += 1;

                //SetRaporHeader(doc);
                //SetRaporFooter(doc, pageNumber);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                table.SetMarginTop(25);
                tempHeight = tempHeight - 25;

                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Güç Üretici Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Güç Tüketici Adı"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Güç Miktarı (watt)"));
                        c.SetFontFamily(new string[] { "Arial", "serif" });
                        c.SetFontSize(11);
                        c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                int tableRow = 1;
                float previousTableHeight = 0;
                float currentTableHeight = CalculateTableHeight(doc, table);

                for (int j = 0; j < connectList.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].GucMiktari.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    table.AddCell(c);

                    row++;
                    tableRow++;
                    currentTableHeight = CalculateTableHeight(doc, table);

                    if (tempHeight < (currentTableHeight - previousTableHeight))
                    {
                        int columnNumbers = table.GetNumberOfColumns();
                        for (int k = (tableRow * columnNumbers) - 1; k > ((tableRow * columnNumbers) - 1) - columnNumbers; k--)
                        {
                            table.GetChildren().RemoveAt(k);
                        }

                        tableRow = 0;
                        row--;
                        j--;

                        doc.Add(table);
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        pageNumber += 1;

                        tempHeight = emptyHeight;
                        table = new Table(new float[] { 300, 300, 193 });
                        table.SetMarginTop(25);
                        tempHeight = tempHeight - 25;
                        previousTableHeight = 0;
                        currentTableHeight = CalculateTableHeight(doc, table);

                        SetRaporHeader(doc);
                        SetRaporFooter(doc, pageNumber);
                    }
                    else
                    {
                        tempHeight = tempHeight - (currentTableHeight - previousTableHeight);
                        previousTableHeight = currentTableHeight;
                    }
                }

                if (tempHeight < (currentTableHeight - previousTableHeight))
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    pageNumber += 1;

                    Table tableTotal = new Table(new float[] { 600, 193 });
                    tableTotal.SetMarginTop(25);
                    Cell c = new Cell();
                    c.Add(new Paragraph("Toplam Güç Tüketimi"));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    c.SetBold();
                    tableTotal.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList.Select(s => s.GucMiktari).Sum().ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c);
                    doc.Add(tableTotal);

                    SetRaporHeader(doc);
                    SetRaporFooter(doc, pageNumber);
                }
                else
                {
                    doc.Add(table);

                    Table tableTotal = new Table(new float[] { 600, 193 });
                    Cell c = new Cell();
                    c.Add(new Paragraph("Toplam Güç Tüketimi"));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList.Select(s => s.GucMiktari).Sum().ToString("0.##")));
                    c.SetFontFamily(new string[] { "Arial", "serif" });
                    c.SetFontSize(11);
                    c.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    tableTotal.AddCell(c);
                    doc.Add(tableTotal);
                }
            }

            return pageNumber;
        }

        private float SetRaporHeader(Document doc)
        {
            float headerHeight = 0;
            try
            {
                //string path = Directory.GetCurrentDirectory() + "\\SEMA_Data\\StreamingAssets\\AYP\\rapor-header.png";
                string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Images\rapor-header.png");
                log.Info(path);

                if (File.Exists(path))
                {
                    ImageData imageData = iText.IO.Image.ImageDataFactory.Create(path);
                    Image pdfImg = new Image(imageData);
                    pdfImg.SetWidth(532);
                    headerHeight = pdfImg.GetImageHeight();
                    doc.Add(pdfImg);
                }
            }
            catch (Exception exception)
            {
                log.Error("Rapor başlığı okunamadı. - " + exception.InnerException?.Message);
            }

            return headerHeight;
        }

        private float SetRaporFooter(Document doc, int pageNumber)
        {
            Table table = new Table(new float[] { 110, 75, 55, 50, 45, 45, 70, 70 });

            Cell c = new Cell(1, 1);
            c.SetMaxWidth(110);
            c.Add(new Paragraph("Onay-Approved By").SetFontSize(6));
            c.Add(new Paragraph(model.Onaylayan != null ? model.Onaylayan : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell(3, 5);
            c.SetMaxWidth(270);
            c.Add(new Paragraph("Tanım-Description").SetFontSize(6));
            c.Add(new Paragraph(model.DokumanTanimi != null ? model.DokumanTanimi : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell(2, 2);
            c.SetMaxWidth(140);
            c.Add(new Paragraph("Doküman/Parça Numarası-Document/Part Number").SetFontSize(6));
            c.Add(new Paragraph(model.DokumanParcaNo != null ? model.DokumanParcaNo : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(110);
            c.Add(new Paragraph("Kontrol-Checked By").SetFontSize(6));
            c.Add(new Paragraph(model.KontrolEden != null ? model.KontrolEden : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(110);
            c.Add(new Paragraph("Hazırlayan-Prepared By").SetFontSize(6));
            c.Add(new Paragraph(model.Hazirlayan != null ? model.Hazirlayan : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(70);
            c.Add(new Paragraph("Rev Kodu-Rev Code").SetFontSize(6));
            c.Add(new Paragraph(model.RevizyonKodu != null ? model.RevizyonKodu : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(70);
            c.Add(new Paragraph("Değişiklik Tarihi-Change Date").SetFontSize(6));
            c.Add(new Paragraph(model.DegistirmeTarihi.HasValue ? model.DegistirmeTarihi.Value.ToString("dd/MM/yyyy") : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(110);
            c.Add(new Paragraph("Bölüm-Department").SetFontSize(6));
            c.Add(new Paragraph(model.Bolum != null ? model.Bolum : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);


            c = new Cell();
            c.SetMaxWidth(75);
            c.Add(new Paragraph("Yazım Ortamı-Editing Env").SetFontSize(6));
            c.Add(new Paragraph(model.YazimOrtami != null ? model.YazimOrtami : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(55);
            c.Add(new Paragraph("Dok Kodu-Doc Code").SetFontSize(6));
            c.Add(new Paragraph(model.DokumanKodu != null ? model.DokumanKodu : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(50);
            c.Add(new Paragraph("Syf-Pg").SetFontSize(6));
            c.Add(new Paragraph(pageNumber.ToString()).SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(45);
            c.Add(new Paragraph("Tarih-Date").SetFontSize(6));
            c.Add(new Paragraph(model.Tarih.HasValue ? model.Tarih.Value.ToString("dd/MM/yyyy") : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(45);
            c.Add(new Paragraph("Dil-Lng").SetFontSize(6));
            c.Add(new Paragraph(model.DilKodu != null ? model.DilKodu : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(70);
            c.Add(new Paragraph("Değiştiren-Changed By").SetFontSize(6));
            c.Add(new Paragraph(model.Degistiren != null ? model.Degistiren : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);

            c = new Cell();
            c.SetMaxWidth(70);
            c.Add(new Paragraph("Boyut-Size").SetFontSize(6));
            c.Add(new Paragraph(model.SayfaBoyutu != null ? model.SayfaBoyutu : "").SetFontSize(9).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            c.SetFontFamily(new string[] { "Arial", "serif" });
            table.AddCell(c);
            table.SetFixedPosition(38, 20, 520);
            doc.Add(table);

            float footerHeight = CalculateTableHeight(doc, table) + 20;
            return footerHeight;
        }

        private float CalculateTableHeight(Document doc, Table table)
        {
            var test = table.CreateRendererSubTree().SetParent(doc.GetRenderer()).Layout(new LayoutContext(new LayoutArea(1, new Rectangle(0, 0, 400, 10000.0F))));
            var tableHeight = test.GetOccupiedArea().GetBBox().GetHeight();

            return tableHeight;
        }
    }
}
