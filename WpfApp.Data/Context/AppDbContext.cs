using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Models;

namespace WpfApp.Data.Context;

public class AppDbContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Vehicle> Vehicles => Set<Vehicle>();
	public DbSet<RentedVehicle> RentedVehicles => Set<RentedVehicle>();
	public DbSet<ReservedVehicle> ReservedVehicles => Set<ReservedVehicle>();
	public DbSet<VehicleRentalLog> VehicleRentalLogs => Set<VehicleRentalLog>();
	public DbSet<UserRole> UserRoles => Set<UserRole>();
	public DbSet<Role> Roles => Set<Role>();

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.BuildUser()
			.BuildVehicle()
			.BuildRentedVehicle()
			.BuildReservedVehicle()
			.BuildVehicleRentalLog()
			.BuildRole()
			.BuildUserRole();
	}
}
