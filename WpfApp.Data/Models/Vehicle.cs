namespace WpfApp.Data.Models;

public enum VehicleFuel
{
	None,
	Gasoline,
	Electric,
}

public enum VehicleCategory
{
	None,
	TwoWheeled,
	FourWheeled,
}

public class Vehicle
{
	public int Id { get; set; }
	public string LicensePlate { get; set; } = string.Empty;
	public string Brand { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public int PricePerDay { get; set; }
	public VehicleFuel Fuel { get; set; }
	public VehicleCategory Category { get; set; }
	public string? Color { get; set; }
	public string? ImageUrl { get; set; }
	public string? Description { get; set; }
}
