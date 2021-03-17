using AYP.DbContext.AYP.DbContexts;
using AYP.Interfaces;
using AYP.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;


namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariPopupWindow.xaml
    /// </summary>
    public partial class AgAnahtariPopupWindow : Window
    {
        public byte[] selectedKatalogFile = null;
        public byte[] selectedSembolFile = null;

        MainWindow window;

        private AYPContext context;

        private IAgAnahatariService service;
        public AgAnahtariPopupWindow(MainWindow window)
        {
            this.window = window;
            this.context = new AYPContext();
            service = new AgAnahatariService(this.context);

            InitializeComponent();
        }

        private void ButtonAgAnahtariPopupClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.window.IsEnabled = true;
        }

        private void BtnOpenKatalogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf;)|*.pdf;";

            if (openFileDialog.ShowDialog() == true)
            {

                KatalogAgAnahtari.Text = Path.GetFileName(openFileDialog.FileName);
                this.selectedKatalogFile = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void BtnOpenSembolFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {

                SembolAgAnahtari.Text = Path.GetFileName(openFileDialog.FileName);
                this.selectedSembolFile = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
    }
}
