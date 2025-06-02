using System.Net.Http;
using System.Net.Http.Json;
using FishingPlanner.Models;
using FishingPlanner.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace FishingPlanner.Services
{
    public class FishingForecastService : IFishingDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;
        private readonly string _apiToken;

        public FishingForecastService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            this.configuration = configuration;

            _apiToken = configuration.GetSection("GlobalFishingWatchApi")["GlobalFishingWatchApiToken"]
                ?? throw new ArgumentNullException("GlobalFishingWatchApiToken was not present on appsettings.json");
        }

        public async Task<FishingDayStat> GetFishingStatAsync(DateTime date, double lat, double lon)
        {
            var latMin = lat - 0.1;
            var latMax = lat + 0.1;
            var lonMin = lon - 0.1;
            var lonMax = lon + 0.1;

            var boundingBox = $"[{lonMin},{latMin},{lonMax},{latMax}]";

            string url = $"https://gateway.api.globalfishingwatch.org/v3/events" +
                         $"?datasets[0]=public-global-fishing-events:latest" +
                         $"&start-date={date:yyyy-MM-dd}" +
                         $"&end-date={date:yyyy-MM-dd}" +
                         $"&boundingBox={boundingBox}" +
                         $"&limit=5";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_apiToken}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new FishingDayStat
                {
                    Date = date,
                    IsFishActive = false,
                    Description = "Нет данных по рыбалке для этого места"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<GlobalFishingWatchResponse>();

            if (result == null || result.Total == 0)
            {
                return new FishingDayStat
                {
                    Date = date,
                    IsFishActive = false,
                    Description = "Рыбалка в этот день неактивна"
                };
            }

            return new FishingDayStat
            {
                Date = date,
                IsFishActive = true,
                Description = $"Найдено {result.Total} рыболовных событий в этом районе"
            };
        }
    }

    public class GlobalFishingWatchResponse
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("entries")]
        public FishingEventEntry[] Entries { get; set; } = [];
    }

    public class FishingEventEntry
    {
        [JsonPropertyName("start")]
        public string Start { get; set; } = string.Empty;

        [JsonPropertyName("end")]
        public string End { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public Position Position { get; set; } = new();

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class Position
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }
}