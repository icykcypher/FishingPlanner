using FishingPlanner.Models;

namespace FishingPlanner.Interfaces
{
    public interface IFishingEventRepository
    {
        Task AddAsync(FishingEvent fishingEvent);
        Task DeleteAsync(Guid id);
        Task<List<FishingEvent>> GetAllAsync();
        Task<FishingEvent?> GetByIdAsync(Guid id);
        Task<List<FishingEvent>> GetForMonthAsync(DateTime month);
        Task UpdateAsync(FishingEvent fishingEvent);
    }
}