using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FishingPlanner.Services
{
    public class DateAndBoolToBrushConverter : IMultiValueConverter
    {
        public Brush CurrentMonthBrush { get; set; } = Brushes.White;
        public Brush OtherMonthBrush { get; set; } = Brushes.LightGray;
        public Brush TodayBrush { get; set; } = Brushes.LightBlue;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return OtherMonthBrush;

            if (values[0] is DateTime date)
            {
                if (date.Date == DateTime.Today)
                    return TodayBrush;
            }

            if (values[1] is bool isCurrent)
            {
                return isCurrent ? CurrentMonthBrush : OtherMonthBrush;
            }

            return OtherMonthBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}