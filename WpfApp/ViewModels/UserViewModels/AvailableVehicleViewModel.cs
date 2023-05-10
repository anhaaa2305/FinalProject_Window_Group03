using System.Diagnostics;
using System.Windows.Controls;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;
using WpfApp.Services;
using WpfApp.Views.UserViews;

namespace WpfApp.ViewModels.UserViewModels;

public class AvailableVehicleViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;
	private List<Vehicle>? displayVehicles;
	private List<Vehicle>? vehicles;
	private ViewState state;
	private SortType sortType;
	private object? selectedSortItem = null;
	private string nameFilter = string.Empty;

	public IRelayCommand<Vehicle> ReserveCommand { get; }

	public List<Vehicle>? Vehicles
	{
		get => displayVehicles; set => SetProperty(ref displayVehicles, value);
	}

	public ViewState State
	{
		get => state;
		set => SetProperty(ref state, value);
	}

	public object? SelectedSortItem
	{
		get => selectedSortItem;
		set
		{
			if (selectedSortItem != value)
			{
				selectedSortItem = value;
				if (value is ComboBoxItem item && item.Tag is SortType type)
				{
					sortType = type;
					ProcessDisplayVehicles();
				}
			}
		}
	}

	public string NameFilter
	{
		get => nameFilter;
		set
		{
			nameFilter = value;
			ProcessDisplayVehicles();
		}
	}

	private void ProcessDisplayVehicles()
	{
		if (vehicles is null)
		{
			return;
		}
		var enumerable = string.IsNullOrEmpty(nameFilter.Trim()) ? vehicles : vehicles.Where(v => v.Name.StartsWith(nameFilter, true, null));
		Vehicles = sortType == SortType.LowPrice
			? enumerable.OrderBy(v => v.PricePerDay).ToList()
			: enumerable.OrderByDescending(v => v.PricePerDay).ToList();
	}

	private readonly IAppNavigationService navigator;
	private readonly ReserveVehicleModel reserveVehicleModel;
	public AvailableVehicleViewModel(IVehicleDAO vehicleDAO, IAppNavigationService navigator, ReserveVehicleModel reserveVehicleModel)
	{
		this.vehicleDAO = vehicleDAO;
		this.navigator = navigator;
		this.reserveVehicleModel = reserveVehicleModel;
		ReserveCommand = new RelayCommand<Vehicle>(Reserve);

		GetAvailableVehiclesAsync().SafeFireAndForget();
	}

	private async Task GetAvailableVehiclesAsync()
	{
		State = ViewState.Busy;

		vehicles = (await vehicleDAO
			.GetAvailableVehicles()
			.ConfigureAwait(false)).ToList();
		foreach (var vehicle in vehicles)
		{
			if (string.IsNullOrEmpty(vehicle.ImageUrl))
			{
				vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
			}
		}
		App.Current.Dispatcher.Invoke(() =>
		{
			ProcessDisplayVehicles();
			if (vehicles.Count == 0)
			{
				State = ViewState.Empty;
			}
			else
			{
				State = ViewState.Present;
			}
		});
	}

	private void Reserve(Vehicle? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}

		reserveVehicleModel.Vehicle = model;
		navigator.Navigate<ReserveVehicleView>();
	}
}
