using Serilog;
using System.Windows;
using FishingPlanner.Data;
using FishingPlanner.Views;
using FishingPlanner.Services;
using System.Net.Http.Headers;
using FishingPlanner.Interfaces;
using FishingPlanner.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FishingPlanner
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public IConfiguration Configuration { get; }
        public static object CalendarView { get; set; } = null!;

        public App()
        {
            SetBrowserFeatureControl();

            // Configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false);

            Configuration = configBuilder.Build();

            // Serilog global logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("App started...");

            // DI
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);
            try
            {
                services.AddDbContext<FishingPlannerDbContext>(options =>
                    options.UseSqlServer
                    (
                        Configuration.GetConnectionString("DefaultConnection"))
                            .EnableSensitiveDataLogging()
                            .LogTo(m => Log.Information(m))
                    );
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while connecting to DB: " + e.Message, "DB Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            services.AddSingleton<FishingForecastService>();
            services.AddScoped<IFishingEventRepository, FishingEventRepository>();
            services.AddScoped<IFishingEventService, FishingEventService>();
            services.AddHttpClient<FishingForecastService>(c =>
            {
                c.BaseAddress = new Uri(Configuration["GlobalFishingWatchApi:BaseUrl"]!);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["GlobalFishingWatchApi:Token"]);
            });
            services.AddHttpClient<StatsFishingDataProvider>(c =>
            {
                c.BaseAddress = new Uri(Configuration["GlobalFishingWatchApi:BaseUrl"]!);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["GlobalFishingWatchApi:Token"]);
            });
            services.AddSingleton<FishingDataProviderFactory>();
            services.AddScoped<IFishingDataProvider>(provider =>
            {
                var factory = provider.GetRequiredService<FishingDataProviderFactory>();
                return factory.Create();
            });

            // ViewModels and Views
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<CalendarViewModel>();
            services.AddSingleton<CalendarView>();
            services.AddSingleton<AddEventView>();
            services.AddSingleton<TipsView>();
            services.AddTransient<DayDetailView>();
            services.AddSingleton<MainWindow>();

            Services = services.BuildServiceProvider();

            // DB Migration
            try
            {
                using var scope = Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FishingPlannerDbContext>();
                context.Database.Migrate();
                Log.Information("Migration completed successfully.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while migrating to DB: " + e.Message, "Migration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Error(e, "Error while migrating to DB");
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var mainViewModel = Services.GetRequiredService<MainViewModel>();
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Error(ex, "Error");
            }
        }

        private void SetBrowserFeatureControl()
        {
            try
            {
                string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName);

                using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                {
                    key?.SetValue(appName, 11001, Microsoft.Win32.RegistryValueKind.DWord); // IE11 Edge mode
                }

                using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION"))
                {
                    key?.SetValue(appName, 1, Microsoft.Win32.RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка установки режима браузера: " + ex.Message);
            }
        }
    }
}