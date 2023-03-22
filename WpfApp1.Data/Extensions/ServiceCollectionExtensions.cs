using System.Diagnostics;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data.Context;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterAppData(this IServiceCollection self)
	{
		var envDict = Env
			.NoClobber()
			.Load()
			.ToDictionary();


		var builder = new SqlConnectionStringBuilder
		{
			DataSource = Environment.GetEnvironmentVariable("WPFAPP_DATA_SOURCE") ?? string.Empty,
			InitialCatalog = Environment.GetEnvironmentVariable("WPFAPP_INITIAL_CATALOG") ?? string.Empty,
			UserID = Environment.GetEnvironmentVariable("WPFAPP_USER_ID") ?? string.Empty,
			Password = Environment.GetEnvironmentVariable("WPFAPP_PASSWORD") ?? string.Empty,
			IntegratedSecurity = true,
			Pooling = false,
			TrustServerCertificate = true
		};

		Debug.WriteLine(builder.ToString());
		return self.AddPooledDbContextFactory<AppDbContext>(options =>
		{
			options
				.UseSqlServer(builder.ToString())
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.EnableDetailedErrors()
#if DEBUG
				.EnableSensitiveDataLogging()
#endif
				;
		});
	}
}
