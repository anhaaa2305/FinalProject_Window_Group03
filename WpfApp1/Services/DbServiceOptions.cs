using Microsoft.Data.SqlClient;

namespace WpfApp1.Services;

public class DbServiceOptions : IDbServiceOptions
{
	public SqlConnectionStringBuilder Builder { get; } = new();
}
