using System.ComponentModel;
using System.Windows.Controls;

namespace WpfApp.Middlewares;

public interface INavigationMiddleware
{
    void OnNavigating(UserControl view, CancelEventArgs e);
}