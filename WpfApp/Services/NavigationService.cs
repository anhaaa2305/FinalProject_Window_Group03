using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp.Services;

public class NavigationService : INavigationService
{
	private readonly IServiceProvider provider;

	public NavigationService(IServiceProvider provider)
	{
		this.provider = provider;
	}

	public event Action<UserControl>? Navigated;
	public bool Navigate<T>() where T : UserControl
	{
		var service = provider.GetService<T>();
		if (service is null)
		{
			return false;
		}

		Navigated?.Invoke(service);
		return true;
	}
}
