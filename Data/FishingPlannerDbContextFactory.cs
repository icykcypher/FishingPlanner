using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace FishingPlanner.Data
{
    public class FishingPlannerDbContextFactory : IDesignTimeDbContextFactory<FishingPlannerDbContext>
    {
        public FishingPlannerDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FishingPlannerDbContext>();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new FishingPlannerDbContext(configuration, optionsBuilder.Options);
        }
    }
}