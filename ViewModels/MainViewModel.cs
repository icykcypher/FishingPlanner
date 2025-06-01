using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FishingPlanner.Views;
using System.Windows.Controls;

namespace FishingPlanner.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel(CalendarView calendarView, AddEventView addEventView, TipsView tipsView)
        {
            _calendarView = calendarView;
            _addEventView = addEventView;
            _tipsView = tipsView;
            ShowCalendar();
        }


        [ObservableProperty]
        private UserControl currentView = null!;
        private readonly CalendarView _calendarView;
        private readonly AddEventView _addEventView;
        private readonly TipsView _tipsView;

        [RelayCommand]
        private void ShowCalendar() => CurrentView = new Views.CalendarView();

        [RelayCommand]
        private void ShowAddEvent() => CurrentView = new Views.AddEventView();

        [RelayCommand]
        private void ShowTips() => CurrentView = new Views.TipsView();
    }
}