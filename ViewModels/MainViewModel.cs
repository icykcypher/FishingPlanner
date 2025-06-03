using CommunityToolkit.Mvvm.ComponentModel;
using FishingPlanner;
using FishingPlanner.Interfaces;
using FishingPlanner.Models;
using FishingPlanner.Repositories;
using FishingPlanner.ViewModels;
using FishingPlanner.Views;
using Microsoft.Extensions.DependencyInjection;

public class MainViewModel : ObservableObject
{
    private readonly IServiceProvider serviceProvider;

    private object _currentView = null!;
    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public MainViewModel(IFishingDataProvider fishingService, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        var repo = serviceProvider.GetRequiredService<IFishingEventRepository>();
        var calendarVM = new CalendarViewModel(fishingService, repo);
        calendarVM.DaySelected += OnDaySelected;

        CurrentView = new CalendarView(calendarVM, serviceProvider) { DataContext = calendarVM };
        App.CalendarView = CurrentView;
    }

    private void OnDaySelected(CalendarDay selectedDay)
    {
        var repository = serviceProvider.GetRequiredService<IFishingEventRepository>();
        var detailVM = new DayDetailViewModel(selectedDay);
        var detailView = new DayDetailView(repository, detailVM) { DataContext = detailVM };

        CurrentView = detailView;
    }
}