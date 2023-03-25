using System.ComponentModel;
using System.Windows.Controls;
using Wpf.Ui.Services;
using WpfApp.Middlewares;

namespace WpfApp.Services;

public class AppNavigationService : NavigationService, IAppNavigationService
{
	private readonly LinkedList<INavigationMiddleware> middlewares = new();

	public event Action<UserControl>? Navigated;

	public AppNavigationService(IServiceProvider provider) : base(provider) { }

	public bool Navigate<T>() where T : UserControl
	{
		if (middlewares.Count != 0)
		{
			var arg = new CancelEventArgs();
			foreach (var m in middlewares)
			{
				m.OnNavigating(typeof(T), arg);
				if (arg.Cancel)
				{
					return false;
				}
			}
		}
		return Navigate(typeof(T));
	}

	public void AddMiddleware(INavigationMiddleware middleware)
	{
		middlewares.AddLast(middleware);
	}
}
