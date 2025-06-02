using FishingPlanner.Models;

namespace FishingPlanner.ViewModels
{
    public class DayDetailViewModel
    {
        public CalendarDay SelectedDay { get; }

        public DayDetailViewModel(CalendarDay selectedDay)
        {
            SelectedDay = selectedDay;
        }
    }
}