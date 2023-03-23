using System.IO;
using System.IO.IsolatedStorage;
using WpfApp.Data.Models;

namespace WpfApp.Services;

public class SessionService : ISessionService
{
	private User? user;

	public User? User => user;

	public async Task LogInAsync(User user)
	{
		this.user = user;
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		if (store.FileExists(".session"))
		{
			using var stream = new IsolatedStorageFileStream(".session", FileMode.Create, store);
			using var writer = new StreamWriter(stream);
			await writer.WriteAsync(user.Id.ToString()).ConfigureAwait(false);
		}
	}

	public void LogOut()
	{
		if (this.user is not null)
		{
			this.user = null;
		}
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		if (store.FileExists(".session"))
		{
			store.DeleteFile(".session");
		}
	}

	public async Task<int?> ReadFromStoreAsync()
	{
		var store = IsolatedStorageFile.GetUserStoreForAssembly();
		if (!store.FileExists(".session"))
		{
			return default;
		}
		using var stream = new IsolatedStorageFileStream(".session", FileMode.Open, store);
		using var reader = new StreamReader(stream);
		var text = await reader.ReadToEndAsync().ConfigureAwait(false);
		if (int.TryParse(text, out var id))
		{
			return id;
		}
		return default;
	}
}