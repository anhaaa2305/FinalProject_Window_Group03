using System.Windows.Controls;

namespace WpfApp.Services;

public interface INavigationService
{
	event Action<UserControl>? Navigated;
	bool Navigate<T>() where T : UserControl;
}
