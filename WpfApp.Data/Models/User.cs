namespace WpfApp.Data.Models;

public class User
{
	public int Id { get; set; }
	public string NationalId { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public bool IsMale { get; set; }
	public string? Address { get; set; }
	public string? Email { get; set; }
	public DateTime? DateOfBirth { get; set; }
}

