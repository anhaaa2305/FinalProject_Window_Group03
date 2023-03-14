using System.Windows.Controls;

namespace WpfApp1.Services;

public interface INavigationService
{
	event Action<UserControl>? Navigated;
	bool Navigate<T>() where T : UserControl;
}
