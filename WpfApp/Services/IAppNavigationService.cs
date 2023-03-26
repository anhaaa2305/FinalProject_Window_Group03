using System.Windows.Controls;
using Wpf.Ui.Contracts;
using WpfApp.Middlewares;

namespace WpfApp.Services;

public interface IAppNavigationService : INavigationService
{
	bool Navigate<T>();
	void AddMiddleware(INavigationMiddleware middleware);
}
