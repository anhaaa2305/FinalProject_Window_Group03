using System.Windows;
using System.Windows.Controls;
using WpfApp.Data.Models;

namespace WpfApp.Utils;

public class UserRentedVehicleDataTemplateSelector : DataTemplateSelector
{
	public DataTemplate Reserved { get; set; } = default!;
	public DataTemplate Rented { get; set; } = default!;

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is ReservedVehicle)
		{
			return Reserved;
		}
		else
		{
			return Rented;
		}
	}
}
