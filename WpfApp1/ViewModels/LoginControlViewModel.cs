using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.Services;
using WpfApp1.Views.UserControls;

namespace WpfApp1.ViewModels;

public class LoginControlViewModel : ObservableObject
{
	private string username = string.Empty;
	private string password = string.Empty;
	public IRelayCommand LoginCommand { get; }

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

	private readonly INavigationService navigationService;
	public LoginControlViewModel(INavigationService navigationService)
	{
		this.navigationService = navigationService;
		LoginCommand = new RelayCommand(Login, CanLogin);
	}

	private void Login()
	{
		navigationService.Navigate<UserHomeControl>();
	}

	private bool CanLogin()
	{
		return Username.Length > 0 && Password.Length > 0;
	}
}