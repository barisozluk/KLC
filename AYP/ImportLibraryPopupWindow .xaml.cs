using AYP.DbContext.AYP.DbContexts;
using AYP.Entities;
using AYP.Helpers.Notifications;
using AYP.Interfaces;
using AYP.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Media;


namespace AYP
{
    /// <summary>
    /// Interaction logic for AgAnahtariTurPopWindow.xaml
    /// </summary>
    public partial class ImportLibraryPopupWindow : Window
    {

        private string fileName;

        public MainWindow MainWindow { get; set; }

        public ImportLibraryPopupWindow()
        {
            InitializeComponent();
        }

        private void Import_ProjectLibrary(object sender, RoutedEventArgs e)
        {
            List<GucArayuzu> ga1 = JsonConvert.DeserializeObject<List<GucArayuzu>>(File.ReadAllText(fileName)); 
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                GucArayuzu ga = (GucArayuzu)serializer.Deserialize(file, typeof(GucArayuzu));
            }
        }

        private void ButtonImportLibraryPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ImportJSONFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                ImportJSON.Text = fileName ;
            }
        }
    }
}
