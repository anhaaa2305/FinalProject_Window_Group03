using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1.Services;

public class NavigationService : INavigationService
{
	public event Action<UserControl>? Navigated;
	public bool Navigate<T>() where T : UserControl
	{
		var service = App.Current.Services.GetService<T>();
		if (service is null)
		{
			return false;
		}

		Navigated?.Invoke(service);
		return true;
	}
}
