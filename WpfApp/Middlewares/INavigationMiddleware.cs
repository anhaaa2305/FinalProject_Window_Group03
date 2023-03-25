using System.ComponentModel;

namespace WpfApp.Middlewares;

public interface INavigationMiddleware
{
	void OnNavigating(Type viewType, CancelEventArgs e);
}
