namespace WpfApp.Data.Models;

public class ReservedVehicle
{
	public User User { get; set; } = new();
	public Vehicle Vehicle { get; set; } = new();
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
