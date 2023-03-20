using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.DAOs;
using WpfApp1.Views;

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
				.RegisterDAOs()
				.RegisterServices()
				.RegisterViews()
				.RegisterViewModels()
				.BuildServiceProvider();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var ok = await EnsureDatabase();
			if (!ok)
			{
				MessageBox.Show("Database không đảm bảo.");
				Shutdown(1);
				return;
			}

			var mainWindow = Services.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}

		private async Task<bool> EnsureDatabase()
		{
			var oks = await Task.WhenAll
			(
				Services.GetRequiredService<IUserDAO>().EnsureTableAsync(),
				Services.GetRequiredService<IVehicleDAO>().EnsureTableAsync()
			);
			return oks.All(ok => ok);
		}
	}
}
