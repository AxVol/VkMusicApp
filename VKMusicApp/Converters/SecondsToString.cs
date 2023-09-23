using System.Globalization;

namespace VKMusicApp.Converters
{
    public class SecondsToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string time = string.Empty; //format 2:43
            TimeSpan timeSpan = new TimeSpan();

            if (value is double doubleValue)
            {
                timeSpan = TimeSpan.FromSeconds(doubleValue);
            }
            else if (value is TimeSpan timeSpanValue)
            {
                timeSpan = timeSpanValue;
            }
            else if (value is int intValue)
            {
                timeSpan = TimeSpan.FromSeconds(intValue);
            }

            if (timeSpan.Seconds < 10)
                time = $"{timeSpan.Minutes}:0{timeSpan.Seconds}";
            else
            {
                time = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
            }

            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
