using WpfApp.Middlewares;
using WpfApp.Services;

namespace WpfApp.ViewModels.UserViewModels;

public class HomeViewModel
{
	public HomeViewModel(IAppNavigationService navigator, SessionMiddleware sessionMiddleware)
	{
		navigator.AddMiddleware(sessionMiddleware);
	}
}
