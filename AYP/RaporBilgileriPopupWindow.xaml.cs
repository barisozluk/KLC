using AYP.Enums;
using AYP.Models;
using AYP.ViewModel;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
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

        private void UygulaButton_Click(object sender, RoutedEventArgs e)
        {
            if(raporTipi == "Ağ Planlama Raporu")
            {
                AgPlanlamaRaporuOlustur();
            }
            else
            {
                GucPlanlamaRaporuOlustur();
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

                    var outputStream = new FileStream(path + "\\Ağ Planlama Raporu.pdf", FileMode.Create);
                    PdfDocument pdf = new PdfDocument(new PdfWriter(outputStream));
                    Document doc = new Document(pdf, PageSize.A4);
                    doc.SetFontProvider(new DefaultFontProvider(true, true, true));

                    SetRaporHeader(doc);

                    Paragraph header = new Paragraph("SEMA - AYP AĞ PLANLAMA RAPORU");
                    header.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    header.SetFontSize(16);
                    header.SetBold();
                    header.SetMarginLeft(130);
                    header.SetMarginTop(120);
                    doc.Add(header);

                    SetUcBirimTable(doc, ucBirimler);
                    if (ucBirimler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
                    }

                    SetAgAnahtariTable(doc, agAnahtarlari);
                    if (agAnahtarlari.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
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

                    SetKenarToUcBirimTable(doc, connectList);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
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

                    SetToplamaToKenarTable(doc, connectList);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
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

                    SetOmurgaToKenarTable(doc, connectList);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
                    }

                    var img = iText.IO.Image.ImageDataFactory.Create(capture);
                    Image pdfImg = new Image(img);
                    pdfImg.SetWidth(532);
                    pdfImg.SetMarginTop(120);
                    doc.Add(pdfImg);

                    doc.Close();
                }
            }
        }

        private void SetUcBirimTable(Document doc, List<NodeViewModel> ucBirimler)
        {
            Paragraph ucBirimHeader = new Paragraph("1. Uç Birimler");
            ucBirimHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            ucBirimHeader.SetFontSize(11);
            ucBirimHeader.SetBold();
            ucBirimHeader.SetMarginTop(50);
            doc.Add(ucBirimHeader);

            if (ucBirimler.Count == 0)
            {
                Paragraph noUcBirimParagraph = new Paragraph("Uç Birim Bulunmamaktadır.");
                noUcBirimParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noUcBirimParagraph.SetFontSize(11);
                noUcBirimParagraph.SetMarginTop(20);
                doc.Add(noUcBirimParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (ucBirimler.Count > 0)
            {
                Table table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                for (int j = 0; j < ucBirimler.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].Tanim));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].StokNo));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(ucBirimler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (ucBirimler.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (ucBirimler.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (ucBirimler.Count > 10 && ucBirimler.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (ucBirimler.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }

                Table tableTotal = new Table(new float[] { 400, 393 });
                Cell c1 = new Cell();
                c1.Add(new Paragraph("Toplam"));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                c1.SetBold();
                tableTotal.AddCell(c1);
                c1 = new Cell();
                c1.Add(new Paragraph(ucBirimler.Count.ToString()));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                tableTotal.AddCell(c1);
                doc.Add(tableTotal);
            }
        }

        private void SetAgAnahtariTable(Document doc, List<NodeViewModel> agAnahtarlari)
        {
            Paragraph agAnahtariHeader = new Paragraph("2. Ağ Anahtarları");
            agAnahtariHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            agAnahtariHeader.SetFontSize(11);
            agAnahtariHeader.SetBold();
            agAnahtariHeader.SetMarginTop(120);
            doc.Add(agAnahtariHeader);

            if (agAnahtarlari.Count == 0)
            {
                Paragraph noAgAnahtariParagraph = new Paragraph("Ağ Anahtarı Bulunmamaktadır.");
                noAgAnahtariParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noAgAnahtariParagraph.SetFontSize(11);
                noAgAnahtariParagraph.SetMarginTop(20);
                doc.Add(noAgAnahtariParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (agAnahtarlari.Count > 0)
            {
                Table table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                for (int j = 0; j < agAnahtarlari.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].Tanim));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].StokNo));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].OutputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(agAnahtarlari[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (agAnahtarlari.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (agAnahtarlari.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (agAnahtarlari.Count > 10 && agAnahtarlari.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (agAnahtarlari.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }

                Table tableTotal = new Table(new float[] { 400, 393 });
                Cell c1 = new Cell();
                c1.Add(new Paragraph("Toplam"));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                c1.SetBold();
                tableTotal.AddCell(c1);
                c1 = new Cell();
                c1.Add(new Paragraph(agAnahtarlari.Count.ToString()));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                tableTotal.AddCell(c1);
                doc.Add(tableTotal);
            }
        }

        private void SetKenarToUcBirimTable(Document doc, List<ConnectViewModel> connectList)
        {
            Paragraph kenarUcBirimHeader = new Paragraph("3. Kenar Ağ Anahtarları ve Bağlı Olduğu Uç Birimler");
            kenarUcBirimHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            kenarUcBirimHeader.SetFontSize(11);
            kenarUcBirimHeader.SetBold();
            kenarUcBirimHeader.SetMarginTop(120);
            doc.Add(kenarUcBirimHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun Bağlantı Bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(20);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Uç Birim Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (connectList.Count > 10 && connectList.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (connectList.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }
            }
        }

        private void SetToplamaToKenarTable(Document doc, List<ConnectViewModel> connectList)
        {
            Paragraph toplamaKenarHeader = new Paragraph("4. Toplama Ağ Anahtarları ve Bağlı Olduğu Kenar Ağ Anahtarları");
            toplamaKenarHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            toplamaKenarHeader.SetFontSize(11);
            toplamaKenarHeader.SetBold();
            toplamaKenarHeader.SetMarginTop(120);
            doc.Add(toplamaKenarHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun Bağlantı Bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(20);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Toplama Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Kenar Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (connectList.Count > 10 && connectList.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (connectList.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }
            }
        }

        private void SetOmurgaToKenarTable(Document doc, List<ConnectViewModel> connectList)
        {
            Paragraph omurgaKenarHeader = new Paragraph("5. Omurga Ağ Anahtarları ve Bağlı Olduğu Kenar Ağ Anahtarları");
            omurgaKenarHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            omurgaKenarHeader.SetFontSize(11);
            omurgaKenarHeader.SetBold();
            omurgaKenarHeader.SetMarginTop(120);
            doc.Add(omurgaKenarHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun Bağlantı Bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(20);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Omurga Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Kenar Ağ Anahtarı Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Ağ Yükü (mbps)"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
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

                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].AgYuku.ToString("0.##")));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (connectList.Count > 10 && connectList.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (connectList.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }
            }
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

                    var outputStream = new FileStream(path + "\\Güç Planlama Raporu.pdf", FileMode.Create);
                    PdfDocument pdf = new PdfDocument(new PdfWriter(outputStream));
                    Document doc = new Document(pdf, PageSize.A4);
                    doc.SetFontProvider(new DefaultFontProvider(true, true, true));

                    SetRaporHeader(doc);

                    iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph("SEMA - AYP GÜÇ PLANLAMA RAPORU");
                    header.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    header.SetFontSize(16);
                    header.SetBold();
                    header.SetMarginLeft(130);
                    header.SetMarginTop(120);
                    doc.Add(header);

                    SetGucTuketicilerTable(doc, gucTuketiciler);
                    if (gucTuketiciler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
                    }

                    SetGucUreticilerTable(doc, gucUreticiler);
                    if (gucUreticiler.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
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

                    SetGucUreticiToGucTuketici(doc, connectList);
                    if (connectList.Count > 0)
                    {
                        doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        SetRaporHeader(doc);
                    }

                    var img = iText.IO.Image.ImageDataFactory.Create(capture);
                    Image pdfImg = new Image(img);
                    pdfImg.SetWidth(532);
                    pdfImg.SetMarginTop(120);
                    doc.Add(pdfImg);

                    doc.Close();
                }
            }
        }

        private void SetGucUreticilerTable(Document doc, List<NodeViewModel> gucUreticiler)
        {
            Paragraph gucUreticiHeader = new Paragraph("2. Güç Üreticiler");
            gucUreticiHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            gucUreticiHeader.SetFontSize(11);
            gucUreticiHeader.SetBold();
            gucUreticiHeader.SetMarginTop(50);
            doc.Add(gucUreticiHeader);

            if (gucUreticiler.Count == 0)
            {
                Paragraph noGucUreticiParagraph = new Paragraph("Güç Üretici Bulunmamaktadır.");
                noGucUreticiParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noGucUreticiParagraph.SetFontSize(11);
                noGucUreticiParagraph.SetMarginTop(20);
                doc.Add(noGucUreticiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (gucUreticiler.Count > 0)
            {
                Table table = new Table(new float[] { 125, 168, 125, 125, 125, 125 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                for (int j = 0; j < gucUreticiler.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].Tanim));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].StokNo));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].InputList.Where(x => x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucUreticiler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.GucUreticiGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (gucUreticiler.Count > row)
                            {
                                table = new Table(new float[] { 125, 168, 125, 125, 125, 125 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (gucUreticiler.Count > row)
                            {
                                table = new Table(new float[] { 125, 168, 125, 125, 125, 125 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (gucUreticiler.Count > 10 && gucUreticiler.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (gucUreticiler.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }

                Table tableTotal = new Table(new float[] { 400, 393 });
                Cell c1 = new Cell();
                c1.Add(new Paragraph("Toplam"));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                c1.SetBold();
                tableTotal.AddCell(c1);
                c1 = new Cell();
                c1.Add(new Paragraph(gucUreticiler.Count.ToString()));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                tableTotal.AddCell(c1);
                doc.Add(tableTotal);
            }
        }

        private void SetGucTuketicilerTable(Document doc, List<NodeViewModel> gucTuketiciler)
        {
            Paragraph gucTuketiciHeader = new Paragraph("1. Güç Tüketiciler");
            gucTuketiciHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            gucTuketiciHeader.SetFontSize(11);
            gucTuketiciHeader.SetBold();
            gucTuketiciHeader.SetMarginTop(50);
            doc.Add(gucTuketiciHeader);

            if (gucTuketiciler.Count == 0)
            {
                Paragraph noGucTuketiciParagraph = new Paragraph("Güç Tüketici Bulunmamaktadır.");
                noGucTuketiciParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noGucTuketiciParagraph.SetFontSize(11);
                noGucTuketiciParagraph.SetMarginTop(20);
                doc.Add(noGucTuketiciParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (gucTuketiciler.Count > 0)
            {
                Table table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Birim Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Tanım"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Stok No"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 3)
                    {
                        c.Add(new Paragraph("Üretici"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 4)
                    {
                        c.Add(new Paragraph("Girdi Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 5)
                    {
                        c.Add(new Paragraph("Çıktı Ağ Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 6)
                    {
                        c.Add(new Paragraph("Güç Arayüzü Sayısı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                for (int j = 0; j < gucTuketiciler.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].Tanim));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].StokNo));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].UreticiAdi));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].OutputList.Where(x => x.TypeId == (int)TipEnum.UcBirimAgArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(gucTuketiciler[row].InputList.Where(x => x.TypeId == (int)TipEnum.UcBirimGucArayuzu).Count().ToString()));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (gucTuketiciler.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (gucTuketiciler.Count > row)
                            {
                                table = new Table(new float[] { 100, 143, 100, 100, 100, 100, 100 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (gucTuketiciler.Count > 10 && gucTuketiciler.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (gucTuketiciler.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }

                Table tableTotal = new Table(new float[] { 400, 393 });
                Cell c1 = new Cell();
                c1.Add(new Paragraph("Toplam"));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                c1.SetBold();
                tableTotal.AddCell(c1);
                c1 = new Cell();
                c1.Add(new Paragraph(gucTuketiciler.Count.ToString()));
                c1.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                c1.SetFontSize(11);
                tableTotal.AddCell(c1);
                doc.Add(tableTotal);
            }
        }

        private void SetGucUreticiToGucTuketici(Document doc, List<ConnectViewModel> connectList)
        {

            Paragraph gucUreticiTuketiciHeader = new Paragraph("3. Güç Üreticiler ve Bağlı Olduğu Güç Tüketiciler");
            gucUreticiTuketiciHeader.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
            gucUreticiTuketiciHeader.SetFontSize(11);
            gucUreticiTuketiciHeader.SetBold();
            gucUreticiTuketiciHeader.SetMarginTop(120);
            doc.Add(gucUreticiTuketiciHeader);

            if (connectList.Count == 0)
            {
                Paragraph noBaglantiParagraph = new Paragraph("Uygun Bağlantı Bulunmamaktadır.");
                noBaglantiParagraph.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                noBaglantiParagraph.SetFontSize(11);
                noBaglantiParagraph.SetMarginTop(20);
                doc.Add(noBaglantiParagraph);
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                SetRaporHeader(doc);
            }

            if (connectList.Count > 0)
            {
                Table table = new Table(new float[] { 300, 300, 193 });
                for (int j = 0; j < table.GetNumberOfColumns(); j++)
                {
                    Cell c = new Cell();

                    if (j == 0)
                    {
                        c.Add(new Paragraph("Güç Üretici Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 1)
                    {
                        c.Add(new Paragraph("Güç Tüketici Adı"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    if (j == 2)
                    {
                        c.Add(new Paragraph("Güç Miktarı (watt)"));
                        c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                        c.SetFontSize(11);
                        c.SetBold();
                    }

                    table.AddCell(c);
                }

                int row = 0;
                for (int j = 0; j < connectList.Count; j++)
                {
                    Cell c = new Cell();
                    c.Add(new Paragraph(connectList[row].FromConnector.Node.Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph(connectList[row].ToConnector.Node.Name));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    c = new Cell();
                    c.Add(new Paragraph("-"));
                    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
                    c.SetFontSize(11);
                    table.AddCell(c);

                    row++;
                    if (row >= 10 && row % 10 == 0)
                    {
                        if (row == 10)
                        {
                            table.SetMarginTop(20);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }
                        else
                        {
                            table.SetMarginTop(120);
                            doc.Add(table);

                            if (connectList.Count > row)
                            {
                                table = new Table(new float[] { 300, 300, 193 });
                                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                                SetRaporHeader(doc);
                            }
                        }

                    }
                }

                if (connectList.Count > 10 && connectList.Count % 10 != 0)
                {
                    table.SetMarginTop(120);
                    doc.Add(table);
                }
                else if (connectList.Count < 10)
                {
                    table.SetMarginTop(20);
                    doc.Add(table);
                }
            }
        }

        private void SetRaporHeader(Document doc)
        {
            ImageData imageData = iText.IO.Image.ImageDataFactory.Create("C:\\Users\\KLC\\Downloads\\header.png");
            Image pdfImg = new Image(imageData);
            pdfImg.SetWidth(532);
            doc.Add(pdfImg);
        }

        //private void SetRaporFooter(Document doc)
        //{
        //    Table table = new Table(new float[] { 150, 493, 150 });

        //    Cell c = new Cell();
        //    c.Add(new Paragraph("Onay-Approved By").SetFontSize(8));
        //    c.Add(new Paragraph(model.Onaylayan).SetFontSize(11));
        //    c.SetFontFamily(new string[] { "Times New Roman", "Times", "serif" });
        //    table.AddCell(c);

        //}
    }
}
