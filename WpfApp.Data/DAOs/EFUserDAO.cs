using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Context;
using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public class EFUserDAO : IUserDAO
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public EFUserDAO(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
	}

	public async Task<int> AddAsync(User model)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		model.Password = await Task.Run(() => BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password)).ConfigureAwait(false);
		ctx.Users.Add(model);
		await ctx.SaveChangesAsync().ConfigureAwait(false);
		return model.Id;
	}

	public async Task<IReadOnlyCollection<User>> GetAllAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Users.ToArrayAsync().ConfigureAwait(false);
	}

	public async Task<int> UpdateAsync(User model)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Users.Update(model);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<User?> GetByIdAsync(int id)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Users
			.Where(e => e.Id == id)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
	}

	public async Task<User?> GetByFullNameAsync(string fullName)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Users
			.Where(e => e.FullName == fullName)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> DeleteByIdAsync(int id)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Users
			.Where(e => e.Id == id)
			.Take(1)
			.ExecuteDeleteAsync()
			.ConfigureAwait(false);
	}
}
