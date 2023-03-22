namespace WpfApp1.Data.Models;

public class RentedVehicle
{
	public User User { get; set; } = default!;
	public Vehicle Vehicle { get; set; } = default!;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
