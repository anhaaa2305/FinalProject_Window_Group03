namespace WpfApp1.Models;

public class UserModel
{
	public int Id { get; set; }
	public string NationalId { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public bool IsMale { get; set; } = true;
	public DateTime? DateOfBirth { get; set; } = default;
}
