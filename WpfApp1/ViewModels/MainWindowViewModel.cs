using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp1.Services;
using WpfApp1.Views;
using WpfApp1.Views.User;

namespace WpfApp1.ViewModels;

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
