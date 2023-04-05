namespace WpfApp.Data.Models;

public class RentedVehicle
{
	public User User { get; set; } = new();
	public Vehicle Vehicle { get; set; } = new();
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int Deposit { get; set; }
	public string? MortgageNationalId { get; set; }
	public string? Note { get; set; } = string.Empty;
}
