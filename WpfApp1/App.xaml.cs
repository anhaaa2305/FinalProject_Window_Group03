using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.ViewModels;
using WpfApp1.Views;
using WpfApp1.Views.UserControls;

namespace WpfApp1
{
	public partial class App : Application
	{
		public new static App Current => (App)Application.Current;
		public IServiceProvider Services { get; }

		public App()
		{
			Services = ConfigureServices();
		}

		private static IServiceProvider ConfigureServices()
		{
			return new ServiceCollection()
				.AddSingleton<INavigationService, NavigationService>()

				.AddSingleton<MainWindow>()
				.AddSingleton<MainWindowViewModel>()

				.AddSingleton<LoginControl>()
				.AddSingleton<LoginControlViewModel>()

				.AddSingleton<UserHomeControl>()

				.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			var mainWindow = Services.GetRequiredService<MainWindow>();
			mainWindow.Show();

			base.OnStartup(e);
		}
	}
}
