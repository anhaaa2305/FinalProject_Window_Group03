using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;

namespace WpfApp.ViewModels;

public class AvailableVehicleViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private ObservableCollection<Vehicle>? vehicles;
	private ViewState state;

	public IRelayCommand<Vehicle> ReserveCommand { get; }
	public IRelayCommand<Vehicle> ViewItemDetailsCommand { get; }

	public ObservableCollection<Vehicle>? Vehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public AvailableVehicleViewModel(IVehicleDAO vehicleDAO)
	{
		this.vehicleDAO = vehicleDAO;
		ReserveCommand = new RelayCommand<Vehicle>(Reserve);
		ViewItemDetailsCommand = new RelayCommand<Vehicle>(ViewItemDetails);

		GetAvailableVehiclesAsync().SafeFireAndForget();
	}

	private async Task GetAvailableVehiclesAsync()
	{
		State = ViewState.Busy;

		var vehicles = await vehicleDAO
			.GetAvailableVehicles()
			.ConfigureAwait(false);
		foreach (var vehicle in vehicles)
		{
			if (string.IsNullOrEmpty(vehicle.ImageUrl))
			{
				vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			Vehicles = new ObservableCollection<Vehicle>(vehicles);
			if (Vehicles.Count == 0)
			{
				State = ViewState.Empty;
			}
			else
			{
				State = ViewState.Present;
			}
		});
	}

	private void Reserve(Vehicle? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}

	private void ViewItemDetails(Vehicle? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
