using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Data.DAOs;
using WpfApp.Data.Models;
using WpfApp.Models;

namespace WpfApp.ViewModels.StaffViewModels;

public class AddVehicleViewModel : ObservableObject
{
	private readonly IVehicleDAO vehicleDAO;

	public ObservableVehicle Vehicle { get; set; }
	public IAsyncRelayCommand AddVehicleCommand { get; }

	public AddVehicleViewModel(IVehicleDAO vehicleDAO)
	{
		this.vehicleDAO = vehicleDAO;
		Vehicle = new ObservableVehicle(new Vehicle());
		AddVehicleCommand = new AsyncRelayCommand(AddVehicleAsync, CanAddVehicle);
		Vehicle.PropertyChanged += (_, _) =>
		{
			AddVehicleCommand.NotifyCanExecuteChanged();
		};
	}

	private Task AddVehicleAsync()
	{
		return vehicleDAO.AddAsync(Vehicle.ToVehicle());
	}

	private bool CanAddVehicle()
	{
		return !string.IsNullOrEmpty(Vehicle.Name)
			&& !string.IsNullOrEmpty(Vehicle.LicensePlate)
			&& !string.IsNullOrEmpty(Vehicle.Brand)
			&& Vehicle.PricePerDay > 0;
	}
}
