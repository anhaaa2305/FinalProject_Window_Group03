using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models.StaffModels;
using WpfApp.Services;

namespace WpfApp.ViewModels.StaffViewModels;

public class EditRentalViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly IAppNavigationService navigator;
	private ViewState state;
	private TimeSpan rentalTimeSpan;
	private int totalPrice;
	private bool confirming;
	private bool saving;

	public EditRentalModel Model { get; set; }
	public IAsyncRelayCommand SaveCommand { get; }
	public IAsyncRelayCommand ConfirmReturnCommand { get; }

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public TimeSpan RentalTimeSpan
	{
		get => rentalTimeSpan;
		set => SetProperty(ref rentalTimeSpan, value);
	}

	public int TotalPrice
	{
		get => totalPrice;
		set => SetProperty(ref totalPrice, value);
	}

	public EditRentalViewModel(EditRentalModel model, IVehicleDAO vehicleDAO, IAppNavigationService navigator)
	{
		this.vehicleDAO = vehicleDAO;
		this.navigator = navigator;
		Model = model;
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		ConfirmReturnCommand = new AsyncRelayCommand(ConfirmReturnAsync, CanConfirmReturn);
		State = ViewState.Present;

		var rented = model.RentedVehicle;
		RentalTimeSpan = TimeSpan.FromDays(Math.Max(Math.Ceiling((rented.EndDate - rented.StartDate).TotalDays), 1));
		var vehicle = rented.Vehicle;
		if (vehicle is not null)
		{
			TotalPrice = RentalTimeSpan.Days * vehicle.PricePerDay;
		}
	}

	private async Task ConfirmReturnAsync()
	{
		confirming = true;
		ConfirmReturnCommand.NotifyCanExecuteChanged();
		var rented = Model.RentedVehicle;
		var deleteTask = vehicleDAO.DeleteRentedVehicleByVehicleIdAsync(rented.Vehicle.Id);
		var addLogTask = vehicleDAO.AddVehicleRentalLogAsync(new VehicleRentalLog
		{
			User = rented.User,
			Vehicle = rented.Vehicle,
			StartDate = rented.StartDate,
			EndDate = rented.EndDate,
		});

		await Task.WhenAll(deleteTask, addLogTask);
		confirming = false;
		App.Current.Dispatcher.Invoke(() =>
		{
			ConfirmReturnCommand.NotifyCanExecuteChanged();
			navigator.GetNavigationControl().GoBack();
			navigator.GetNavigationControl().ClearJournal();
		});
	}

	private bool CanConfirmReturn()
	{
		return !confirming;
	}

	private async Task SaveAsync()
	{
		saving = true;
		SaveCommand.NotifyCanExecuteChanged();
		await vehicleDAO.UpdateRentedVehicleByVehicleIdAsync(Model.RentedVehicle.ToRentedVehicle());
		saving = false;
		App.Current.Dispatcher.Invoke(SaveCommand.NotifyCanExecuteChanged);
	}

	private bool CanSave()
	{
		return !saving;
	}
}
