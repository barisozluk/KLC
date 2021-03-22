using AYP.DbContext.AYP.DbContexts;
using AYP.Helpers.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using System.Windows;
using WritableJsonConfiguration;

namespace AYP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {            

                Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
                Locator.CurrentMutable.RegisterConstant(new ConverterBoolAndVisibility(), typeof(IBindingTypeConverter));

                IConfigurationRoot configuration;
                configuration = WritableJsonConfigurationFabric.Create("Settings.json");
                Locator.CurrentMutable.RegisterConstant(configuration, typeof(IConfiguration));
        }

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                AYPContext context = new AYPContext();
                context.Database.Migrate();

                MainWindow window = new MainWindow();
                window.Show();
            }
            catch (Exception exception)
            {
                DbConnectionErrorPopupWindow popup = new DbConnectionErrorPopupWindow();
                popup.Show();
            }
        }
    }
}
