using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Data.Models;

namespace WpfApp.Models;

public class ObservableVehicle : ObservableObject
{
	private readonly Vehicle vehicle;

	public ObservableVehicle(Vehicle vehicle) => this.vehicle = vehicle;

	public int Id
	{
		get => vehicle.Id;
		set => SetProperty(vehicle.Id, value, vehicle, (u, v) => u.Id = v);
	}

	public string LicensePlate
	{
		get => vehicle.LicensePlate;
		set => SetProperty(vehicle.LicensePlate, value, vehicle, (u, v) => u.LicensePlate = v);
	}

	public string Brand
	{
		get => vehicle.Brand;
		set => SetProperty(vehicle.Brand, value, vehicle, (u, v) => u.Brand = v);
	}

	public string Name
	{
		get => vehicle.Name;
		set => SetProperty(vehicle.Name, value, vehicle, (u, v) => u.Name = v);
	}

	public int PricePerDay
	{
		get => vehicle.PricePerDay;
		set => SetProperty(vehicle.PricePerDay, value, vehicle, (u, v) => u.PricePerDay = v);
	}

	public VehicleFuel Fuel
	{
		get => vehicle.Fuel;
		set => SetProperty(vehicle.Fuel, value, vehicle, (u, v) => u.Fuel = v);
	}

	public VehicleCategory Category
	{
		get => vehicle.Category;
		set => SetProperty(vehicle.Category, value, vehicle, (u, v) => u.Category = v);
	}

	public string? Color
	{
		get => vehicle.Color;
		set => SetProperty(vehicle.Color, value, vehicle, (u, v) => u.Color = v);
	}

	public string? ImageUrl
	{
		get => vehicle.ImageUrl;
		set => SetProperty(vehicle.ImageUrl, value, vehicle, (u, v) => u.ImageUrl = v);
	}

	public string? Description
	{
		get => vehicle.Description;
		set => SetProperty(vehicle.Description, value, vehicle, (u, v) => u.Description = v);
	}

	public Vehicle ToVehicle()
	{
		return new Vehicle
		{
			Id = Id,
			LicensePlate = LicensePlate,
			Brand = Brand,
			Name = Name,
			PricePerDay = PricePerDay,
			Fuel = Fuel,
			Category = Category,
			Color = Color,
			ImageUrl = ImageUrl,
			Description = Description,
		};
	}
}
