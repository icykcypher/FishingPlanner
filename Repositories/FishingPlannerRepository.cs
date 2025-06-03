using FishingPlanner.Data;
using FishingPlanner.Models;
using FishingPlanner.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FishingPlanner.Repositories
{
    public class FishingEventRepository : IFishingEventRepository
    {
        private readonly FishingPlannerDbContext _context;

        public FishingEventRepository(FishingPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<FishingEvent>> GetAllAsync()
        {
            var list = await _context.FishingEvents.ToListAsync();

            return list;
        }

        public async Task<List<FishingEvent>> GetForMonthAsync(DateTime month)
        {
            var start = new DateOnly(month.Year, month.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            return await _context.FishingEvents
                .Where(e => e.Date >= start && e.Date <= end)
                .ToListAsync();
        }

        public async Task<FishingEvent?> GetByIdAsync(Guid id)
        {
            return await _context.FishingEvents.FindAsync(id);
        }

        public async Task AddAsync(FishingEvent fishingEvent)
        {
            _context.FishingEvents.Add(fishingEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FishingEvent>> GetEventsInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);

            return await _context.FishingEvents
                .Where(e => e.Date >= start && e.Date <= end)
                .ToListAsync();
        }

        public async Task UpdateAsync(FishingEvent fishingEvent)
        {
            _context.FishingEvents.Update(fishingEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.FishingEvents.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}