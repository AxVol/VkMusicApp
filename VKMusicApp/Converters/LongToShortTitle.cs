using System.Globalization;

namespace VKMusicApp.Converters
{
    public class LongToShortTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string title = value as string;
            int maxLength = 30;

            if (title.Length > maxLength)
            {
                string temp = title.Remove(maxLength);
                title = $"{temp}...";
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
