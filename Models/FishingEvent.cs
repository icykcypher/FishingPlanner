using System.ComponentModel.DataAnnotations;

namespace FishingPlanner.Models
{
    public class FishingEvent
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
    }
}