using System.ComponentModel;

namespace FishingPlanner.Models
{
    public class CalendarDay : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public List<FishingEvent> Events { get; set; } = [];

        private bool _isFishActive;
        public bool IsFishActive
        {
            get => _isFishActive;
            set
            {
                _isFishActive = value;
                OnPropertyChanged(nameof(IsFishActive));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}