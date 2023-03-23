using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Middlewares;

namespace WpfApp.Services;

public class NavigationService : INavigationService
{
	private readonly IServiceProvider provider;
	private readonly LinkedList<INavigationMiddleware> middlewares = new();

	public event Action<UserControl>? Navigated;

	public NavigationService(IServiceProvider provider)
	{
		this.provider = provider;
	}

	public bool Navigate<T>() where T : UserControl
	{
		var service = provider.GetService<T>();
		if (service is null)
		{
			return false;
		}

		if (middlewares.Count != 0)
		{
			var arg = new CancelEventArgs();
			foreach (var m in middlewares)
			{
				m.OnNavigating(service, arg);
				if (arg.Cancel)
				{
					return false;
				}
			}
		}

		Navigated?.Invoke(service);
		return true;
	}

	public void AddMiddleware(INavigationMiddleware middleware)
	{
		middlewares.AddLast(middleware);
	}
}
