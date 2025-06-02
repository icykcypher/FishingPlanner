using FishingPlanner.Models;

namespace FishingPlanner.Interfaces
{
    public interface IFishingDataProvider
    {
        Task<FishingDayStat> GetFishingStatAsync(DateTime date, double latitude, double longitude);
    }
}