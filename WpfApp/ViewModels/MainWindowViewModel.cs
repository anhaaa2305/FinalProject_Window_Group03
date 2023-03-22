using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Services;
using WpfApp.Views;
using WpfApp.Views.User;

namespace WpfApp.ViewModels;

public class MainWindowViewModel : ObservableObject
{
	private UserControl? currentView = null;

	public UserControl? CurrentView
	{
		get => currentView;
		set => SetProperty(ref currentView, value);
	}

	public MainWindowViewModel(INavigationService navigationService)
	{
		navigationService.Navigated += OnNavigated;
		navigationService.Navigate<LoginView>();
	}

	private void OnNavigated(UserControl newView)
	{
		CurrentView = newView;
	}
}
