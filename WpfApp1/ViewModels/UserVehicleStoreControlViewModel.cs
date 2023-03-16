using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp1.DAOs;
using WpfApp1.Models;

namespace WpfApp1.ViewModels;

public class UserVehicleStoreControlViewModel : ObservableObject
{
	private ObservableCollection<VehicleModel>? vehicles;
	private bool showLoadingTemplate;
	private bool showEmptyTemplate;
	private bool showDataGridTemplate;

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

	public bool ShowDataGridTemplate
	{
		get => showDataGridTemplate;
		set => SetProperty(ref showDataGridTemplate, value);
	}

	public UserVehicleStoreControlViewModel(IVehicleDAO vehicleDAO)
	{
		showDataGridTemplate = false;
		showEmptyTemplate = false;
		showLoadingTemplate = true;
		Task.Run(async () =>
		{
			var vehicles = await vehicleDAO.GetAllAsync().ConfigureAwait(false);
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
					ShowDataGridTemplate = true;
				}
			});
		});
	}
}
