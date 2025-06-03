using CommunityToolkit.Mvvm.Input;
using FishingPlanner.Interfaces;
using FishingPlanner.Models;
using FishingPlanner.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class CalendarViewModel : INotifyPropertyChanged
{
    private readonly IFishingDataProvider _fishingService;
    private readonly IFishingEventRepository repository;

    public ObservableCollection<CalendarDay> Days { get; } = [];

    private DateTime _currentMonth;
    public DateTime CurrentMonth
    {
        get => _currentMonth;
        set
        {
            if (_currentMonth != value)
            {
                _currentMonth = value;
                OnPropertyChanged();
                LoadCalendarDays();
            }
        }
    }

    public ICommand PreviousMonthCommand { get; }
    public ICommand NextMonthCommand { get; }

    public IRelayCommand<CalendarDay> DaySelectedCommand { get; }

    public event Action<CalendarDay>? DaySelected;
    public event Action? ReturnToCalendar;
    public CalendarViewModel(IFishingDataProvider fishingService, IFishingEventRepository repository)
    {
        _fishingService = fishingService;
        this.repository = repository;
        _currentMonth = DateTime.Today;

        PreviousMonthCommand = new RelayCommand(() => CurrentMonth = CurrentMonth.AddMonths(-1));
        NextMonthCommand = new RelayCommand(() => CurrentMonth = CurrentMonth.AddMonths(1));

        DaySelectedCommand = new RelayCommand<CalendarDay>(OnDaySelected);

        LoadCalendarDays();
    }

    private void OnDaySelected(CalendarDay? selectedDay)
    {
        if (selectedDay == null) return;

        DaySelected?.Invoke(selectedDay);
    }

    private void OnReturnToCalendar()
    {
        ReturnToCalendar?.Invoke();
    }

    private async Task LoadCalendarDays()
    {
        Days.Clear();

        var firstDayOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        var daysOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
        var startDate = firstDayOfMonth.AddDays(-daysOffset);

        var endDate = startDate.AddDays(41);
        var fishingEvents = await repository.GetEventsInRangeAsync(startDate, endDate);

        for (int i = 0; i < 42; i++)
        {
            var day = startDate.AddDays(i);

            var calendarDay = new CalendarDay
            {
                Date = day,
                IsCurrentMonth = day.Month == CurrentMonth.Month,
                Events = fishingEvents
                    .Where(e => e.Date.ToDateTime(new TimeOnly(0, 0)) == day.Date)
                    .ToList()
            };

            Days.Add(calendarDay);
        }

        double lat = 49.748;
        double lon = 13.377;

        var tasks = Days.Select(async day =>
        {
            var stat = await _fishingService.GetFishingStatAsync(day.Date, lat, lon);
            day.IsFishActive = stat.IsFishActive;
        });

        await Task.WhenAll(tasks);

        OnPropertyChanged(nameof(Days));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}