using WpfApp.Data.Models;

namespace WpfApp.Data.Services;

public interface IFluentDbReader
{
	IFluentDbReader Read(User user);
	IFluentDbReader Read(Vehicle vehicle);
	IFluentDbReader Read(RentedVehicle rentedVehicle);
	IFluentDbReader Read(ReservedVehicle reservedVehicle);
	IFluentDbReader Read(VehicleRentalLog log);
	IFluentDbReader Read(Role role);
}
