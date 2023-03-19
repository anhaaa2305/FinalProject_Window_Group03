using System.Data;
using System.Data.Common;
using System.Diagnostics;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.DAOs;

public class VehicleDAO : IVehicleDAO
{
	private readonly IDbService db;

	public VehicleDAO(IDbService db)
	{
		this.db = db;
	}

	public async Task<bool> EnsureTableAsync()
	{
		try
		{
			await using var conn = await db.OpenAsync().ConfigureAwait(false);
			if (conn is null)
			{
				return false;
			}

			using var cmd = conn.CreateCommand();
			cmd.CommandText =
			@"
			if not exists (select * from sysobjects where name='Vehicles' and xtype='U')
			create table Vehicles(
				Id int not null identity(1, 1) primary key,
				LicensePlate varchar(16) not null unique,
				Name varchar(64) not null,
				PricePerDay int not null default 0,
				Color varchar(32) null,
				ImageUrl varchar(256) null
			)
			";
			// for (var i = 0; i != 100; ++i)
			// {
			// 	await AddAsync(new VehicleModel
			// 	{
			// 		LicensePlate = $"86-B1-{35904 + i}",
			// 		Name = "Yamaha Sirius",
			// 		PricePerDay = 120_000,
			// 	});
			// }
			await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
		}
		catch (Exception ex) { Debug.WriteLine(ex); }
		return true;
	}

	public async Task<int> AddAsync(VehicleModel model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into Vehicles (LicensePlate, Name, PricePerDay, Color, ImageUrl)
			output Inserted.Id
			values (@LicensePlate, @Name, @PricePerDay, @Color, @ImageUrl)
		";
		cmd.Parameters.AddWithValue("@LicensePlate", model.LicensePlate);
		cmd.Parameters.AddWithValue("@Name", model.Name);
		cmd.Parameters.AddWithValue("@PricePerDay", model.PricePerDay);
		cmd.Parameters.AddWithValue("@Color", model.Color ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
		return (int)(await cmd.ExecuteScalarAsync().ConfigureAwait(false))!;
	}

	public async Task<int> DeleteByIdAsync(VehicleModel model)
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
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<VehicleModel>> GetAllAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleModel>();
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Vehicles
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

		var models = new LinkedList<VehicleModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var model = new VehicleModel();
			PopulateData(model, reader);
			models.AddLast(model);
		}
		return models;
	}

	public async Task<VehicleModel?> GetByIdAsync(VehicleModel model)
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
		cmd.Parameters.AddWithValue("@Id", model.Id);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var m = new VehicleModel();
			PopulateData(m, reader);
			return m;
		}
		return default;
	}

	public async Task<int> UpdateAsync(VehicleModel model)
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
				LicensePlate = @LicensePlate, Name = @Name, PricePerDay = @PricePerDay,
				Color = @Color, ImageUrl = @ImageUrl
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@LicensePlate", model.LicensePlate);
		cmd.Parameters.AddWithValue("@Name", model.Name);
		cmd.Parameters.AddWithValue("@PricePerDay", model.PricePerDay);
		cmd.Parameters.AddWithValue("@Color", model.Color ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleModel>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from RentedVehicles
			inner join Vehicles on RentedVehicles.Id = Vehicles.Id
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<VehicleModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var model = new RentedVehicleModel
			{
				UserId = reader.GetInt32("UserId"),
				StartDate = reader.GetDateTime("StartDate"),
				EndDate = reader.GetDateTime("EndDate"),
			};
			PopulateData(model, reader);
			models.AddLast(model);
		}
		return models;
	}

	public async Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleModel>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from ReservedVehicles
			inner join Vehicles on ReservedVehicles.Id = Vehicles.Id
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<VehicleModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var model = new ReservedVehicleModel
			{
				UserId = reader.GetInt32("UserId"),
				StartDate = reader.GetDateTime("StartDate"),
				EndDate = reader.GetDateTime("EndDate"),
			};
			PopulateData(model, reader);
			models.AddLast(model);
		}
		return models;
	}

	private static void PopulateData(VehicleModel model, DbDataReader reader)
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
	}
}
