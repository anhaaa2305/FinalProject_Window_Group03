using WpfApp1.Data.Models;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
	public static ModelBuilder BuildUser(this ModelBuilder self)
	{
		return self.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).UseIdentityColumn();
			entity.Property(e => e.NationalId).IsRequired().HasMaxLength(16);
			entity.Property(e => e.Password).IsRequired().HasMaxLength(61);
			entity.Property(e => e.FullName).IsRequired().HasMaxLength(64);
			entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(16);
			entity.Property(e => e.Address).HasMaxLength(128);
			entity.Property(e => e.Email).HasMaxLength(128);
		});
	}

	public static ModelBuilder BuildVehicle(this ModelBuilder self)
	{
		return self.Entity<Vehicle>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).UseIdentityColumn();
			entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(16);
			entity.Property(e => e.Brand).IsRequired().HasMaxLength(32);
			entity.Property(e => e.Name).IsRequired().HasMaxLength(64);
			entity.Property(e => e.PricePerDay).IsRequired();
			entity.Property(e => e.Color).HasMaxLength(32);
			entity.Property(e => e.ImageUrl).HasMaxLength(128);
			entity.Property(e => e.Description).HasMaxLength(512);
		});
	}

	public static ModelBuilder BuildVehicleRentalLog(this ModelBuilder self)
	{
		return self.Entity<VehicleRentalLog>(entity =>
		{
			entity.Property<int?>("UserId");
			entity.Property<int?>("VehicleId");
			entity.HasNoKey();
			entity
				.HasOne(e => e.User)
				.WithOne()
				.HasForeignKey<VehicleRentalLog>("UserId")
				.OnDelete(DeleteBehavior.SetNull);
			entity
				.Navigation(e => e.User)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity
				.HasOne(e => e.Vehicle)
				.WithOne()
				.HasForeignKey<VehicleRentalLog>("VehicleId")
				.OnDelete(DeleteBehavior.SetNull);
			entity
				.Navigation(e => e.Vehicle)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity.Property(e => e.StartDate).IsRequired();
			entity.Property(e => e.EndDate).IsRequired();
		});
	}

	public static ModelBuilder BuildRentedVehicle(this ModelBuilder self)
	{
		return self.Entity<RentedVehicle>(entity =>
		{
			entity.Property<int>("UserId").IsRequired();
			entity.Property<int>("VehicleId").IsRequired();
			entity.HasKey("VehicleId");
			entity
				.HasOne(e => e.User)
				.WithOne()
				.HasForeignKey<RentedVehicle>("UserId")
				.OnDelete(DeleteBehavior.Cascade);
			entity
				.Navigation(e => e.User)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity
				.HasOne(e => e.Vehicle)
				.WithOne()
				.HasForeignKey<RentedVehicle>("VehicleId")
				.OnDelete(DeleteBehavior.Cascade);
			entity
				.Navigation(e => e.Vehicle)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity.Property(e => e.StartDate).IsRequired();
			entity.Property(e => e.EndDate).IsRequired();
		});
	}

	public static ModelBuilder BuildReservedVehicle(this ModelBuilder self)
	{
		return self.Entity<ReservedVehicle>(entity =>
		{
			entity.Property<int>("UserId").IsRequired();
			entity.Property<int>("VehicleId").IsRequired();
			entity.HasKey("VehicleId");
			entity
				.HasOne(e => e.User)
				.WithOne()
				.HasForeignKey<ReservedVehicle>("UserId")
				.OnDelete(DeleteBehavior.Cascade);
			entity
				.Navigation(e => e.User)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity
				.HasOne(e => e.Vehicle)
				.WithOne()
				.HasForeignKey<ReservedVehicle>("VehicleId")
				.OnDelete(DeleteBehavior.Cascade);
			entity
				.Navigation(e => e.Vehicle)
				.UsePropertyAccessMode(PropertyAccessMode.Property);
			entity.Property(e => e.StartDate).IsRequired();
			entity.Property(e => e.EndDate).IsRequired();
		});
	}
}
