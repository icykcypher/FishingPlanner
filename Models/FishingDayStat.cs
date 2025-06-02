namespace FishingPlanner.Models
{
    public class FishingDayStat
    {
        public DateTime Date { get; set; }
        public bool IsFishActive { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}