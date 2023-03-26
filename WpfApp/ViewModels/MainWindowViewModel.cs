using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class MainWindowViewModel : ObservableObject
{
	public MainWindowViewModel(IAppNavigationService navigator)
	{
		navigator.Navigate<LoginView>();
	}
}
