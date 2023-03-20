using System.Diagnostics;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Utils;

namespace WpfApp1.DAOs;

public class UserDAO : IUserDAO
{
	private readonly IDbService db;

	public UserDAO(IDbService db)
	{
		this.db = db;
	}

	public async Task<bool> EnsureTableAsync()
	{
		try
		{
			var oks = await Task.WhenAll
			(
				EnsureUsersTableAsync(),
				EnsureRentingLogsTableAsync()
			);
			return oks.All(ok => ok);
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message);
			return false;
		}
	}

	private async Task<bool> EnsureUsersTableAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return false;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			if not exists (select * from sysobjects where name='Users' and xtype='U')
				create table Users(
				Id int not null identity(1, 1) primary key,
				NationalId varchar(16) not null,
				Password varchar(61) not null,
				FullName varchar(64) not null,
				Phone varchar(16) not null,
				Email varchar(128) null,
				IsMale bit default 1,
				DateOfBirth datetime null
			)
		";
		await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
		// for (var i = 0; i != 100; ++i)
		// {
		// 	await AddAsync(new UserModel
		// 	{
		// 		NationalId = (261616417 + i).ToString(),
		// 		FullName = $"User No.{i}",
		// 		IsMale = Random.Shared.Next(1000) > 500,
		// 		DateOfBirth = Random.Shared.Next(1000) > 500 ? null : DateTime.Now,
		// 		Phone = $"0{978649520 + i}",
		// 		Email = Random.Shared.Next(1000) > 500 ? null : $"user{i}@gmail.com"
		// 	});
		// }
		return true;
	}

	private async Task<bool> EnsureRentingLogsTableAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return false;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			if not exists (select * from sysobjects where name='UserRentingLogs' and xtype='U')
				create table UserRentingLogs(
				Id int not null,
				VehicleId int not null,
				LicensePlate varchar(16) not null,
				Name varchar(64) not null,
				PricePerDay int not null,
				Color varchar(32) null,
				ImageUrl varchar(256) null,
				StartDate datetime not null,
				EndDate datetime not null
			)
		";
		await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
		return true;
	}

	public async Task<int> AddAsync(UserModel model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}

		var hash = await Task.Run(() => BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password))
			.ConfigureAwait(false);
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into Users (NationalId, FullName, Password, IsMale, DateOfBirth, Phone, Email)
			output Inserted.Id
			values (@NationalId, @FullName, @Password, @IsMale, @DateOfBirth, @Phone, @Email)
		";
		cmd.Parameters.AddWithValue("@NationalId", model.NationalId);
		cmd.Parameters.AddWithValue("@FullName", model.FullName);
		cmd.Parameters.AddWithValue("@Password", hash);
		cmd.Parameters.AddWithValue("@IsMale", model.IsMale);
		cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Phone", model.Phone);
		cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
		return (int)(await cmd.ExecuteScalarAsync().ConfigureAwait(false))!;
	}

	public async Task<int> DeleteByIdAsync(UserModel model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			delete from Users
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<UserModel>> GetAllAsync()
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<UserModel>();
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Users
		";
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

		var models = new LinkedList<UserModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			models.AddLast(DbHelper.Read(new UserModel(), reader));
		}
		return models;
	}

	public async Task<UserModel?> GetByIdAsync(UserModel model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return default;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			select * from Users where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@Id", model.Id);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			return DbHelper.Read(new UserModel(), reader);
		}
		return default;
	}

	public async Task<int> UpdateAsync(UserModel model)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return default;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			update Users set
				NationalId = @NationalId, FullName = @FullName, IsMale = @IsMale,
				DateOfBirth = @DateOfBirth, Phone = @Phone, Email = @Email
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@NationalId", model.NationalId);
		cmd.Parameters.AddWithValue("@FullName", model.FullName);
		cmd.Parameters.AddWithValue("@IsMale", model.IsMale);
		cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Phone", model.Phone);
		cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public Task<IReadOnlyCollection<UserRentingLogModel>> GetAllRentingLogsAsync()
	{
		return GetAllRentalRecordsAsync(
		@"
			select * from UserRentingLogs
		");
	}

	public Task<IReadOnlyCollection<UserRentingLogModel>> GetAllRentingLogsAsync(UserModel model)
	{
		return GetAllRentalRecordsAsync(
		@"
			select * from UserRentingLogs
			where Id = @Id
		", new SqlParameter("@Id", model.Id));
	}

	private async Task<IReadOnlyCollection<UserRentingLogModel>> GetAllRentalRecordsAsync(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<UserRentingLogModel>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var models = new LinkedList<UserRentingLogModel>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			models.AddLast(DbHelper.Read(new UserRentingLogModel(), reader));
		}
		return models;
	}
}
