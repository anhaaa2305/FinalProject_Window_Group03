using System.Data.Common;
using WpfApp.Data.Constants;
using WpfApp.Data.Models;

namespace WpfApp.Data.Services;

public class FluentDbReader : IFluentDbReader
{
	private readonly DbDataReader reader;
	private int ordinal;

	public FluentDbReader(DbDataReader reader, int ordinal)
	{
		this.reader = reader;
		this.ordinal = ordinal;
	}

	public IFluentDbReader Read(User user)
	{
		user.Id = reader.GetInt32(ordinal++);
		user.NationalId = reader.GetString(ordinal++);
		user.Password = reader.GetString(ordinal++);
		user.FullName = reader.GetString(ordinal++);
		user.PhoneNumber = reader.GetString(ordinal++);
		user.IsMale = reader.GetBoolean(ordinal++);
		user.Address = GetOrDefault<string?>();
		user.Email = GetOrDefault<string?>();
		user.DateOfBirth = GetOrDefault<DateTime?>();
		return this;
	}

	public IFluentDbReader Read(Vehicle vehicle)
	{
		vehicle.Id = reader.GetInt32(ordinal++);
		vehicle.LicensePlate = reader.GetString(ordinal++);
		vehicle.Brand = reader.GetString(ordinal++);
		vehicle.Name = reader.GetString(ordinal++);
		vehicle.PricePerDay = reader.GetInt32(ordinal++);
		vehicle.Fuel = (VehicleFuel)reader.GetInt32(ordinal++);
		vehicle.Category = (VehicleCategory)reader.GetInt32(ordinal++);
		vehicle.Color = GetOrDefault<string?>();
		vehicle.ImageUrl = GetOrDefault<string?>();
		vehicle.Description = GetOrDefault<string?>();
		return this;
	}

	public IFluentDbReader Read(RentedVehicle rentedVehicle)
	{
		ordinal += 2;
		rentedVehicle.StartDate = reader.GetDateTime(ordinal++);
		rentedVehicle.EndDate = reader.GetDateTime(ordinal++);
		rentedVehicle.Deposit = reader.GetInt32(ordinal++);
		rentedVehicle.MortgageNationalId = GetOrDefault<string?>();
		rentedVehicle.Note = GetOrDefault<string?>();
		return this;
	}

	public IFluentDbReader Read(ReservedVehicle reservedVehicle)
	{
		ordinal += 2;
		reservedVehicle.StartDate = reader.GetDateTime(ordinal++);
		reservedVehicle.EndDate = reader.GetDateTime(ordinal++);
		reservedVehicle.Deposit = reader.GetInt32(ordinal++);
		reservedVehicle.MortgageNationalId = GetOrDefault<string?>();
		reservedVehicle.Note = GetOrDefault<string?>();
		return this;
	}

	public IFluentDbReader Read(VehicleRentalLog log)
	{
		log.Id = reader.GetInt32(ordinal++);
		ordinal += 2;
		log.StartDate = reader.GetDateTime(ordinal++);
		log.EndDate = reader.GetDateTime(ordinal++);
		log.Rate = GetOrDefault<int?>();
		log.Feedback = GetOrDefault<string?>();
		return this;
	}

	public IFluentDbReader Read(Role role)
	{
		role.Flag = (RoleFlag)reader.GetInt32(ordinal++);
		role.Name = reader.GetString(ordinal++);
		return this;
	}

	private T? GetOrDefault<T>()
	{
		var value = reader.IsDBNull(ordinal) ? default : (T)reader.GetValue(ordinal);
		++ordinal;
		return value;
	}
}
