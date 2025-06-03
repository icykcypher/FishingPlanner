using System.Windows;
using FishingPlanner.Models;

namespace FishingPlanner.Views
{
    public partial class SaveFishingWindow : Window
    {
        public FishingEvent FishingEvent { get; private set; }

        public SaveFishingWindow(string location, DateOnly selectedDate)
        {
            InitializeComponent();

            LocationTextBox.Text = location;

            FishingEvent = new FishingEvent
            {
                Id = Guid.NewGuid(),
                Date = selectedDate,
                Location = location
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FishingEvent.Title = TitleTextBox.Text;
            FishingEvent.Note = NoteTextBox.Text;
            FishingEvent.Tag = TagTextBox.Text;

            if (string.IsNullOrWhiteSpace(FishingEvent.Title))
            {
                MessageBox.Show("Enter the title", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}