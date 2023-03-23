using Microsoft.Data.SqlClient;

namespace WpfApp.Data.Services;

public interface IDbServiceOptions
{
	SqlConnectionStringBuilder Builder { get; }
}
