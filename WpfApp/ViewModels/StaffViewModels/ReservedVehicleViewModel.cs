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

public class ReservedVehiclesViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly IServiceProvider provider;
	private readonly IAppNavigationService navigator;
	private IReadOnlyCollection<ReservedVehicle>? vehicles;
	private ViewState state;

	public IRelayCommand<ReservedVehicle> ViewItemDetailsCommand { get; }
	public IRelayCommand<ReservedVehicle> EditItemCommand { get; }

	public IReadOnlyCollection<ReservedVehicle>? ReservedVehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public ReservedVehiclesViewModel(IVehicleDAO vehicleDAO, ISessionService sessionService, IServiceProvider provider, IAppNavigationService navigator)
	{
		this.vehicleDAO = vehicleDAO;
		this.provider = provider;
		this.navigator = navigator;
		ViewItemDetailsCommand = new RelayCommand<ReservedVehicle>(ViewItemDetails);
		EditItemCommand = new RelayCommand<ReservedVehicle>(EditItem);

		if (sessionService.User is null)
		{
			App.Current.Services.GetRequiredService<IAppNavigationService>().Navigate<LoginView>();
			return;
		}
		GetReservedVehiclesAsync().SafeFireAndForget();
	}

	private async Task GetReservedVehiclesAsync()
	{
		State = ViewState.Busy;

		var reservedVehicles = await vehicleDAO.GetAllReservedVehiclesAsync().ConfigureAwait(false);
		foreach (var reserved in reservedVehicles)
		{
			if (string.IsNullOrEmpty(reserved.Vehicle.ImageUrl))
			{
				reserved.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			ReservedVehicles = reservedVehicles;
			State = ReservedVehicles.Count switch
			{
				0 => ViewState.Empty,
				_ => ViewState.Present
			};
		});
	}

	private void ViewItemDetails(ReservedVehicle? model)
	{
		if (ReservedVehicles is null || model is null)
		{
			return;
		}
	}

	private void EditItem(ReservedVehicle? model)
	{
		if (ReservedVehicles is null || model is null)
		{
			return;
		}
		provider.GetRequiredService<EditReservationModel>().ReservedVehicle = new ObservableReservedVehicle(model);
		navigator.Navigate<EditReservationView>();
	}
}
