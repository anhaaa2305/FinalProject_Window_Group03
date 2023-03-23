using WpfApp.Data.Models;

namespace WpfApp.Services;

public interface ISessionService
{
	Task LogInAsync(User user);
	void LogOut();
	Task<int?> ReadFromStoreAsync();
}