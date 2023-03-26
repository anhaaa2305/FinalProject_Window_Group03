using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data.Context;
using WpfApp.Views;

namespace WpfApp;

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
			.RegisterSql()
			.RegisterEntityFramework()
			.RegisterMiddlewares()
			.RegisterServices()
			.RegisterViews()
			.RegisterViewModels()
			.BuildServiceProvider();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		if (!EnsureDatabase())
		{
			MessageBox.Show("Database không đảm bảo.");
			Shutdown(1);
			return;
		}

		var mainWindow = Services.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}

	private bool EnsureDatabase()
	{
		var factory = Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
		using var ctx = factory.CreateDbContext();
		var ok = ctx.Database.CanConnect();
		// ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();
		return ok;
	}
}
