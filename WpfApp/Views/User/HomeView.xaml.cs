using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Contracts;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Views.User;

public partial class HomeView : UserControl
{
	public HomeView(IServiceScopeFactory scopeFactory)
	{
		InitializeComponent();

		var scope = scopeFactory.CreateScope();
		var provider = scope.ServiceProvider;
		var navigator = provider.GetRequiredService<IAppNavigationService>();
		navigationView.SetPageService(provider.GetRequiredService<IPageService>());
		navigator.SetNavigationControl(navigationView);
		DataContext = provider.GetRequiredService<HomeViewModel>();
		Unloaded += (_, _) =>
		{
			scope.Dispose();
		};
	}
}
