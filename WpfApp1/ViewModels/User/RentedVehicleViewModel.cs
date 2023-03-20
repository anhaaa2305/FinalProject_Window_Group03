using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.DAOs;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.ViewModels;

public class RentedVehicleViewModel : ObservableObject
{
	private ObservableCollection<VehicleModel>? vehicles;
	private ViewState state;

	public IRelayCommand<VehicleModel> ViewItemDetailsCommand { get; }

	public ObservableCollection<VehicleModel>? Vehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	private readonly IVehicleDAO vehicleDAO;
	public RentedVehicleViewModel(IVehicleDAO vehicleDAO)
	{
		this.vehicleDAO = vehicleDAO;
		ViewItemDetailsCommand = new RelayCommand<VehicleModel>(ViewItemDetails);

		FetchVehiclesAsync().SafeFireAndForget();
	}

	private async Task FetchVehiclesAsync()
	{
		State = ViewState.Busy;
		var vehicles = await vehicleDAO.GetAllRentedAsync().ConfigureAwait(false);
		foreach (var vehicle in vehicles)
		{
			if (string.IsNullOrEmpty(vehicle.ImageUrl))
			{
				vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			Vehicles = new ObservableCollection<VehicleModel>(vehicles);
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

	private void ViewItemDetails(VehicleModel? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
