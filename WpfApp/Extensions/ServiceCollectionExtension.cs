using Wpf.Ui.Contracts;
using Wpf.Ui.Services;
using WpfApp.Middlewares;
using WpfApp.Models;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;
using WpfApp.Views.User;
using StaffViews = WpfApp.Views.StaffViews;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterMiddlewares(this IServiceCollection self)
	{
		return self
			.AddSingleton<SessionMiddleware>();
	}

	public static IServiceCollection RegisterModels(this IServiceCollection self)
	{
		return self
			.AddScoped<ReserveVehicleModel>();
	}

	public static IServiceCollection RegisterServices(this IServiceCollection self)
	{
		return self
			.AddScoped<IPageService, PageService>()
			.AddScoped<IAppNavigationService, AppNavigationService>()
			.AddSingleton<ISessionService, SessionService>()
			.AddScoped<IDialogService, DialogService>();
	}

	public static IServiceCollection RegisterViews(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindow>()
			.AddTransient<LoginView>()
			.AddTransient<HomeView>()
			.AddTransient<AvailableVehicleView>()
			.AddTransient<RentedVehicleView>()
			.AddTransient<RentalLogView>()
			.AddTransient<ReserveVehicleView>()
			.AddTransient<SignOutView>()
			.AddTransient<AccountView>()
			.AddTransient<StaffViews.HomeView>();
	}

	public static IServiceCollection RegisterViewModels(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindowViewModel>()
			.AddTransient<LoginViewModel>()
			.AddTransient<HomeViewModel>()
			.AddTransient<AvailableVehicleViewModel>()
			.AddTransient<RentedVehicleViewModel>()
			.AddTransient<RentalLogViewModel>()
			.AddTransient<ReserveVehicleViewModel>()
			.AddTransient<SignOutViewModel>()
			.AddTransient<AccountViewModel>();
	}
}
