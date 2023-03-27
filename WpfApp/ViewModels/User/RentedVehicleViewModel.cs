using System.Windows;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class RentedVehicleViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly ISessionService sessionService;
	private IReadOnlyCollection<object>? vehicles;
	private ViewState state;

	public IRelayCommand<object> ViewItemDetailsCommand { get; }

	public IReadOnlyCollection<object>? Vehicles
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
		ViewItemDetailsCommand = new RelayCommand<object>(ViewItemDetails);

		if (sessionService.User is null)
		{
			App.Current.Services.GetRequiredService<IAppNavigationService>().Navigate<LoginView>();
			return;
		}
		GetVehiclesAsync().SafeFireAndForget();
	}

	private async Task GetVehiclesAsync()
	{
		State = ViewState.Busy;

		var rentedVehiclesTask = vehicleDAO.GetRentedByUserIdAsync(sessionService.User!.Id);
		var reservedVehiclesTask = vehicleDAO.GetReservedByUserIdAsync(sessionService.User!.Id);
		await Task.WhenAll(rentedVehiclesTask, reservedVehiclesTask).ConfigureAwait(false);
		var vehicles = (await rentedVehiclesTask.ConfigureAwait(false)).Cast<object>().Concat(await reservedVehiclesTask.ConfigureAwait(false)).ToArray();
		foreach (var vehicle in vehicles)
		{
			if (vehicle is RentedVehicle rented && string.IsNullOrEmpty(rented.Vehicle.ImageUrl))
			{
				rented.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
			else if (vehicle is ReservedVehicle reserved && string.IsNullOrEmpty(reserved.Vehicle.ImageUrl))
			{
				reserved.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			Vehicles = vehicles;
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

	private void ViewItemDetails(object? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
