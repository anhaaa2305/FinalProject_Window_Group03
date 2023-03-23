using System.ComponentModel;
using System.Windows.Controls;
using WpfApp.Services;
using WpfApp.Views;

namespace WpfApp.Middlewares;

public class SessionMiddleware : INavigationMiddleware
{
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;

    public SessionMiddleware(ISessionService sessionService, INavigationService navigationService)
    {
        this.sessionService = sessionService;
        this.navigationService = navigationService;
    }

    public void OnNavigating(UserControl view, CancelEventArgs e)
    {
        if (sessionService.User is not null)
        {
            return;
        }
        e.Cancel = true;
        navigationService.Navigate<LoginView>();
    }
}