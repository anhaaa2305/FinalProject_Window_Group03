using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.DAOs;
using WpfApp1.Services;
using WpfApp1.Views.UserControls;

namespace WpfApp1.ViewModels;

public class UserHomeControlViewModel : ObservableObject
{
	private UserControl currentView;
	public IRelayCommand VehicleStoreClickCommand { get; }
	public IRelayCommand VehicleRentingClickCommand { get; }
	public IRelayCommand RentHistoryClickCommand { get; }

	public UserControl CurrentView
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

	private readonly UserVehicleStoreControl vehicleStoreControl;
	private readonly UserVehicleRentingControl vehicleRentingControl;
	private readonly UserRentHistoryControl rentHistoryControl;
	public UserHomeControlViewModel(UserVehicleStoreControl vehicleStoreControl, UserVehicleRentingControl vehicleRentingControl, UserRentHistoryControl rentHistoryControl, IUserDAO userDAO)
	{
		this.vehicleStoreControl = vehicleStoreControl;
		this.vehicleRentingControl = vehicleRentingControl;
		this.rentHistoryControl = rentHistoryControl;
		Task.Run(async () =>
		{
			await userDAO.EnsureTableAsync();
		});
		currentView = vehicleStoreControl;
		VehicleStoreClickCommand = new RelayCommand(ShowVehicleStoreView, CanShowVehicleStoreView);
		VehicleRentingClickCommand = new RelayCommand(ShowVehicleRentingView, CanShowVehicleRentingView);
		RentHistoryClickCommand = new RelayCommand(ShowRentHistoryView, CanShowRentHistoryView);
	}

	private void ShowVehicleStoreView()
	{
		CurrentView = vehicleStoreControl;
	}

	private bool CanShowVehicleStoreView()
	{
		return CurrentView != vehicleStoreControl;
	}

	private void ShowVehicleRentingView()
	{
		CurrentView = vehicleRentingControl;
	}

	private bool CanShowVehicleRentingView()
	{
		return CurrentView != vehicleRentingControl;
	}

	private void ShowRentHistoryView()
	{
		CurrentView = rentHistoryControl;
	}

	private bool CanShowRentHistoryView()
	{
		return CurrentView != rentHistoryControl;
	}
}
