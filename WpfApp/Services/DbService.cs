using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace WpfApp.Services;

public class DbService : IDbService
{
	private readonly string connectionString;

	public DbService(IDbServiceOptions options)
	{
		connectionString = options.Builder.ConnectionString;
	}

	public async Task<SqlConnection?> OpenAsync()
	{
		try
		{
			var conn = new SqlConnection(connectionString);
			await conn.OpenAsync().ConfigureAwait(false);
			return conn;
		}
		catch (Exception ex)
		{
			Debug.WriteLine("OpenAsync: " + ex.Message);
			return default;
		}
	}
}
