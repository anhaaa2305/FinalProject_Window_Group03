using Microsoft.Data.SqlClient;

namespace WpfApp1.Services;

public interface IDbServiceOptions
{
	SqlConnectionStringBuilder Builder { get; }
}