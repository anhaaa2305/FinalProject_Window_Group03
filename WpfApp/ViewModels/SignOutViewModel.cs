using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Services;

namespace WpfApp.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using WpfApp.Views;

public class SignOutViewModel : ObservableObject
{
	private readonly ISessionService sessionService;
	private readonly IAppNavigationService navigator;

	public IRelayCommand SignOutCommand { get; }
	public IRelayCommand BackCommand { get; }

	public SignOutViewModel(ISessionService sessionService, IAppNavigationService navigator)
	{
		this.sessionService = sessionService;
		this.navigator = navigator;
		SignOutCommand = new RelayCommand(SignOut);
		BackCommand = new RelayCommand(Back);
	}

	private void SignOut()
	{
		sessionService.LogOut();
		App.Current.Services.GetRequiredService<IAppNavigationService>().Navigate<LoginView>();
	}

	private void Back()
	{
		navigator.GetNavigationControl().GoBack();
		navigator.GetNavigationControl().ClearJournal();
	}
}
