using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class MainWindowViewModel : ObservableObject
{
	private UserControl? currentView = null;

	public UserControl? CurrentView
	{
		get => currentView;
		set => SetProperty(ref currentView, value);
	}

	public MainWindowViewModel(IAppNavigationService navigationService)
	{
		navigationService.Navigated += OnNavigated;
		navigationService.Navigate<LoginView>();
	}

	private void OnNavigated(UserControl newView)
	{
		CurrentView = newView;
	}
}
