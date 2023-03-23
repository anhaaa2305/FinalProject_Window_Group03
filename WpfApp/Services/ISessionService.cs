using WpfApp.Data.Models;

namespace WpfApp.Services;

public interface ISessionService
{
	User? User { get; }
	Task LogInAsync(User user);
	void LogOut();
	Task<int?> ReadFromStoreAsync();
}