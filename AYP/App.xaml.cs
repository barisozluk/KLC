using AYP.DbContext.AYP.DbContexts;
using AYP.Helpers.Converters;
using log4net;
using log4net.Appender;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using WritableJsonConfiguration;

namespace AYP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        
        public App()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
                Locator.CurrentMutable.RegisterConstant(new ConverterBoolAndVisibility(), typeof(IBindingTypeConverter));
                
                IConfigurationRoot configuration;
                configuration = WritableJsonConfigurationFabric.Create("Settings.json");
                Locator.CurrentMutable.RegisterConstant(configuration, typeof(IConfiguration));
        }

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            log4net.GlobalContext.Properties["LogFileName"] = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"AYP");
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  AYP Başlatıldı.  =============        ");

            try
            {
                AYPContext context = new AYPContext();
                context.Database.Migrate();

                MainWindow window = new MainWindow();
                window.Show();
            }
            catch (Exception exception)
            {
                log.Error("Veritabanı bağlantısı kurulamadı. - " + exception.Message);
                DbConnectionErrorPopupWindow popup = new DbConnectionErrorPopupWindow();
                popup.Show();
            }
        }
    }
}
