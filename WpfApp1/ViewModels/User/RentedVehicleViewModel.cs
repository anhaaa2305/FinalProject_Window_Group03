using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Data.Context;
using WpfApp1.Data.Models;

namespace WpfApp1.ViewModels;

public class RentedVehicleViewModel : ObservableObject
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;
	private ObservableCollection<RentedVehicle>? vehicles;
	private ViewState state;

	public IRelayCommand<RentedVehicle> ViewItemDetailsCommand { get; }

	public ObservableCollection<RentedVehicle>? Vehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public RentedVehicleViewModel(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
		ViewItemDetailsCommand = new RelayCommand<RentedVehicle>(ViewItemDetails);

		FetchVehiclesAsync().SafeFireAndForget();
	}

	private async Task FetchVehiclesAsync()
	{
		State = ViewState.Busy;

		using var ctx = dbContextFactory.CreateDbContext();
		var vehicles = await ctx.RentedVehicles
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
		foreach (var vehicle in vehicles)
		{
			if (string.IsNullOrEmpty(vehicle.Vehicle.ImageUrl))
			{
				vehicle.Vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			Vehicles = new ObservableCollection<RentedVehicle>(vehicles);
			if (Vehicles.Count == 0)
			{
				State = ViewState.Empty;
			}
			else
			{
				State = ViewState.Present;
			}
		});
	}

	private void ViewItemDetails(RentedVehicle? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
