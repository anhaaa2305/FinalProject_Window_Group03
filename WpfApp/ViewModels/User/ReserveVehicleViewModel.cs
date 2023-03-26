using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;
using WpfApp.Services;

namespace WpfApp.ViewModels;

public class ReserveVehicleViewModel : ObservableObject
{
	private readonly ReserveVehicleModel model;
	private readonly IVehicleDAO vehicleDAO;
	private readonly IAppNavigationService navigator;
	private ViewState state;
	private Vehicle? vehicle;

	public IRelayCommand ReserveCommand { get; }
	public IRelayCommand BackCommand { get; }

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public Vehicle? Vehicle
	{
		get => vehicle;
		set => SetProperty(ref vehicle, value);
	}

	public ReserveVehicleViewModel(ReserveVehicleModel model, IVehicleDAO vehicleDAO, IAppNavigationService navigator)
	{
		this.model = model;
		this.vehicleDAO = vehicleDAO;
		this.navigator = navigator;
		ReserveCommand = new RelayCommand(Reserve);
		BackCommand = new RelayCommand(Back);
		GetVehicleAsync().SafeFireAndForget();
	}

	private async Task GetVehicleAsync()
	{
		State = ViewState.Busy;
		await Task.Delay(500);

		var vehicle = await vehicleDAO
			.GetByIdAsync(model.Vehicle.Id)
			.ConfigureAwait(false);
		App.Current.Dispatcher.Invoke(() =>
		{
			if (vehicle is null)
			{
				State = ViewState.Empty;
			}
			else
			{
				if (string.IsNullOrEmpty(vehicle.ImageUrl))
				{
					vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
				}
				Vehicle = vehicle;
				State = ViewState.Present;
			}
		});
	}

	private void Reserve()
	{
		if (Vehicle is null || model is null)
		{
			return;
		}
	}

	private void Back()
	{
		navigator.GetNavigationControl().GoBack();
	}
}
