using System.Data;
using Microsoft.Data.SqlClient;
using WpfApp.Data.Models;
using WpfApp.Data.Services;

namespace WpfApp.Data.DAOs;

public class SqlVehicleDAO : IVehicleDAO
{
	private readonly IDbService db;
	private readonly IFluentDbReaderFactory readerFactory;

	public SqlVehicleDAO(IDbService db, IFluentDbReaderFactory readerFactory)
	{
		this.db = db;
		this.readerFactory = readerFactory;
	}

	public async Task<int> AddAsync(Vehicle model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into Vehicles (LicensePlate, Brand, Name, PricePerDay, Color, ImageUrl, Description)
			output Inserted.Id
			values (@LicensePlate, @Brand, @Name, @PricePerDay, @Color, @ImageUrl, @Description)
		";
		cmd.Parameters.AddWithValue("@LicensePlate", model.LicensePlate);
		cmd.Parameters.AddWithValue("@Brand", model.Brand);
		cmd.Parameters.AddWithValue("@Name", model.Name);
		cmd.Parameters.AddWithValue("@PricePerDay", model.PricePerDay);
		cmd.Parameters.AddWithValue("@Color", model.Color ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
		return (int)(await cmd.ExecuteScalarAsync().ConfigureAwait(false))!;
	}

	public async Task<IReadOnlyCollection<Vehicle>> GetAllAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<Vehicle>();
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Vehicles
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

		var vehicles = new LinkedList<Vehicle>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var vehicle = new Vehicle();
			readerFactory.Create(reader).Read(vehicle);
			vehicles.AddLast(vehicle);
		}
		return vehicles;
	}

	public async Task<int> UpdateAsync(Vehicle model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return default;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			update Vehicles set
				LicensePlate = @LicensePlate, Brand = @Brand, Name = @Name,
				PricePerDay = @PricePerDay, Color = @Color, ImageUrl = @ImageUrl,
				Description = @Description
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@LicensePlate", model.LicensePlate);
		cmd.Parameters.AddWithValue("@Brand", model.Brand);
		cmd.Parameters.AddWithValue("@Name", model.Name);
		cmd.Parameters.AddWithValue("@PricePerDay", model.PricePerDay);
		cmd.Parameters.AddWithValue("@Color", model.Color ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<int> AddReservedVehicleAsync(ReservedVehicle reservedVehicle)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into ReservedVehicles (UserId, VehicleId, StartDate, EndDate)
			values (@UserId, @VehicleId, @StartDate, @EndDate)
		";
		cmd.Parameters.AddWithValue("@UserId", reservedVehicle.User.Id);
		cmd.Parameters.AddWithValue("@VehicleId", reservedVehicle.Vehicle.Id);
		cmd.Parameters.AddWithValue("@StartDate", reservedVehicle.StartDate);
		cmd.Parameters.AddWithValue("@EndDate", reservedVehicle.EndDate);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<int> AddRentedVehicleAsync(RentedVehicle rentedVehicle)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into ReservedVehicles (UserId, VehicleId, StartDate, EndDate)
			values (@UserId, @VehicleId, @StartDate, @EndDate)
		";
		cmd.Parameters.AddWithValue("@UserId", rentedVehicle.User.Id);
		cmd.Parameters.AddWithValue("@VehicleId", rentedVehicle.Vehicle.Id);
		cmd.Parameters.AddWithValue("@StartDate", rentedVehicle.StartDate);
		cmd.Parameters.AddWithValue("@EndDate", rentedVehicle.EndDate);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<Vehicle?> GetByIdAsync(int id)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return default;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Vehicles where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@Id", id);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var vehicle = new Vehicle();
			readerFactory.Create(reader).Read(vehicle);
			return vehicle;
		}
		return default;
	}

	public async Task<int> DeleteByIdAsync(int id)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			delete from Vehicles
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@Id", id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<Vehicle>> GetAvailableVehicles()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<Vehicle>();
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Vehicles
			where Id not in
				(select VehicleId from RentedVehicles
				union
				select VehicleId from ReservedVehicles)
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

		var vehicles = new LinkedList<Vehicle>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var vehicle = new Vehicle();
			readerFactory.Create(reader).Read(vehicle);
			vehicles.AddLast(vehicle);
		}
		return vehicles;
	}

	public Task<IReadOnlyCollection<RentedVehicle>> GetRentedByUserIdAsync(int userId)
	{
		return GetAllRentedAsync(
		@"
			select * from RentedVehicles
				inner join Users on Users.Id = RentedVehicles.UserId
				inner join Vehicles on Vehicles.Id = RentedVehicles.VehicleId
			where UserId = @UserId
		", new SqlParameter("@UserId", userId));
	}

	public Task<IReadOnlyCollection<ReservedVehicle>> GetReservedByUserIdAsync(int userId)
	{
		return GetAllReservedAsync(
		@"
			select * from ReservedVehicles
				inner join Users on Users.Id = RentedVehicles.UserId
				inner join Vehicles on Vehicles.Id = RentedVehicles.VehicleId
			where UserId = @UserId
		", new SqlParameter("@UserId", userId));
	}

	public Task<IReadOnlyCollection<VehicleRentalLog>> GetRentalLogsByUserIdAsync(int userId)
	{
		return GetAllRentalRecordsAsync(
		@"
			select * from VehicleRentalLogs
				inner join Users on Users.Id = VehicleRentalLogs.UserId
				inner join Vehicles on Vehicles.Id = VehicleRentalLogs.VehicleId
			where UserId = @UserId
		", new SqlParameter("@UserId", userId));
	}

	private async Task<IReadOnlyCollection<ReservedVehicle>> GetAllReservedAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<ReservedVehicle>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<ReservedVehicle>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var reserved = new ReservedVehicle();
			readerFactory
				.Create(reader)
				.Read(reserved)
				.Read(reserved.User)
				.Read(reserved.Vehicle);
			models.AddLast(reserved);
		}
		return models;
	}

	private async Task<IReadOnlyCollection<RentedVehicle>> GetAllRentedAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<RentedVehicle>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<RentedVehicle>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var rented = new RentedVehicle();
			readerFactory.Create(reader)
				.Read(rented)
				.Read(rented.User)
				.Read(rented.Vehicle);
			models.AddLast(rented);
		}
		return models;
	}

	private async Task<IReadOnlyCollection<VehicleRentalLog>> GetAllRentalRecordsAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleRentalLog>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<VehicleRentalLog>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var log = new VehicleRentalLog();
			var r = readerFactory.Create(reader).Read(log);
			if (!reader.IsDBNull("UserId"))
			{
				log.User = new();
				r.Read(log.User);
			}
			if (!reader.IsDBNull("VehicleId"))
			{
				log.Vehicle = new();
				r.Read(log.Vehicle);
			}
			models.AddLast(log);
		}
		return models;
	}
}