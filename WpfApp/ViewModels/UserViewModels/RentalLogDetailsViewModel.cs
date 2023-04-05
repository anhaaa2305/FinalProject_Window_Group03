using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Models.UserModels;

namespace WpfApp.ViewModels.UserViewModels;

public class RentalLogDetailsViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private ViewState state;
	private TimeSpan rentalTimeSpan;
	private int totalPrice;
	private bool saving;

	public RentalLogDetailsModel Model { get; set; }
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

	public RentalLogDetailsViewModel(RentalLogDetailsModel model, IVehicleDAO vehicleDAO)
	{
		this.vehicleDAO = vehicleDAO;
		Model = model;
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		State = ViewState.Present;

		var log = model.RentalLog;
		RentalTimeSpan = TimeSpan.FromDays(Math.Max(Math.Ceiling((log.EndDate - log.StartDate).TotalDays), 1));
		var vehicle = log.Vehicle;
		if (vehicle is not null)
		{
			TotalPrice = RentalTimeSpan.Days * vehicle.PricePerDay;
		}
	}

	private async Task SaveAsync()
	{
		saving = true;
		SaveCommand.NotifyCanExecuteChanged();
		await vehicleDAO.UpdateVehicleRentalLogByIdAsync(Model.RentalLog.ToVehicleRentalLog());
		saving = false;
		App.Current.Dispatcher.Invoke(SaveCommand.NotifyCanExecuteChanged);
	}

	private bool CanSave()
	{
		return !saving;
	}
}
