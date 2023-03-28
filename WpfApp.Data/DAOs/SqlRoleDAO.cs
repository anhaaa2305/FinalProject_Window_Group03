using Microsoft.Data.SqlClient;
using WpfApp.Data.Constants;
using WpfApp.Data.Models;
using WpfApp.Data.Services;

namespace WpfApp.Data.DAOs;

public class SqlRoleDAO : IRoleDAO
{
	private readonly IDbService db;
	private readonly IFluentDbReaderFactory readerFactory;

	public SqlRoleDAO(IDbService db, IFluentDbReaderFactory readerFactory)
	{
		this.db = db;
		this.readerFactory = readerFactory;
	}

	public async Task<int> AddAsync(Role role)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			insert into Roles (Flag, Name)
			values (@Flag, @Name)
		";
		cmd.Parameters.AddWithValue("@Flag", role.Flag);
		cmd.Parameters.AddWithValue("@Name", role.Name);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public Task<IReadOnlyCollection<Role>> GetAllAsync()
	{
		return Get("select * from Roles");
	}

	public async Task<int> UpdateAsync(Role role)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return default;
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			update top 1 Roles set
				Name = @Name
			where Flag = @Flag
		";
		cmd.Parameters.AddWithValue("@Name", role.Name);
		cmd.Parameters.AddWithValue("@Flag", role.Flag);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	public async Task<Role?> GetByFlagAsync(RoleFlag flag)
	{
		return (await Get
		(
			"select top 1 * from Roles where Flag = @Flag",
			new SqlParameter("@Flag", flag)
		).ConfigureAwait(false)).FirstOrDefault();
	}

	public async Task<int> DeleteByFlagAsync(RoleFlag flag)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return 0;
		}
		using var cmd = conn.CreateCommand();
		cmd.CommandText =
		@"
			delete top 1 from Roles
			where Flag = @Flag
		";
		cmd.Parameters.AddWithValue("@Flag", flag);
		return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
	}

	private async Task<IReadOnlyCollection<Role>> Get(string commandText, params SqlParameter[] parameters)
	{
		await using var conn = await db.OpenAsync().ConfigureAwait(false);
		if (conn is null)
		{
			return Array.Empty<Role>();
		}

		using var cmd = conn.CreateCommand();
		cmd.CommandText = commandText;
		cmd.Parameters.AddRange(parameters);
		using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
		var roles = new LinkedList<Role>();
		while (await reader.ReadAsync().ConfigureAwait(false))
		{
			var role = new Role();
			readerFactory.Create(reader).Read(role);
			roles.AddLast(role);
		}
		return roles;
	}
}
