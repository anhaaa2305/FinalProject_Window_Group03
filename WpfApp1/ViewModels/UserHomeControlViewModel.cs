using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.Views.UserControls;

namespace WpfApp1.ViewModels;

public class UserHomeControlViewModel : ObservableObject
{
	private readonly IServiceScope scope;
	private readonly INavigationService navigator;
	private UserControl? currentView;
	public IRelayCommand VehicleStoreClickCommand { get; }
	public IRelayCommand VehicleRentingClickCommand { get; }
	public IRelayCommand RentHistoryClickCommand { get; }

	public UserControl? CurrentView
	{
		get => currentView; set
		{
			var changed = SetProperty(ref currentView, value);
			if (changed)
			{
				VehicleStoreClickCommand.NotifyCanExecuteChanged();
				VehicleRentingClickCommand.NotifyCanExecuteChanged();
				RentHistoryClickCommand.NotifyCanExecuteChanged();
			}
		}
	}

	public UserHomeControlViewModel(IServiceScopeFactory scopeFactory)
	{
		scope = scopeFactory.CreateAsyncScope();
		VehicleStoreClickCommand = new RelayCommand(ShowVehicleStoreView, CanShowVehicleStoreView);
		VehicleRentingClickCommand = new RelayCommand(ShowVehicleRentingView, CanShowVehicleRentingView);
		RentHistoryClickCommand = new RelayCommand(ShowRentHistoryView, CanShowRentHistoryView);
		navigator = scope.ServiceProvider.GetRequiredService<INavigationService>();
		navigator.Navigated += OnNavigated;
		navigator.Navigate<UserVehicleStoreControl>();
	}

	private void OnNavigated(UserControl control)
	{
		CurrentView = control;
	}

	private void ShowVehicleStoreView()
	{
		navigator.Navigate<UserVehicleStoreControl>();
	}

	private bool CanShowVehicleStoreView()
	{
		return CurrentView is not UserVehicleStoreControl;
	}

	private void ShowVehicleRentingView()
	{
		navigator.Navigate<UserVehicleRentingControl>();
	}

	private bool CanShowVehicleRentingView()
	{
		return CurrentView is not UserVehicleRentingControl;
	}

	private void ShowRentHistoryView()
	{
		navigator.Navigate<UserRentHistoryControl>();
	}

	private bool CanShowRentHistoryView()
	{
		return CurrentView is not UserRentHistoryControl;
	}
}
