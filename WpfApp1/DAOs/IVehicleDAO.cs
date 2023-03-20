using WpfApp1.Models;

namespace WpfApp1.DAOs;

public interface IVehicleDAO : IBaseDAO<VehicleModel>
{
	Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync(UserModel user);
	Task<IReadOnlyCollection<VehicleModel>> GetAllRentedAsync();
	Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync(UserModel user);
	Task<IReadOnlyCollection<VehicleModel>> GetAllReservedAsync();
}
