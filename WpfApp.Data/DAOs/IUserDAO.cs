using WpfApp.Data.Constants;
using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public interface IUserDAO : IDAO<User>
{
	Task<User?> GetByIdAsync(int id);
	Task<User?> GetByFullNameAsync(string fullName);
	Task<int> DeleteByIdAsync(int id);
	Task<Role?> GetRoleByIdAsync(int id);
	Task<int> AddUserRoleAsync(int id, RoleFlag flag);
}
