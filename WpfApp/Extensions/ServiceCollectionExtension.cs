using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;
using WpfApp.Views.User;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterServices(this IServiceCollection self)
	{
		return self
			.AddSingleton<IDbServiceOptions, DbServiceOptions>(provider =>
			{
				var options = new DbServiceOptions();
				options.Builder.DataSource = "localhost";
				options.Builder.InitialCatalog = "WpfApp";
				options.Builder.IntegratedSecurity = true;
				options.Builder.Pooling = false;
				options.Builder.UserID = "sa";
				options.Builder.Password = "123danCkoi";
				options.Builder.TrustServerCertificate = true;
				return options;
			})
			.AddSingleton<IDbService, DbService>()
			.AddScoped<INavigationService, NavigationService>();
	}

	public static IServiceCollection RegisterViews(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindow>()
			.AddTransient<LoginView>()
			.AddTransient<HomeView>()
			.AddTransient<AvailableVehicleView>()
			.AddTransient<RentedVehicleView>()
			.AddTransient<RentalLogView>();
	}

	public static IServiceCollection RegisterViewModels(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindowViewModel>()
			.AddTransient<LoginViewModel>()
			.AddTransient<HomeViewModel>()
			.AddTransient<AvailableVehicleViewModel>()
			.AddTransient<RentedVehicleViewModel>()
			.AddTransient<RentalLogViewModel>();
	}
}
