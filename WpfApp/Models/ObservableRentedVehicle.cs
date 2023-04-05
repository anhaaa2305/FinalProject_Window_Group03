using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Data.Models;

namespace WpfApp.Models;

public class ObservableRentedVehicle : ObservableObject
{
	private readonly RentedVehicle rentedVehicle;

	public ObservableRentedVehicle(RentedVehicle rentedVehicle)
	{
		this.rentedVehicle = rentedVehicle;
	}

	public User User
	{
		get => rentedVehicle.User;
		set => SetProperty(rentedVehicle.User, value, rentedVehicle, (u, v) => u.User = v);
	}

	public Vehicle Vehicle
	{
		get => rentedVehicle.Vehicle;
		set => SetProperty(rentedVehicle.Vehicle, value, rentedVehicle, (u, v) => u.Vehicle = v);
	}

	public DateTime StartDate
	{
		get => rentedVehicle.StartDate;
		set => SetProperty(rentedVehicle.StartDate, value, rentedVehicle, (u, v) => u.StartDate = v);
	}

	public DateTime EndDate
	{
		get => rentedVehicle.EndDate;
		set => SetProperty(rentedVehicle.EndDate, value, rentedVehicle, (u, v) => u.EndDate = v);
	}

	public int Deposit
	{
		get => rentedVehicle.Deposit;
		set => SetProperty(rentedVehicle.Deposit, value, rentedVehicle, (u, v) => u.Deposit = v);
	}

	public string? MortgageNationalId
	{
		get => rentedVehicle.MortgageNationalId;
		set => SetProperty(rentedVehicle.MortgageNationalId, value, rentedVehicle, (u, v) => u.MortgageNationalId = v);
	}

	public string? Note
	{
		get => rentedVehicle.Note;
		set => SetProperty(rentedVehicle.Note, value, rentedVehicle, (u, v) => u.Note = v);
	}

	public RentedVehicle ToRentedVehicle()
	{
		return new RentedVehicle
		{
			User = rentedVehicle.User,
			Vehicle = rentedVehicle.Vehicle,
			StartDate = rentedVehicle.StartDate,
			EndDate = rentedVehicle.EndDate,
			Deposit = rentedVehicle.Deposit,
			MortgageNationalId = rentedVehicle.MortgageNationalId,
			Note = rentedVehicle.Note,
		};
	}
}
