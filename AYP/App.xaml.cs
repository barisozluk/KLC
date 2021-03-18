using AYP.Helpers.Converters;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;
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
    }
}
