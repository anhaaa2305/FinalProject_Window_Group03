using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.Views.User;

namespace WpfApp1.ViewModels;

public class HomeViewModel : ObservableObject
{
	private readonly IServiceScope scope;
	private readonly INavigationService navigator;
	private UserControl? currentView;
	public IRelayCommand AvailableVehicleClickCommand { get; }
	public IRelayCommand VehicleRentingClickCommand { get; }
	public IRelayCommand RentHistoryClickCommand { get; }

	public UserControl? CurrentView
	{
		get => currentView; set
		{
			var changed = SetProperty(ref currentView, value);
			if (changed)
			{
				AvailableVehicleClickCommand.NotifyCanExecuteChanged();
				VehicleRentingClickCommand.NotifyCanExecuteChanged();
				RentHistoryClickCommand.NotifyCanExecuteChanged();
			}
		}
	}

	public HomeViewModel(IServiceScopeFactory scopeFactory)
	{
		scope = scopeFactory.CreateAsyncScope();
		AvailableVehicleClickCommand = new RelayCommand(ShowAvailableVehicleView, CanShowAvailableVehicleView);
		VehicleRentingClickCommand = new RelayCommand(ShowVehicleRentingView, CanShowVehicleRentingView);
		RentHistoryClickCommand = new RelayCommand(ShowRentHistoryView, CanShowRentHistoryView);
		navigator = scope.ServiceProvider.GetRequiredService<INavigationService>();
		navigator.Navigated += OnNavigated;
		navigator.Navigate<AvailableVehicleView>();
	}

	private void OnNavigated(UserControl control)
	{
		CurrentView = control;
	}

	private void ShowAvailableVehicleView()
	{
		navigator.Navigate<AvailableVehicleView>();
	}

	private bool CanShowAvailableVehicleView()
	{
		return CurrentView is not AvailableVehicleView;
	}

	private void ShowVehicleRentingView()
	{
		navigator.Navigate<RentedVehicleView>();
	}

	private bool CanShowVehicleRentingView()
	{
		return CurrentView is not RentedVehicleView;
	}

	private void ShowRentHistoryView()
	{
		navigator.Navigate<RentingLogView>();
	}

	private bool CanShowRentHistoryView()
	{
		return CurrentView is not RentingLogView;
	}
}
