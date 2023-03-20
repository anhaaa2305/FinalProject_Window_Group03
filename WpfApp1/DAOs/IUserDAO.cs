using WpfApp1.Models;

namespace WpfApp1.DAOs;

public interface IUserDAO : IBaseDAO<UserModel>
{
	Task<IReadOnlyCollection<UserRentingLogModel>> GetAllRentingLogsAsync();
	Task<IReadOnlyCollection<UserRentingLogModel>> GetAllRentingLogsAsync(UserModel model);
}
