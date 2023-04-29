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
	private string fuel = string.Empty;
	private string category = string.Empty;

	public ObservableVehicle Vehicle { get; set; }
	public IAsyncRelayCommand AddVehicleCommand { get; }

	public string Fuel
	{
		get => fuel;
		set
		{
			if (SetProperty(ref fuel, value))
			{
				AddVehicleCommand.NotifyCanExecuteChanged();
			}
		}
	}

	public string Category
	{
		get => category;
		set
		{
			if (SetProperty(ref category, value))
			{
				AddVehicleCommand.NotifyCanExecuteChanged();
			}
		}
	}

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

	private async Task AddVehicleAsync()
	{
		var dict = new ResourceDictionary()
		{
			Source = new Uri("pack://application:,,,/Resources/Dictionaries/Staff/AddVehicleViewStrings.xaml")
		};
		if (Fuel == (string?)dict["FuelItem_Gasoline"])
		{
			Vehicle.Fuel = VehicleFuel.Gasoline;
		}
		else if (Fuel == (string?)dict["FuelItem_Electric"])
		{
			Vehicle.Fuel = VehicleFuel.Electric;
		}
		else
		{
			Vehicle.Fuel = VehicleFuel.None;
		}
		if (Category == (string?)dict["CategoryItem_TwoWheeled"])
		{
			Vehicle.Category = VehicleCategory.TwoWheeled;
		}
		else if (Category == (string?)dict["CategoryItem_FourWheeled"])
		{
			Vehicle.Category = VehicleCategory.FourWheeled;
		}
		else
		{
			Vehicle.Category = VehicleCategory.None;
		}
		await vehicleDAO.AddAsync(Vehicle.ToVehicle());
	}

	private bool CanAddVehicle()
	{
		return !string.IsNullOrEmpty(Vehicle.Name)
			&& !string.IsNullOrEmpty(Vehicle.LicensePlate)
			&& !string.IsNullOrEmpty(Vehicle.Brand)
			&& Vehicle.PricePerDay > 0
			&& !string.IsNullOrEmpty(Fuel)
			&& !string.IsNullOrEmpty(Category);
	}
}
