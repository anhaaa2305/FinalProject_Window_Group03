using Wpf.Ui.Contracts;
using Wpf.Ui.Services;
using WpfApp.Middlewares;
using WpfApp.Models;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;
using StaffModels = WpfApp.Models.StaffModels;
using StaffViewModels = WpfApp.ViewModels.StaffViewModels;
using StaffViews = WpfApp.Views.StaffViews;
using UserViewModels = WpfApp.ViewModels.UserViewModels;
using UserViews = WpfApp.Views.UserViews;

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
			.AddScoped<ReserveVehicleModel>()
			.AddScoped<StaffModels.EditReservationModel>();
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
			.AddTransient<SignOutView>()
			.AddTransient<AccountView>()

			.AddTransient<UserViews.HomeView>()
			.AddTransient<UserViews.AvailableVehicleView>()
			.AddTransient<UserViews.RentedVehicleView>()
			.AddTransient<UserViews.RentalLogView>()
			.AddTransient<UserViews.ReserveVehicleView>()

			.AddTransient<StaffViews.HomeView>()
			.AddTransient<StaffViews.ReservedVehiclesView>()
			.AddTransient<StaffViews.EditReservationView>()
		;
	}

	public static IServiceCollection RegisterViewModels(this IServiceCollection self)
	{
		return self
			.AddSingleton<MainWindowViewModel>()
			.AddTransient<LoginViewModel>()
			.AddTransient<SignOutViewModel>()
			.AddTransient<AccountViewModel>()

			.AddTransient<UserViewModels.HomeViewModel>()
			.AddTransient<UserViewModels.AvailableVehicleViewModel>()
			.AddTransient<UserViewModels.RentedVehicleViewModel>()
			.AddTransient<UserViewModels.RentalLogViewModel>()
			.AddTransient<UserViewModels.ReserveVehicleViewModel>()

			.AddTransient<StaffViewModels.HomeViewModel>()
			.AddTransient<StaffViewModels.ReservedVehiclesViewModel>()
			.AddTransient<StaffViewModels.EditReservationViewModel>()
		;
	}
}
