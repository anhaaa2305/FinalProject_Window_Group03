using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Constants;
using WpfApp.Data.Context;
using WpfApp.Data.Models;

namespace WpfApp.Data.DAOs;

public class EFRoleDAO : IRoleDAO
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public EFRoleDAO(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
	}

	public async Task<int> AddAsync(Role role)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Roles.Add(role);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<IReadOnlyCollection<Role>> GetAllAsync()
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Roles.ToArrayAsync().ConfigureAwait(false);
	}

	public async Task<int> UpdateAsync(Role role)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		ctx.Roles.Update(role);
		return await ctx.SaveChangesAsync().ConfigureAwait(false);
	}

	public async Task<Role?> GetByFlagAsync(RoleFlag flag)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Roles
			.Where(e => e.Flag == flag)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> DeleteByFlagAsync(RoleFlag flag)
	{
		using var ctx = dbContextFactory.CreateDbContext();
		return await ctx.Roles
			.Where(e => e.Flag == flag)
			.Take(1)
			.ExecuteDeleteAsync()
			.ConfigureAwait(false);
	}
}
