using Microsoft.Data.SqlClient;
using WpfApp.Data.Models;
using WpfApp.Data.Services;

namespace WpfApp.Data.DAOs;

public class SqlUserDAO : IUserDAO
{
	private readonly IDbService db;
	private readonly IFluentDbReaderFactory readerFactory;

	public SqlUserDAO(IDbService db, IFluentDbReaderFactory readerFactory)
	{
		this.db = db;
		this.readerFactory = readerFactory;
	}

	public async Task<int> AddAsync(User model)
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
			insert into Users (NationalId, Password, FullName, PhoneNumber, IsMale, Address, Email, DateOfBirth)
			output Inserted.Id
			values (@NationalId, @Password, @FullName, @PhoneNumber, @IsMale, @Address, @Email, @DateOfBirth)
		";
		cmd.Parameters.AddWithValue("@NationalId", model.NationalId);
		cmd.Parameters.AddWithValue("@Password", hash);
		cmd.Parameters.AddWithValue("@FullName", model.FullName);
		cmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
		cmd.Parameters.AddWithValue("@IsMale", model.IsMale);
		cmd.Parameters.AddWithValue("@Address", model.Address ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
		return (int)(await cmd.ExecuteScalarAsync().ConfigureAwait(false))!;
	}

	public Task<IReadOnlyCollection<User>> GetAllAsync()
	{
		return Get("select * from Users");
	}

	public async Task<int> UpdateAsync(User model)
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
				NationalId = @NationalId, Password = @Password, FullName = @FullName,
				PhoneNumber = @PhoneNumber, IsMale = @IsMale, Address = @Address,
				Email = @Email, DateOfBirth = @DateOfBirth
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@NationalId", model.NationalId);
		cmd.Parameters.AddWithValue("@Password", model.Password);
		cmd.Parameters.AddWithValue("@FullName", model.FullName);
		cmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
		cmd.Parameters.AddWithValue("@IsMale", model.IsMale);
		cmd.Parameters.AddWithValue("@Address", model.Address ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
		cmd.Parameters.AddWithValue("@Id", model.Id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<User?> GetByIdAsync(int id)
	{
		return (await Get
		(
			"select * from Users where Id = @Id limit 1",
			new SqlParameter("@Id", id)
		).ConfigureAwait(false)).FirstOrDefault();
	}

	public async Task<User?> GetByFullNameAsync(string fullName)
	{
		return (await Get
		(
			"select top 1 * from Users where FullName = @FullName",
			new SqlParameter("@FullName", fullName)
		).ConfigureAwait(false)).FirstOrDefault();
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
			delete from Users
			where Id = @Id limit 1
		";
		cmd.Parameters.AddWithValue("@Id", id);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	private async Task<IReadOnlyCollection<User>> Get(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<User>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var users = new LinkedList<User>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var user = new User();
			readerFactory.Create(reader).Read(user);
			users.AddLast(user);
		}
		return users;
	}
}