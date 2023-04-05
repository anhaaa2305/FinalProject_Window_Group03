using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Data.Models;

namespace WpfApp.Models;

public class ObservableVehicleRentalLog : ObservableObject
{
	private readonly VehicleRentalLog vehicleRentalLog;

	public ObservableVehicleRentalLog(VehicleRentalLog vehicleRentalLog)
	{
		this.vehicleRentalLog = vehicleRentalLog;
	}

	public int Id
	{
		get => vehicleRentalLog.Id;
		set => SetProperty(vehicleRentalLog.Id, value, vehicleRentalLog, (u, v) => u.Id = v);
	}

	public User? User
	{
		get => vehicleRentalLog.User;
		set => SetProperty(vehicleRentalLog.User, value, vehicleRentalLog, (u, v) => u.User = v);
	}

	public Vehicle? Vehicle
	{
		get => vehicleRentalLog.Vehicle;
		set => SetProperty(vehicleRentalLog.Vehicle, value, vehicleRentalLog, (u, v) => u.Vehicle = v);
	}

	public DateTime StartDate
	{
		get => vehicleRentalLog.StartDate;
		set => SetProperty(vehicleRentalLog.StartDate, value, vehicleRentalLog, (u, v) => u.StartDate = v);
	}

	public DateTime EndDate
	{
		get => vehicleRentalLog.EndDate;
		set => SetProperty(vehicleRentalLog.EndDate, value, vehicleRentalLog, (u, v) => u.EndDate = v);
	}

	public int Rate
	{
		get => vehicleRentalLog.Rate ?? 0;
		set => SetProperty(vehicleRentalLog.Rate, value, vehicleRentalLog, (u, v) => u.Rate = v);
	}

	public string? Feedback
	{
		get => vehicleRentalLog.Feedback;
		set => SetProperty(vehicleRentalLog.Feedback, value, vehicleRentalLog, (u, v) => u.Feedback = v);
	}

	public VehicleRentalLog ToVehicleRentalLog()
	{
		return new VehicleRentalLog
		{
			Id = vehicleRentalLog.Id,
			User = vehicleRentalLog.User,
			Vehicle = vehicleRentalLog.Vehicle,
			StartDate = vehicleRentalLog.StartDate,
			EndDate = vehicleRentalLog.EndDate,
			Rate = vehicleRentalLog.Rate,
			Feedback = vehicleRentalLog.Feedback,
		};
	}
}
