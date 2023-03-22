namespace WpfApp.Data.Models;

public class ReservedVehicle
{
	public User User { get; set; } = default!;
	public Vehicle Vehicle { get; set; } = default!;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
