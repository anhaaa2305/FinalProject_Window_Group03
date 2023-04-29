using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WpfApp.Data.Models;

namespace WpfApp.Utils;

[ValueConversion(typeof(string), typeof(VehicleCategory))]
public class VehicleCategoryConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var type = typeof(VehicleCategory);
		var dict = new ResourceDictionary()
		{
			Source = new Uri("pack://application:,,,/Resources/Dictionaries/VehicleCategoryStrings.xaml")
		};
		var name = Enum.GetName(type, value);
		if (name is null)
		{
			return value;
		}
		return (string?)dict[name] ?? value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var type = typeof(VehicleCategory);
		var dict = new ResourceDictionary()
		{
			Source = new Uri("pack://application:,,,/Resources/Dictionaries/VehicleCategoryStrings.xaml")
		};
		foreach (var i in dict.Keys)
		{
			if (i is not string key)
			{
				continue;
			}
			if ((string?)dict[key] == (string)value)
			{
				if (!Enum.TryParse(type, key, out var result))
				{
					return VehicleCategory.None;
				}
				return result;
			}
		}
		return VehicleCategory.None;
	}
}
