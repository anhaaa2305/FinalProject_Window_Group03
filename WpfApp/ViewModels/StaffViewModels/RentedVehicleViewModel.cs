using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;
using WpfApp.Models.StaffModels;
using WpfApp.Services;
using WpfApp.Views;
using WpfApp.Views.StaffViews;

namespace WpfApp.ViewModels.StaffViewModels;

public class RentedVehiclesViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly IServiceProvider provider;
	private readonly IAppNavigationService navigator;
	private IReadOnlyCollection<RentedVehicle>? vehicles;
	private ViewState state;

	public IRelayCommand<RentedVehicle> ViewItemDetailsCommand { get; }
	public IRelayCommand<RentedVehicle> EditItemCommand { get; }

	public IReadOnlyCollection<RentedVehicle>? RentedVehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public RentedVehiclesViewModel(IVehicleDAO vehicleDAO, ISessionService sessionService, IServiceProvider provider, IAppNavigationService navigator)
	{
		this.vehicleDAO = vehicleDAO;
		this.provider = provider;
		this.navigator = navigator;
		ViewItemDetailsCommand = new RelayCommand<RentedVehicle>(ViewItemDetails);
		EditItemCommand = new RelayCommand<RentedVehicle>(EditItem);

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

		var rentedVehicles = await vehicleDAO.GetAllRentedVehiclesAsync().ConfigureAwait(false);
		foreach (var reserved in rentedVehicles)
		{
			if (string.IsNullOrEmpty(reserved.Vehicle.ImageUrl))
			{
				reserved.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			RentedVehicles = rentedVehicles;
			State = RentedVehicles.Count switch
			{
				0 => ViewState.Empty,
				_ => ViewState.Present
			};
		});
	}

	private void ViewItemDetails(RentedVehicle? model)
	{
		if (RentedVehicles is null || model is null)
		{
			return;
		}
	}

	private void EditItem(RentedVehicle? model)
	{
		if (RentedVehicles is null || model is null)
		{
			return;
		}
		provider.GetRequiredService<EditRentalModel>().RentedVehicle = new ObservableRentedVehicle(model);
		navigator.Navigate<EditRentalView>();
	}
}
