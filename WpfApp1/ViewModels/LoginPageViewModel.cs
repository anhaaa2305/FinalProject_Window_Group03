using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfApp1.ViewModels;

public class LoginPageViewModel : ObservableObject
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

	public LoginPageViewModel()
	{
		LoginCommand = new RelayCommand(Login, CanLogin);
	}

	private void Login()
	{
		Debug.WriteLine(Username);
		Debug.WriteLine(Password);
	}

	private bool CanLogin()
	{
		return Username.Length > 0 && Password.Length > 0;
	}
}
