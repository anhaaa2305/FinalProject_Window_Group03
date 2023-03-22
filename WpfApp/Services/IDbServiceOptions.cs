using Microsoft.Data.SqlClient;

namespace WpfApp.Services;

public interface IDbServiceOptions
{
	SqlConnectionStringBuilder Builder { get; }
}
