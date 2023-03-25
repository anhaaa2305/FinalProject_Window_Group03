using System.Windows;
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
		var navigationService = provider.GetRequiredService<IAppNavigationService>();
		DataContext = provider.GetRequiredService<HomeViewModel>();
		navigationView.SetPageService(provider.GetRequiredService<IPageService>());
		navigationService.SetNavigationControl(navigationView);
	}
}
