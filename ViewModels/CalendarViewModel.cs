using System;
using FishingPlanner.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FishingPlanner.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _currentMonth;

        public ObservableCollection<CalendarDay> Days { get; } = [];

        public CalendarViewModel()
        {
            CurrentMonth = DateTime.Today;
            GenerateCalendar(CurrentMonth);
        }

        [RelayCommand]
        private void NextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            GenerateCalendar(CurrentMonth);
        }

        [RelayCommand]
        private void PreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            GenerateCalendar(CurrentMonth);
        }

        private void GenerateCalendar(DateTime month)
        {
            Days.Clear();

            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var daysBefore = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            var startDate = firstDayOfMonth.AddDays(-daysBefore);
            for (var i = 0; i < 42; i++)
            {
                var day = startDate.AddDays(i);

                Days.Add(new()
                {
                    Date = day,
                    IsCurrentMonth = day.Month == month.Month
                });
            }
        }
    }
}
