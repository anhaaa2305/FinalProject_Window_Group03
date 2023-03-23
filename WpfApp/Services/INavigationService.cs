using System.ComponentModel;
using System.Windows.Controls;
using WpfApp.Middlewares;

namespace WpfApp.Services;

public interface INavigationService
{
	event Action<UserControl>? Navigated;
	bool Navigate<T>() where T : UserControl;
	void AddMiddleware(INavigationMiddleware middleware);
}
