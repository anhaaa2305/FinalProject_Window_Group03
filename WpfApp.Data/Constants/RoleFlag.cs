namespace WpfApp.Data.Constants;

[Flags]
public enum RoleFlag
{
	None = 0b_0000,
	Staff = 0b_0001,
	Admin = Staff << 1,
}
