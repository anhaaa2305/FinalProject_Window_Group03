using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp1.DAOs;
using WpfApp1.Models;

namespace WpfApp1.ViewModels;

public class UserVehicleStoreControlViewModel : ObservableObject
{
	private ObservableCollection<VehicleModel>? vehicles;
	private bool showLoadingTemplate;
	private bool showEmptyTemplate;
	private bool showListViewTemplate;

	public IRelayCommand<VehicleModel> RentCommand { get; }
	public IRelayCommand<VehicleModel> ViewItemDetailsCommand { get; }

	public ObservableCollection<VehicleModel>? Vehicles
	{
		get => vehicles; set => SetProperty(ref vehicles, value);
	}

	public bool ShowLoadingTemplate
	{
		get => showLoadingTemplate;
		set => SetProperty(ref showLoadingTemplate, value);
	}

	public bool ShowEmptyTemplate
	{
		get => showEmptyTemplate;
		set => SetProperty(ref showEmptyTemplate, value);
	}

	public bool ShowListViewTemplate
	{
		get => showListViewTemplate;
		set => SetProperty(ref showListViewTemplate, value);
	}

	public UserVehicleStoreControlViewModel(IVehicleDAO vehicleDAO)
	{
		showListViewTemplate = false;
		showEmptyTemplate = false;
		showLoadingTemplate = true;
		RentCommand = new RelayCommand<VehicleModel>(Rent);
		ViewItemDetailsCommand = new RelayCommand<VehicleModel>(ViewItemDetails);
		Task.Run(async () =>
		{
			var vehicles = await vehicleDAO.GetAllAsync().ConfigureAwait(false);
			foreach (var vehicle in vehicles)
			{
				if (string.IsNullOrEmpty(vehicle.ImageUrl))
				{
					vehicle.ImageUrl = "/Resources/Images/scooter_icon.png";
				}
			}
			App.Current.Dispatcher.Invoke(() =>
			{
				Vehicles = new ObservableCollection<VehicleModel>(vehicles);
				ShowLoadingTemplate = false;
				if (Vehicles.Count == 0)
				{
					ShowEmptyTemplate = true;
				}
				else
				{
					ShowListViewTemplate = true;
				}
			});
		});
	}

	private void Rent(VehicleModel? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}

	private void ViewItemDetails(VehicleModel? model)
	{
		if (Vehicles is null || model is null)
		{
			return;
		}
	}
}
