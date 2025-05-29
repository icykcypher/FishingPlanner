using FishingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FishingPlanner.Data.Configurations
{
    public class FishingEventConfiguration : IEntityTypeConfiguration<FishingEvent>
    {
        public void Configure(EntityTypeBuilder<FishingEvent> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(e => e.Location)
                .HasMaxLength(80);

            builder.Property(e => e.Note)
                .HasMaxLength(200);

            builder.Property(e => e.Tag)
                .HasMaxLength(20);
        }
    }
}