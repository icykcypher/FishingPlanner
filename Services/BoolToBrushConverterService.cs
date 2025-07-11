﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FishingPlanner.Services
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush CurrentMonthBrush { get; set; } = Brushes.White;
        public Brush OtherMonthBrush { get; set; } = Brushes.LightGray;
        public Brush TodayBrush { get; set; } = Brushes.LightBlue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date.Date == DateTime.Today)
                    return TodayBrush;
            }

            if (parameter is bool isCurrent)
            {
                return isCurrent ? CurrentMonthBrush : OtherMonthBrush;
            }

            return OtherMonthBrush;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}