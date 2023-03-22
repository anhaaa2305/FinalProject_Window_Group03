using Microsoft.Data.SqlClient;

namespace WpfApp.Services;

public class DbServiceOptions : IDbServiceOptions
{
	public SqlConnectionStringBuilder Builder { get; } = new();
}
