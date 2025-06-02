using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace FishingPlanner.Services
{
    public class FishActivityToBorderBrushConverter : IValueConverter
    {
        public Brush ActiveBrush { get; set; } = Brushes.Green;
        public Brush InactiveBrush { get; set; } = Brushes.Transparent;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive && isActive) return ActiveBrush;

            return InactiveBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}