using System.ComponentModel;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.Middlewares;

public class SessionMiddleware : INavigationMiddleware
{
	private readonly ISessionService sessionService;
	private readonly IAppNavigationService navigationService;

	public SessionMiddleware(ISessionService sessionService, IAppNavigationService navigationService)
	{
		this.sessionService = sessionService;
		this.navigationService = navigationService;
	}

	public void OnNavigating(Type viewType, CancelEventArgs e)
	{
		if (sessionService.User is not null)
		{
			return;
		}
		e.Cancel = true;
		navigationService.Navigate(typeof(LoginView));
	}
}
