using WpfApp.Middlewares;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;
using WpfApp.Views.User;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterMiddlewares(this IServiceCollection self)
	{
		return self
			.AddSingleton<SessionMiddleware>();
	}

	public static IServiceCollection RegisterServices(this IServiceCollection self)
	{
		return self
			.AddScoped<INavigationService, NavigationService>()
			.AddSingleton<ISessionService, SessionService>();
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
