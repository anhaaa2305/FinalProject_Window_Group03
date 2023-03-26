using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
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
		ConfigureApp();
		Services = ConfigureServices();
	}

	private static void ConfigureApp()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
		Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
		FrameworkElement.LanguageProperty.OverrideMetadata
		(
			typeof(FrameworkElement),
			new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag))
		);
		TextElement.FontSizeProperty.OverrideMetadata(
			typeof(TextElement),
			new FrameworkPropertyMetadata(14.0));
		TextBlock.FontSizeProperty.OverrideMetadata(
			typeof(TextBlock),
			new FrameworkPropertyMetadata(14.0));
	}

	private static IServiceProvider ConfigureServices()
	{
		return new ServiceCollection()
			.RegisterSql()
			.RegisterEntityFramework()
			.RegisterMiddlewares()
			.RegisterModels()
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

		Services.GetRequiredService<MainWindow>().Show();
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
