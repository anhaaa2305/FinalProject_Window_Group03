using WpfApp1.DAOs;
using WpfApp1.Services;
using WpfApp1.ViewModels;
using WpfApp1.Views;
using WpfApp1.Views.User;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterDAOs(this IServiceCollection self)
	{
		return self
			.AddSingleton<IUserDAO, UserDAO>()
			.AddSingleton<IVehicleDAO, VehicleDAO>();
	}

	public static IServiceCollection RegisterServices(this IServiceCollection self)
	{
		return self
			.AddSingleton<IDbServiceOptions, DbServiceOptions>(provider =>
			{
				var options = new DbServiceOptions();
				options.Builder.DataSource = "localhost";
				options.Builder.InitialCatalog = "wpfapp1";
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
			.AddTransient<RentalHistoryView>();
	}

	public static IServiceCollection RegisterViewModels(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindowViewModel>()
			.AddSingleton<LoginViewModel>()
			.AddSingleton<HomeViewModel>()
			.AddTransient<AvailableVehicleViewModel>()
			.AddTransient<RentedVehicleViewModel>();
	}
}
