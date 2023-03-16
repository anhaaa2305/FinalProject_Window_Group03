namespace WpfApp1.Models;

public class VehicleModel
{
	public int Id { get; set; }
	public string LicensePlate { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public int PricePerDay { get; set; }
	public string? Color { get; set; }
	public string? ImageUrl { get; set; }
}
