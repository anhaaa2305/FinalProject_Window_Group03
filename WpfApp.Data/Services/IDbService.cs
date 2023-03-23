using Microsoft.Data.SqlClient;

namespace WpfApp.Data.Services;

public interface IDbService
{
	Task<SqlConnection?> OpenAsync();
}
