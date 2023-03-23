using Microsoft.Data.SqlClient;

namespace WpfApp.Data.Services;

public class DbServiceOptions : IDbServiceOptions
{
	public SqlConnectionStringBuilder Builder { get; }
	public DbServiceOptions(SqlConnectionStringBuilder builder)
	{
		Builder = builder;
	}
}
