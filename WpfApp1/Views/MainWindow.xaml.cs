using System.Windows.Navigation;
using WpfApp1.Views.Pages;

namespace WpfApp1.Views;

public partial class MainWindow : NavigationWindow
{
	public MainWindow(LoginPage loginPage)
	{
		InitializeComponent();

		Navigate(loginPage);
	}

	private void OnNavigated(object? sender, NavigationEventArgs e)
	{
	}
}
