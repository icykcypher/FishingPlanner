using CommunityToolkit.Mvvm.Input;
using FishingPlanner.Models;
using FishingPlanner.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows.Input;

namespace FishingPlanner.Views
{
    public partial class CalendarView : UserControl
    {
        public ICommand DaySelectedCommand { get; }

        public CalendarView(CalendarViewModel calendarViewModel)
        {
            InitializeComponent();

            var vm = calendarViewModel;
            this.DataContext = vm;

            DaySelectedCommand = new RelayCommand<CalendarDay>(OnDaySelected);
        }

        private void OnDaySelected(CalendarDay? selectedDay)
        {
            if (selectedDay == null) return;

            var detailViewModel = new DayDetailViewModel(selectedDay);
            var detailView = new DayDetailView { DataContext = detailViewModel };
        }
    }
}