using System.Windows.Controls;
using FishingPlanner.Interfaces;

namespace FishingPlanner.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ContentControl _mainContent;

        public NavigationService(ContentControl mainContent)
        {
            _mainContent = mainContent;
        }

        public void Navigate(UserControl view)
        {
            _mainContent.Content = view;
        }
    }
}