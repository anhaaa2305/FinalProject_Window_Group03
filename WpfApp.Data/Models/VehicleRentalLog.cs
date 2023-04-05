namespace WpfApp.Data.Models;

public class VehicleRentalLog
{
	public int Id { get; set; }
	public User? User { get; set; }
	public Vehicle? Vehicle { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int? Rate { get; set; }
	public string? Feedback { get; set; }
}
