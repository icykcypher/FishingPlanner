using Serilog;
using System.Windows;
using FishingPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FishingPlanner
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public IConfiguration Configuration { get; }

        public App()
        {
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

            services.AddDbContext<FishingPlannerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            Services = services.BuildServiceProvider();

            // DB Migration
            try
            {
                using var scope = Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FishingPlannerDbContext>();
                context.Database.Migrate();
                Log.Information("Миграция успешно выполнена.");
            }
            catch (Exception e)
            {
                Log.Error(e, "Ошибка при миграции базы данных");
            }
        }
    }
}