using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WpfApp.Data.Context;
using WpfApp.Data.DAOs;
using WpfApp.Data.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterSql(this IServiceCollection self)
	{
		return self
			.RegisterServices()
			.AddSingleton<IDbServiceOptions>(provider => new DbServiceOptions(GetSqlConnectionStringBuilder()))
			.AddSingleton<IDbService, DbService>()
			.AddSingleton<IUserDAO, SqlUserDAO>()
			.AddSingleton<IVehicleDAO, SqlVehicleDAO>();
	}

	public static IServiceCollection RegisterEntityFramework(this IServiceCollection self)
	{
		return self
			.RegisterServices()
			.AddPooledDbContextFactory<AppDbContext>(options =>
		{
			options
				.UseSqlServer(GetSqlConnectionStringBuilder().ToString())
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.EnableDetailedErrors()
#if DEBUG
				.EnableSensitiveDataLogging()
#endif
				;
		});
	}

	private static IServiceCollection RegisterServices(this IServiceCollection self)
	{
		return self
			.AddSingleton<IFluentDbReaderFactory, FluentDbReaderFactory>();
	}

	private static SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
	{
		Env
			.NoClobber()
			.Load();

		return new SqlConnectionStringBuilder
		{
			DataSource = Environment.GetEnvironmentVariable("WPFAPP_DATA_SOURCE") ?? string.Empty,
			InitialCatalog = Environment.GetEnvironmentVariable("WPFAPP_INITIAL_CATALOG") ?? string.Empty,
			UserID = Environment.GetEnvironmentVariable("WPFAPP_USER_ID") ?? string.Empty,
			Password = Environment.GetEnvironmentVariable("WPFAPP_PASSWORD") ?? string.Empty,
			IntegratedSecurity = true,
			Pooling = false,
			TrustServerCertificate = true
		};
	}
}
