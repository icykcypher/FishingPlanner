using System.Net.Http;
using System.Net.Http.Json;
using FishingPlanner.Models;
using FishingPlanner.Interfaces;

namespace FishingPlanner.Services
{
    public class StatsFishingDataProvider : IFishingDataProvider
    {
        private readonly HttpClient _httpClient;

        public StatsFishingDataProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FishingDayStat> GetFishingStatAsync(DateTime date, double lan , double lon)
        {
            string url = $"fishingstats?date={date:yyyy-MM-dd}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new FishingDayStat { Date = date, IsFishActive = false, Description = "No data" };

            var stats = await response.Content.ReadFromJsonAsync<FishingStatsResponse>();

            bool isActive = stats.FishActivityScore > 70;

            string desc = isActive ? "Fish is active" : "Fish is not active";

            return new FishingDayStat
            {
                Date = date,
                IsFishActive = isActive,
                Description = desc
            };
        }
    }

    public class FishingStatsResponse
    {
        public int FishActivityScore { get; set; }
    }
}