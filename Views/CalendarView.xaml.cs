using FishingPlanner.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace FishingPlanner.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public CalendarView()
        {
            InitializeComponent();

            var vm = App.Services.GetRequiredService<CalendarViewModel>();
            this.DataContext = vm;
        }
    }
}