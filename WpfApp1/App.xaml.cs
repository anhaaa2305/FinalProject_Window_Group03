using System.Security.Permissions;
using System.ServiceProcess;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp1.ViewModels;
using WpfApp1.Views;
using WpfApp1.Views.Pages;

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
			var services = new ServiceCollection();
			services.AddSingleton<MainWindow>();
			services.AddSingleton<LoginPage>();
			services.AddSingleton<LoginPageViewModel>();

			return services.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			var mainWindow = Services.GetRequiredService<MainWindow>();
			mainWindow.Show();

			base.OnStartup(e);
		}
	}
}
