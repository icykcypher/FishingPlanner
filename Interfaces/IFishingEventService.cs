using FishingPlanner.Models;

namespace FishingPlanner.Interfaces
{
    public interface IFishingEventService
    {
        Task AddEventAsync(FishingEvent fishingEvent);
        Task DeleteEventAsync(Guid id);
        Task<List<FishingEvent>> GetAllFishingEventsAsync();
        Task<FishingEvent?> GetEventByIdAsync(Guid id);
        Task<List<FishingEvent>> GetEventsForMonthAsync(DateTime month);
        Task UpdateEventAsync(FishingEvent fishingEvent);
    }
}