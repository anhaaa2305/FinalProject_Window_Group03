using System.Data;
using System.Data.Common;
using WpfApp1.Models;

namespace WpfApp1.Utils;

public static class DbHelper
{
	public static UserModel Read(UserModel model, DbDataReader reader)
	{
		model.Id = reader.GetInt32("Id");
		model.NationalId = reader.GetString("NationalId");
		model.FullName = reader.GetString("FullName");
		model.IsMale = reader.GetBoolean("IsMale");
		model.DateOfBirth = reader.IsDBNull("DateOfBirth")
			? default
			: reader.GetDateTime("DateOfBirth");
		model.Phone = reader.GetString("Phone");
		model.Email = reader.IsDBNull("Email")
			? default
			: reader.GetString("Email");
		return model;
	}

	public static VehicleModel Read(VehicleModel model, DbDataReader reader)
	{
		model.Id = reader.GetInt32("Id");
		model.LicensePlate = reader.GetString("LicensePlate");
		model.Name = reader.GetString("Name");
		model.PricePerDay = reader.GetInt32("PricePerDay");
		model.Color = reader.IsDBNull("Color")
			? default
			: reader.GetString("Color");
		model.ImageUrl = reader.IsDBNull("ImageUrl")
			? default
			: reader.GetString("ImageUrl");
		return model;
	}

	public static RentedVehicleModel Read(RentedVehicleModel model, DbDataReader reader)
	{
		Read(model as VehicleModel, reader);
		model.UserId = reader.GetInt32("UserId");
		model.StartDate = reader.GetDateTime("StartDate");
		model.EndDate = reader.GetDateTime("EndDate");
		return model;
	}

	public static ReservedVehicleModel Read(ReservedVehicleModel model, DbDataReader reader)
	{
		Read(model as VehicleModel, reader);
		model.UserId = reader.GetInt32("UserId");
		model.StartDate = reader.GetDateTime("StartDate");
		model.EndDate = reader.GetDateTime("EndDate");
		return model;
	}

	public static UserRentingLogModel Read(UserRentingLogModel model, DbDataReader reader)
	{
		Read(model as VehicleModel, reader);
		model.VehicleId = reader.GetInt32("VehicleId");
		model.StartDate = reader.GetDateTime("StartDate");
		model.EndDate = reader.GetDateTime("EndDate");
		return model;
	}
}
