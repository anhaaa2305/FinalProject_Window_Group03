using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Data.Models;

namespace WpfApp.Models;

public class ObservableUser : ObservableObject
{
	private readonly User user;

	public ObservableUser(User user) => this.user = user;

	public int Id
	{
		get => user.Id;
		set => SetProperty(user.Id, value, user, (u, v) => u.Id = v);
	}

	public string NationalId
	{
		get => user.NationalId;
		set => SetProperty(user.NationalId, value, user, (u, v) => u.NationalId = v);
	}

	public string Password
	{
		get => user.Password;
		set => SetProperty(user.Password, value, user, (u, v) => u.Password = v);
	}

	public string FullName
	{
		get => user.FullName;
		set => SetProperty(user.FullName, value, user, (u, v) => u.FullName = v);
	}

	public string PhoneNumber
	{
		get => user.PhoneNumber;
		set => SetProperty(user.PhoneNumber, value, user, (u, v) => u.PhoneNumber = v);
	}

	public bool IsMale
	{
		get => user.IsMale;
		set => SetProperty(user.IsMale, value, user, (u, v) => u.IsMale = v);
	}

	public string? Address
	{
		get => user.Address;
		set => SetProperty(user.Address, value, user, (u, v) => u.Address = v);
	}

	public string? Email
	{
		get => user.Email;
		set => SetProperty(user.Email, value, user, (u, v) => u.Email = v);
	}

	public DateTime? DateOfBirth
	{
		get => user.DateOfBirth;
		set => SetProperty(user.DateOfBirth, value, user, (u, v) => u.DateOfBirth = v);
	}
}
