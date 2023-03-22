using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp.Data;
using WpfApp.Data.Context;
using WpfApp.Data.Models;

namespace WpfApp.ViewModels;

public class RentalLogViewModel : ObservableObject
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;
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

	public RentalLogViewModel(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
		ViewItemDetailsCommand = new RelayCommand<VehicleRentalLog>(ViewItemDetails);

		GetRentalLogsAsync().SafeFireAndForget();
	}

	private async Task GetRentalLogsAsync()
	{
		State = ViewState.Busy;

		using var ctx = dbContextFactory.CreateDbContext();
		var logs = await ctx.VehicleRentalLogs
			.Where(e => e.User != null && e.User.Id == 1)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
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
	}
}
