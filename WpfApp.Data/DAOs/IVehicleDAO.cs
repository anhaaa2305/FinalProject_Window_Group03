using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public interface IVehicleDAO : IDAO<Vehicle>
{
	Task<Vehicle?> GetByIdAsync(int id);
	Task<int> DeleteByIdAsync(int id);
	Task<IReadOnlyCollection<RentedVehicle>> GetRentedByUserIdAsync(int userId);
	Task<IReadOnlyCollection<ReservedVehicle>> GetReservedByUserIdAsync(int userId);
	Task<IReadOnlyCollection<VehicleRentalLog>> GetRentalLogsByUserIdAsync(int userId);
}
