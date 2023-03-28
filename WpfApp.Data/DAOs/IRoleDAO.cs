using WpfApp.Data.Constants;
using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public interface IRoleDAO : IDAO<Role>
{
	Task<Role?> GetByFlagAsync(RoleFlag flag);
	Task<int> DeleteByFlagAsync(RoleFlag flag);
}
