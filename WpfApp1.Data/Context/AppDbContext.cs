using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Models;

namespace WpfApp1.Data.Context;

public class AppDbContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Vehicle> Vehicles => Set<Vehicle>();
	public DbSet<RentedVehicle> RentedVehicles => Set<RentedVehicle>();
	public DbSet<ReservedVehicle> ReservedVehicles => Set<ReservedVehicle>();
	public DbSet<VehicleRentalLog> VehicleRentalLogs => Set<VehicleRentalLog>();

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.BuildUser()
			.BuildVehicle()
			.BuildRentedVehicle()
			.BuildReservedVehicle()
			.BuildVehicleRentalLog();
	}
}
