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
    public partial class ExportLibraryPopupWindow : Window
    {

        private string fileName;

        public MainWindow MainWindow { get; set; }

        public ExportLibraryPopupWindow()
        {
            InitializeComponent();
        }

        private void Save_ProjectLibrary(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Trusted_Connection=True;Database=master"))
                {
                    connection.Open();

                    var tableNames = new List<string>();
                    //using (var command = new SqlCommand(@"SELECT table_name FROM information_schema.tables where table_schema = @database", connection))
                    using (var command = new SqlCommand(@"SELECT name FROM master.sys.tables where is_ms_shipped=0", connection))
                    {
                        command.Parameters.AddWithValue("@database", "master");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                tableNames.Add(reader.GetString(0));
                        }
                    }

                    // open a JSON file for output; use the streaming JsonTextWriter interface to avoid high memory usage
                    using (var streamWriter = new StreamWriter(fileName))
                    using (var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Newtonsoft.Json.Formatting.Indented, Indentation = 2, IndentChar = ' ' })
                    {
                        // one array to hold all tables
                        jsonWriter.WriteStartArray();

                        foreach (var tableName in tableNames)
                        {
                            // an object for each table
                            jsonWriter.WriteStartObject();
                            jsonWriter.WritePropertyName("tableName");
                            jsonWriter.WriteValue(tableName);
                            jsonWriter.WritePropertyName("rows");

                            // an array for all the rows in the table
                            jsonWriter.WriteStartArray();

                            // select all the data from each table
                            using (var command = new SqlCommand($@"SELECT * FROM {tableName}", connection))
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // write each row as a JSON object
                                    jsonWriter.WriteStartObject();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        jsonWriter.WritePropertyName(reader.GetName(i));
                                        jsonWriter.WriteValue(reader.GetValue(i));
                                    }
                                    jsonWriter.WriteEndObject();
                                }
                            }

                            jsonWriter.WriteEndArray();
                            jsonWriter.WriteEndObject();
                        }

                        jsonWriter.WriteEndArray();

                        NotifySuccessPopup nfp = new NotifySuccessPopup();
                        nfp.msg.Text = "Kütüphane başarıyla kaydedildi.";
                        nfp.Owner = this.MainWindow;
                        nfp.Show();

                        Close();
                        Owner.IsEnabled = true;
                        Owner.Effect = null;
                    }
                }
            }
            catch
            {
                NotifyWarningPopup nfp = new NotifyWarningPopup();
                nfp.msg.Text = "Veritabanı bağlantısı oluşturulamadı. Kütüphane aktarılamadı.";
                nfp.Owner = this.MainWindow;
                nfp.Show();
            }
        }

        private void ButtonExportLibraryPopupClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.IsEnabled = true;
            Owner.Effect = null;
        }

        private void ExportJSONFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json";

            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;
                ExportJSON.Text = fileName ;
            }
        }
    }
}
