using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Data.Models;

namespace WpfApp.Models;

public class ObservableReservedVehicle : ObservableObject
{
	private readonly ReservedVehicle reservedVehicle;

	public ObservableReservedVehicle(ReservedVehicle reservedVehicle)
	{
		this.reservedVehicle = reservedVehicle;
	}

	public User User
	{
		get => reservedVehicle.User;
		set => SetProperty(reservedVehicle.User, value, reservedVehicle, (u, v) => u.User = v);
	}

	public Vehicle Vehicle
	{
		get => reservedVehicle.Vehicle;
		set => SetProperty(reservedVehicle.Vehicle, value, reservedVehicle, (u, v) => u.Vehicle = v);
	}

	public DateTime StartDate
	{
		get => reservedVehicle.StartDate;
		set => SetProperty(reservedVehicle.StartDate, value, reservedVehicle, (u, v) => u.StartDate = v);
	}

	public DateTime EndDate
	{
		get => reservedVehicle.EndDate;
		set => SetProperty(reservedVehicle.EndDate, value, reservedVehicle, (u, v) => u.EndDate = v);
	}

	public int Deposit
	{
		get => reservedVehicle.Deposit;
		set => SetProperty(reservedVehicle.Deposit, value, reservedVehicle, (u, v) => u.Deposit = v);
	}

	public string? MortgageNationalId
	{
		get => reservedVehicle.MortgageNationalId;
		set => SetProperty(reservedVehicle.MortgageNationalId, value, reservedVehicle, (u, v) => u.MortgageNationalId = v);
	}

	public string? Note
	{
		get => reservedVehicle.Note;
		set => SetProperty(reservedVehicle.Note, value, reservedVehicle, (u, v) => u.Note = v);
	}

	public ReservedVehicle ToReservedVehicle()
	{
		return new ReservedVehicle
		{
			User = reservedVehicle.User,
			Vehicle = reservedVehicle.Vehicle,
			StartDate = reservedVehicle.StartDate,
			EndDate = reservedVehicle.EndDate,
			Deposit = reservedVehicle.Deposit,
			MortgageNationalId = reservedVehicle.MortgageNationalId,
			Note = reservedVehicle.Note,
		};
	}
}
