using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace FishingPlanner.Services
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush CurrentMonthBrush { get; set; } = Brushes.White;
        public Brush OtherMonthBrush { get; set; } = Brushes.LightGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isCurrent = (bool)value;
            return isCurrent ? CurrentMonthBrush : OtherMonthBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}