using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data;
using WpfApp.Data.Context;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class RentedVehicleViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly ISessionService sessionService;
	private ObservableCollection<RentedVehicle>? vehicles;
	private ViewState state;

	public IRelayCommand<RentedVehicle> ViewItemDetailsCommand { get; }

	public ObservableCollection<RentedVehicle>? Vehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public RentedVehicleViewModel(IVehicleDAO vehicleDAO, ISessionService sessionService)
	{
		this.vehicleDAO = vehicleDAO;
		this.sessionService = sessionService;
		ViewItemDetailsCommand = new RelayCommand<RentedVehicle>(ViewItemDetails);

		if (sessionService.User is null)
		{
			App.Current.Services.GetRequiredService<IAppNavigationService>().Navigate<LoginView>();
			return;
		}
		GetRentedVehiclesAsync().SafeFireAndForget();
	}

	private async Task GetRentedVehiclesAsync()
	{
		State = ViewState.Busy;

		var vehicles = await vehicleDAO
			.GetRentedByUserIdAsync(sessionService.User!.Id)
			.ConfigureAwait(false);
		foreach (var vehicle in vehicles)
		{
			if (string.IsNullOrEmpty(vehicle.Vehicle.ImageUrl))
			{
				vehicle.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			Vehicles = new ObservableCollection<RentedVehicle>(vehicles);
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

	private void ViewItemDetails(RentedVehicle? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
