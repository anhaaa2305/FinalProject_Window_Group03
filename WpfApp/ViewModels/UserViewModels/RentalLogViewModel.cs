using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;
using WpfApp.Models.UserModels;
using WpfApp.Services;
using WpfApp.Views;
using WpfApp.Views.UserViews;

namespace WpfApp.ViewModels.UserViewModels;

public class RentalLogViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private readonly ISessionService sessionService;
	private readonly IAppNavigationService navigator;
	private readonly IServiceProvider provider;
	private ObservableCollection<VehicleRentalLog>? logs;
	private ViewState state;

	public IRelayCommand<VehicleRentalLog> ViewItemDetailsCommand { get; }

	public ObservableCollection<VehicleRentalLog>? Logs
	{
		get => logs;
		set => SetProperty(ref logs, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public RentalLogViewModel(IVehicleDAO vehicleDAO, ISessionService sessionService, IAppNavigationService navigator, IServiceProvider provider)
	{
		this.vehicleDAO = vehicleDAO;
		this.sessionService = sessionService;
		this.navigator = navigator;
		this.provider = provider;
		ViewItemDetailsCommand = new RelayCommand<VehicleRentalLog>(ViewItemDetails);

		if (sessionService.User is null)
		{
			App.Current.Services.GetRequiredService<IAppNavigationService>().Navigate<LoginView>();
			return;
		}
		GetRentalLogsAsync().SafeFireAndForget();
	}

	private async Task GetRentalLogsAsync()
	{
		State = ViewState.Busy;

		var logs = await vehicleDAO
			.GetRentalLogsByUserIdAsync(sessionService.User!.Id)
			.ConfigureAwait(false);
		App.Current.Dispatcher.Invoke(() =>
		{
			Logs = new ObservableCollection<VehicleRentalLog>(logs);
			if (Logs.Count == 0)
			{
				State = ViewState.Empty;
			}
			else
			{
				State = ViewState.Present;
			}
		});
	}

	private void ViewItemDetails(VehicleRentalLog? model)
	{
		if (Logs is null || model is null)
		{
			return;
		}
		provider.GetRequiredService<RentalLogDetailsModel>().RentalLog = new ObservableVehicleRentalLog(model);
		navigator.Navigate<RentalLogDetailsView>();
	}
}
