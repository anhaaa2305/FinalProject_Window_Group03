using System.Data;
using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Context;
using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public class EFVehicleDAO : IVehicleDAO
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public EFVehicleDAO(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
	}

	public async Task<int> AddAsync(Vehicle model)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Vehicles.Add(model);
		await ctx.SaveChangesAsync().ConfigureAwait(false);
		return model.Id;
	}

	public async Task<IReadOnlyCollection<Vehicle>> GetAllAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Vehicles.ToArrayAsync().ConfigureAwait(false);
	}

	public async Task<int> UpdateAsync(Vehicle model)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Vehicles.Update(model);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<int> AddReservedVehicleAsync(ReservedVehicle reservedVehicle)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Users.Entry(reservedVehicle.User).State = EntityState.Unchanged;
		ctx.Vehicles.Entry(reservedVehicle.Vehicle).State = EntityState.Unchanged;
		ctx.ReservedVehicles.Add(reservedVehicle);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<int> AddRentedVehicleAsync(RentedVehicle rentedVehicle)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Users.Entry(rentedVehicle.User).State = EntityState.Unchanged;
		ctx.Vehicles.Entry(rentedVehicle.Vehicle).State = EntityState.Unchanged;
		ctx.RentedVehicles.Add(rentedVehicle);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<Vehicle?> GetByIdAsync(int id)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Vehicles
			.Where(e => e.Id == id)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> DeleteByIdAsync(int id)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Vehicles
			.Where(e => e.Id == id)
			.Take(1)
			.ExecuteDeleteAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<Vehicle>> GetAvailableVehicles()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Vehicles
			.Where(e => !ctx.RentedVehicles
				.Select(x => x.Vehicle.Id)
				.Union(ctx.ReservedVehicles
					.Select(x => x.Vehicle.Id))
				.Any(id => id == e.Id))
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<RentedVehicle>> GetRentedByUserIdAsync(int userId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.RentedVehicles
			.Where(e => e.User.Id == userId)
			.Include(e => e.User)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<ReservedVehicle>> GetReservedByUserIdAsync(int userId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.ReservedVehicles
			.Where(e => e.User.Id == userId)
			.Include(e => e.User)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<VehicleRentalLog>> GetRentalLogsByUserIdAsync(int userId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.VehicleRentalLogs
			.Where(e => e.User != null && e.User.Id == userId)
			.Include(e => e.User)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> DeleteReservedVehicleByVehicleIdAsync(int vehicleId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.ReservedVehicles
			.Where(e => e.Vehicle.Id == vehicleId)
			.Take(1)
			.ExecuteDeleteAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> DeleteRentedVehicleByVehicleIdAsync(int vehicleId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.RentedVehicles
			.Where(e => e.Vehicle.Id == vehicleId)
			.Take(1)
			.ExecuteDeleteAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<RentedVehicle>> GetAllRentedVehiclesAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.RentedVehicles
			.Include(e => e.User)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<ReservedVehicle>> GetAllReservedVehiclesAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.ReservedVehicles
			.Include(e => e.User)
			.Include(e => e.Vehicle)
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> UpdateReservedVehicleByVehicleIdAsync(ReservedVehicle reservedVehicle)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Users.Entry(reservedVehicle.User).State = EntityState.Unchanged;
		ctx.Vehicles.Entry(reservedVehicle.Vehicle).State = EntityState.Unchanged;
		ctx.ReservedVehicles.Update(reservedVehicle);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<int> UpdateRentedVehicleByVehicleIdAsync(RentedVehicle rentedVehicle)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Users.Entry(rentedVehicle.User).State = EntityState.Unchanged;
		ctx.Vehicles.Entry(rentedVehicle.Vehicle).State = EntityState.Unchanged;
		ctx.RentedVehicles.Update(rentedVehicle);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<int> AddVehicleRentalLogAsync(VehicleRentalLog log)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		if (log.User is not null)
		{
			ctx.Users.Entry(log.User).State = EntityState.Unchanged;
		}
		if (log.Vehicle is not null)
		{
			ctx.Vehicles.Entry(log.Vehicle).State = EntityState.Unchanged;
		}
		ctx.VehicleRentalLogs.Add(log);
		await ctx.SaveChangesAsync().ConfigureAwait(false);
		return log.Id;
	}

	public async Task<int> UpdateVehicleRentalLogByIdAsync(VehicleRentalLog log)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		if (log.User is not null)
		{
			ctx.Users.Entry(log.User).State = EntityState.Unchanged;
		}
		if (log.Vehicle is not null)
		{
			ctx.Vehicles.Entry(log.Vehicle).State = EntityState.Unchanged;
		}
		ctx.VehicleRentalLogs.Update(log);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<int> CountRentalAsync(int vehicleId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.VehicleRentalLogs.CountAsync(e => e.Vehicle != null && e.Vehicle.Id == vehicleId);
	}

	public async Task<float> GetAverageRateAsync(int vehicleId)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return (float?)await ctx.VehicleRentalLogs.Where(e => e.Vehicle != null && e.Vehicle.Id == vehicleId).AverageAsync(e => e.Rate!) ?? 0f;
	}
}
