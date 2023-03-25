using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Services;
using WpfApp.Views.User;

namespace WpfApp.ViewModels;

using System.Resources;
using System.Windows;
using BCrypt.Net;
using WpfApp.Data.DAOs;

public class LoginViewModel : ObservableObject
{
	private readonly INavigationService navigationService;
	private readonly ISessionService sessionService;
	private readonly IUserDAO userDAO;
	private string username = string.Empty;
	private string password = string.Empty;
	private Visibility helpVisibility = Visibility.Collapsed;
	private string helpText = string.Empty;
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

	public Visibility HelpVisibility
	{
		get => helpVisibility; set => SetProperty(ref helpVisibility, value);
	}

	public string HelpText
	{
		get => helpText; set => SetProperty(ref helpText, value);
	}

	public LoginViewModel(INavigationService navigationService, ISessionService sessionService, IUserDAO userDAO)
	{
		this.navigationService = navigationService;
		this.sessionService = sessionService;
		this.userDAO = userDAO;
		LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
	}

	private async Task LoginAsync()
	{
		var user = await userDAO.GetByFullNameAsync(Username).ConfigureAwait(false);
		if (user is null)
		{
			App.Current.Dispatcher.Invoke(() =>
			{
				SetHelp("LoginError_Text");
			});
			return;
		}

		var ok = await Task.Run(() => BCrypt.EnhancedVerify(Password, user.Password)).ConfigureAwait(false);
		if (!ok)
		{
			App.Current.Dispatcher.Invoke(() =>
			{
				SetHelp("LoginError_Text");
			});
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

	private void SetHelp(string text)
	{
		var dict = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/Resources/Dictionaries/LoginStrings.xaml")
		};
		if (dict is null)
		{
			return;
		}
		if (string.IsNullOrEmpty(text))
		{
			HelpVisibility = Visibility.Collapsed;
			return;
		}
		HelpVisibility = Visibility.Visible;
		HelpText = (string?)dict[text] ?? string.Empty;
	}
}
