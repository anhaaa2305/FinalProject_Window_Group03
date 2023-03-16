using Microsoft.Data.SqlClient;

namespace WpfApp1.Services;

public interface IDbService
{
	Task<SqlConnection?> OpenAsync();
}
