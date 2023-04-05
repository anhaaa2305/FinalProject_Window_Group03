using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public interface IVehicleDAO : IDAO<Vehicle>
{
	Task<Vehicle?> GetByIdAsync(int id);
	Task<int> DeleteByIdAsync(int id);
	Task<IReadOnlyCollection<Vehicle>> GetAvailableVehicles();
	Task<IReadOnlyCollection<RentedVehicle>> GetRentedByUserIdAsync(int userId);
	Task<IReadOnlyCollection<ReservedVehicle>> GetReservedByUserIdAsync(int userId);
	Task<IReadOnlyCollection<VehicleRentalLog>> GetRentalLogsByUserIdAsync(int userId);
	Task<int> AddReservedVehicleAsync(ReservedVehicle reservedVehicle);
	Task<int> AddRentedVehicleAsync(RentedVehicle rentedVehicle);
	Task<int> DeleteReservedVehicleByVehicleIdAsync(int vehicleId);
	Task<int> DeleteRentedVehicleByVehicleIdAsync(int vehicleId);
	Task<IReadOnlyCollection<RentedVehicle>> GetAllRentedVehiclesAsync();
	Task<IReadOnlyCollection<ReservedVehicle>> GetAllReservedVehiclesAsync();
	Task<int> UpdateReservedVehicleByVehicleIdAsync(ReservedVehicle reservedVehicle);
	Task<int> UpdateRentedVehicleByVehicleIdAsync(RentedVehicle rentedVehicle);
	Task<int> AddVehicleRentalLogAsync(VehicleRentalLog log);
	Task<int> UpdateVehicleRentalLogByIdAsync(VehicleRentalLog log);
}
