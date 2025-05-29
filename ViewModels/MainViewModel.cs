using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FishingPlanner.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            ShowCalendar();
        }

        [ObservableProperty]
        private UserControl currentView = null!;

        [RelayCommand]
        private void ShowCalendar() => CurrentView = new Views.CalendarView();

        [RelayCommand]
        private void ShowAddEvent() => CurrentView = new Views.AddEventView();

        [RelayCommand]
        private void ShowTips() => CurrentView = new Views.TipsView();
    }
}