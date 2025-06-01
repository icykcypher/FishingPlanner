namespace FishingPlanner.Models
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public List<string> Events { get; set; } = [];
        public string? FishingForecast { get; set; } = string.Empty; 
    }
}