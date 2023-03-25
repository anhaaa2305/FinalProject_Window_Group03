using System.Windows.Controls;
using Wpf.Ui.Contracts;
using WpfApp.Middlewares;

namespace WpfApp.Services;

public interface IAppNavigationService : INavigationService
{
	event Action<UserControl>? Navigated;
	bool Navigate<T>() where T : UserControl;
	void AddMiddleware(INavigationMiddleware middleware);
}
