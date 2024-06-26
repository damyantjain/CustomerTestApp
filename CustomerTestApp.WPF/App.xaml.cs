using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CustomerTestApp.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;
using CustomerTestApp.WPF.Services;

namespace CustomerTestApp.WPF
{
    /// <summary>
    /// The App class is the entry point of the application.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The OnStartup method is called when the application starts.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// The ConfigureServices method configures the services for the application.
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            var config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddSingleton<CustomerDataViewModel>();
            services.AddTransient<CustomerEditViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ICustomerService, CustomerService>();
        }

        private IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
