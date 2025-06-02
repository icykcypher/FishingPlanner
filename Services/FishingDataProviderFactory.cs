using FishingPlanner.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FishingPlanner.Services
{
    public class FishingDataProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _dataSource;

        public FishingDataProviderFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _dataSource = configuration["FishingDataSource"] ?? "weather";
        }

        public IFishingDataProvider Create()
        {
            return _dataSource switch
            {
                "weather" => _serviceProvider.GetRequiredService<FishingForecastService>(),
                "stats" => _serviceProvider.GetRequiredService<StatsFishingDataProvider>(),
                _ => throw new InvalidOperationException("Unknown fishing data source")
            };
        }
    }

}