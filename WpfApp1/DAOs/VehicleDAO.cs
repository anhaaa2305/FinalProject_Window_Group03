using System.Diagnostics;
using System.Windows;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Utils;

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
			var oks = await Task.WhenAll
			(
				EnsureVehicleTableAsync(),
				EnsureRentedVehicleTableAsync(),
				EnsureReservedVehicleTableAsync()
			).ConfigureAwait(false);
			return oks.All(ok => ok);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			Debug.WriteLine(ex);
			return false;
		}
	}

	private async Task<bool> EnsureVehicleTableAsync()
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
		return true;
	}

	private async Task<bool> EnsureRentedVehicleTableAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return false;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			if not exists (select * from sysobjects where name='RentedVehicles' and xtype='U')
			create table RentedVehicles(
				Id int not null primary key,
				UserId int not null,
				StartDate datetime not null,
				EndDate datetime not null,
				constraint FK_RentedVehicles_Vehicles foreign key (Id) references Vehicles(Id) on update cascade on delete cascade,
				constraint FK_RentedVehicles_Users foreign key (UserId) references Users(Id) on update cascade on delete cascade
			)
		";
		await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
		return true;
	}

	private async Task<bool> EnsureReservedVehicleTableAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return false;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			if not exists (select * from sysobjects where name='ReservedVehicles' and xtype='U')
			create table ReservedVehicles(
				Id int not null primary key,
				UserId int not null,
				StartDate datetime not null,
				EndDate datetime not null,
				constraint FK_ReservedVehicles_Vehicles foreign key (Id) references Vehicles(Id) on update cascade on delete cascade,
				constraint FK_ReservedVehicles_Users foreign key (UserId) references Users(Id) on update cascade on delete cascade
			)
			";
		await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
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
			models.AddLast(DbHelper.Read(new VehicleModel(), reader));
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
			return DbHelper.Read(new VehicleModel(), reader);
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

	public Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync()
	{
		return GetAllRentedAsync(
		@"
			select * from RentedVehicles
			inner join Vehicles on RentedVehicles.Id = Vehicles.Id
		");
	}

	public Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync(UserModel user)
	{
		return GetAllRentedAsync(
		@"
			select * from RentedVehicles
			where UserId = @Id
			inner join Vehicles on RentedVehicles.Id = Vehicles.Id
		", new SqlParameter("@Id", user.Id));
	}

	public Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync()
	{
		return GetAllReservedAsync(
		@"
			select * from ReservedVehicles
			inner join Vehicles on ReservedVehicles.Id = Vehicles.Id
		");
	}

	public Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync(UserModel user)
	{
		return GetAllRentedAsync(
		@"
			select * from ReservedVehicles
			where UserId = @Id
			inner join Vehicles on RentedVehicles.Id = Vehicles.Id
		", new SqlParameter("@Id", user.Id));
	}

	private async Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleModel>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<VehicleModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			models.AddLast(DbHelper.Read(new ReservedVehicleModel(), reader));
		}
		return models;
	}

	private async Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<VehicleModel>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<VehicleModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			models.AddLast(DbHelper.Read(new RentedVehicleModel(), reader));
		}
		return models;
	}
}
