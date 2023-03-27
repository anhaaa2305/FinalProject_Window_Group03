namespace WpfApp.Data.Models;

public class UserRole
{
	public User User { get; set; } = new();
	public Role Role { get; set; } = new();
}

