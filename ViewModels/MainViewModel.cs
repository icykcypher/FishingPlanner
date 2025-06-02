using FishingPlanner.Views;
using FishingPlanner.Models;
using FishingPlanner.Interfaces;
using FishingPlanner.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

public class MainViewModel : ObservableObject
{
    private object _currentView = null!;
    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public MainViewModel(IFishingDataProvider fishingService)
    {
        var calendarVM = new CalendarViewModel(fishingService);
        calendarVM.DaySelected += OnDaySelected;

        CurrentView = new CalendarView (calendarVM) { DataContext = calendarVM };
    }

    private void OnDaySelected(CalendarDay selectedDay)
    {
        var detailVM = new DayDetailViewModel(selectedDay);
        var detailView = new DayDetailView { DataContext = detailVM };

        CurrentView = detailView;  // переключаем вью
    }
}