using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Context;
using WpfApp.Services;
using WpfApp.Views.User;

namespace WpfApp.ViewModels;

using BCrypt.Net;
using WpfApp.Data.Models;

public class LoginViewModel : ObservableObject
{
	private readonly INavigationService navigationService;
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;
	private readonly ISessionService sessionService;
	private string username = string.Empty;
	private string password = string.Empty;
	public IAsyncRelayCommand LoginCommand { get; }

	public string Username
	{
		get => username; set
		{
			var changed = SetProperty(ref username, value);
			if (changed)
			{
				LoginCommand.NotifyCanExecuteChanged();
			}
		}
	}

	public string Password
	{
		get => password; set
		{
			var changed = SetProperty(ref password, value);
			if (changed)
			{
				LoginCommand.NotifyCanExecuteChanged();
			}
		}
	}

	public LoginViewModel(INavigationService navigationService, IDbContextFactory<AppDbContext> dbContextFactory, ISessionService sessionService)
	{
		this.navigationService = navigationService;
		this.dbContextFactory = dbContextFactory;
		this.sessionService = sessionService;
		LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
	}

	private async Task LoginAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		var user = await ctx.Users
			.Where(e => e.FullName == Username)
			.Select(e => new User { Id = e.Id, FullName = e.FullName, Password = e.Password })
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		if (user is null)
		{
			return;
		}

		var ok = await Task.Run(() => BCrypt.EnhancedVerify(Password, user.Password)).ConfigureAwait(false);
		if (!ok)
		{
			return;
		}

		user.Password = string.Empty;
		await sessionService.LogInAsync(user).ConfigureAwait(false);
		App.Current.Dispatcher.Invoke(() =>
		{
			navigationService.Navigate<HomeView>();
		});
	}

	private bool CanLogin()
	{
		return Username.Length > 0 && Password.Length > 0;
	}
}
