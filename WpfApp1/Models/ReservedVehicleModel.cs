namespace WpfApp1.Models;

public class ReservedVehicleModel : VehicleModel
{
	public int UserId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
