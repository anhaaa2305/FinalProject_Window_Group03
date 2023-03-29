using System.IO;
using System.IO.IsolatedStorage;
using WpfApp.Data.Models;

namespace WpfApp.Services;

public class SessionService : ISessionService
{
	public User? User { get; private set; }

	public async Task LogInAsync(User user)
	{
		User = user;
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		using var stream = new IsolatedStorageFileStream("session.txt", FileMode.OpenOrCreate, store);
		using var writer = new StreamWriter(stream);
		await writer.WriteAsync(user.Id.ToString()).ConfigureAwait(false);
	}

	public void LogOut()
	{
		if (User is not null)
		{
			User = null;
		}
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		if (store.FileExists("session.txt"))
		{
			store.DeleteFile("session.txt");
		}
	}

	public async Task<int?> ReadFromStoreAsync()
	{
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		if (!store.FileExists("session.txt"))
		{
			return default;
		}
		using var stream = new IsolatedStorageFileStream("session.txt", FileMode.Open, store);
		using var reader = new StreamReader(stream);
		var text = await reader.ReadToEndAsync().ConfigureAwait(false);
		if (int.TryParse(text, out var id))
		{
			return id;
		}
		return default;
	}
}
