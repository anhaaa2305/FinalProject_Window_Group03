using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models.StaffModels;
using WpfApp.Services;

namespace WpfApp.ViewModels.StaffViewModels;

public class EditReservationViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly IAppNavigationService navigator;
	private ViewState state;
	private TimeSpan rentalTimeSpan;
	private int totalPrice;
	private bool settingPickedUp;
	private bool cancelling;
	private bool saving;

	public EditReservationModel Model { get; set; }
	public IAsyncRelayCommand SetPickedUpCommand { get; }
	public IAsyncRelayCommand CancelCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }

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

	public EditReservationViewModel(EditReservationModel model, IVehicleDAO vehicleDAO, IAppNavigationService navigator)
	{
		this.vehicleDAO = vehicleDAO;
		this.navigator = navigator;
		Model = model;
		SetPickedUpCommand = new AsyncRelayCommand(SetPickedUpAsync, CanBeSetPickedUp);
		CancelCommand = new AsyncRelayCommand(CancelAsync, CanCancel);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		State = ViewState.Present;


		var reserved = model.ReservedVehicle;

		reserved.PropertyChanged += (sender, e) =>
		{
			if (e.PropertyName == nameof(reserved.MortgageNationalId)
			|| e.PropertyName == nameof(reserved.Deposit))
			{
				SetPickedUpCommand.NotifyCanExecuteChanged();
			}
		};

		RentalTimeSpan = TimeSpan.FromDays(Math.Max(Math.Ceiling((reserved.EndDate - reserved.StartDate).TotalDays), 1));
		var vehicle = reserved.Vehicle;
		if (vehicle is not null)
		{
			TotalPrice = RentalTimeSpan.Days * vehicle.PricePerDay;
		}
	}

	private async Task SetPickedUpAsync()
	{
		settingPickedUp = true;
		SetPickedUpCommand.NotifyCanExecuteChanged();
		var reserved = Model.ReservedVehicle;
		var addTask = vehicleDAO.AddRentedVehicleAsync(new RentedVehicle
		{
			User = reserved.User,
			Vehicle = reserved.Vehicle,
			StartDate = reserved.StartDate,
			EndDate = reserved.EndDate,
			Deposit = reserved.Deposit,
			MortgageNationalId = reserved.MortgageNationalId,
			Note = reserved.Note,
		});
		var deleteTask = vehicleDAO.DeleteReservedVehicleByVehicleIdAsync(reserved.Vehicle.Id);
		await Task.WhenAll(addTask, deleteTask).ConfigureAwait(false);
		settingPickedUp = false;
		App.Current.Dispatcher.Invoke(() =>
		{
			SetPickedUpCommand.NotifyCanExecuteChanged();
			navigator.GetNavigationControl().GoBack();
			navigator.GetNavigationControl().ClearJournal();
		});
	}

	private bool CanBeSetPickedUp()
	{
		var reservedVehicle = Model.ReservedVehicle;
		return !settingPickedUp && !string.IsNullOrEmpty(reservedVehicle.MortgageNationalId)
			&& reservedVehicle.Deposit > 0;
	}

	private async Task CancelAsync()
	{
		cancelling = true;
		CancelCommand.NotifyCanExecuteChanged();
		await vehicleDAO.DeleteReservedVehicleByVehicleIdAsync(Model.ReservedVehicle.Vehicle.Id).ConfigureAwait(false);
		cancelling = false;
		App.Current.Dispatcher.Invoke(() =>
		{
			CancelCommand.NotifyCanExecuteChanged();
			navigator.GetNavigationControl().GoBack();
			navigator.GetNavigationControl().ClearJournal();
		});
	}

	private bool CanCancel()
	{
		return !cancelling;
	}

	private async Task SaveAsync()
	{
		saving = true;
		SaveCommand.NotifyCanExecuteChanged();
		await vehicleDAO.UpdateReservedVehicleByVehicleIdAsync(Model.ReservedVehicle.ToReservedVehicle());
		saving = false;
		App.Current.Dispatcher.Invoke(SaveCommand.NotifyCanExecuteChanged);
	}

	private bool CanSave()
	{
		return !saving;
	}
}
