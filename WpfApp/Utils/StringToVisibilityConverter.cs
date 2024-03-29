using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp.Utils;

[ValueConversion(typeof(string), typeof(Visibility))]
public class StringToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null || string.IsNullOrEmpty((string)value))
		{
			return Visibility.Collapsed;
		}
		else
		{
			return Visibility.Visible;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
