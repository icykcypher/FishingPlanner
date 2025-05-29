using FishingPlanner.Data.Configurations;
using FishingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FishingPlanner.Data
{
    public class FishingPlannerDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<FishingEvent> FishingEvents { get; set; }
        public FishingPlannerDbContext(IConfiguration configuration, DbContextOptions<FishingPlannerDbContext> options) : base(options)
        {
            this._configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FishingEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
            });
        }
    }
}