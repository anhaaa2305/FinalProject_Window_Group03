using Microsoft.Data.SqlClient;

namespace WpfApp.Services;

public interface IDbService
{
	Task<SqlConnection?> OpenAsync();
}
