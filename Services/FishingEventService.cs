using FishingPlanner.Models;
using FishingPlanner.Interfaces;

namespace FishingPlanner.Services
{
    public class FishingEventService : IFishingEventService
    {
        private readonly IFishingEventRepository _repository;

        public FishingEventService(IFishingEventRepository repository)
        {
            _repository = repository;
        }

        public Task<List<FishingEvent>> GetAllFishingEventsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<List<FishingEvent>> GetEventsForMonthAsync(DateTime month)
        {
            return _repository.GetForMonthAsync(month);
        }

        public Task<FishingEvent?> GetEventByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddEventAsync(FishingEvent fishingEvent)
        {
            return _repository.AddAsync(fishingEvent);
        }

        public Task UpdateEventAsync(FishingEvent fishingEvent)
        {
            return _repository.UpdateAsync(fishingEvent);
        }

        public Task DeleteEventAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}