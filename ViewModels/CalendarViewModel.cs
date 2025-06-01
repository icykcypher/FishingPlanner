using FishingPlanner.Models;
using FishingPlanner.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace FishingPlanner.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        private readonly FishingForecastService _forecastService;
        private readonly double _latitude = 55.751244;
        private readonly double _longitude = 37.618423;

        [ObservableProperty]
        private DateTime _currentMonth;

        public ObservableCollection<CalendarDay> Days { get; } = [];

        public CalendarViewModel(IServiceProvider serviceProvider)
        {
            _forecastService = serviceProvider.GetRequiredService<FishingForecastService>()
                ?? throw new InvalidOperationException("FishingForecastService is not registered.");

            CurrentMonth = DateTime.Today;
            _ = GenerateCalendarAsync(CurrentMonth);
        }

        [RelayCommand]
        private async Task NextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            await GenerateCalendarAsync(CurrentMonth);
        }

        [RelayCommand]
        private async Task PreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            await GenerateCalendarAsync(CurrentMonth);
        }

        private async Task GenerateCalendarAsync(DateTime month)
        {
            Days.Clear();

            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var daysBefore = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            var startDate = firstDayOfMonth.AddDays(-daysBefore);

            for (var i = 0; i < 42; i++)
            {
                var day = startDate.AddDays(i);

                var forecast = await _forecastService.GetFishingForecastAsync(day, _latitude, _longitude);

                Days.Add(new CalendarDay
                {
                    Date = day,
                    IsCurrentMonth = day.Month == month.Month,
                    FishingForecast = forecast
                });
            }
        }
    }
}
