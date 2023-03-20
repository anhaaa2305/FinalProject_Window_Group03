namespace WpfApp1.Models;

public class UserRentingLogModel : VehicleModel
{
	public int VehicleId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
