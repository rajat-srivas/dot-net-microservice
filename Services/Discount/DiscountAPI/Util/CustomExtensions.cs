using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Threading;

namespace DiscountAPI.Util
{
	public static class CustomExtensions
	{
		public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
		{
			int retryForAvailability = retry.Value;
			using(var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var configuration = services.GetRequiredService<IConfiguration>();
				try
				{
					using var _connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
					_connection.Open();

					using var command = new NpgsqlCommand
					{
						Connection = _connection
					};
					command.CommandText = "DROP TABLE IF EXISTS Coupon";
					command.ExecuteNonQuery();

					command.CommandText = @"CREATE TABLE COUPON(ID SERIAL PRIMARY KEY NOT NULL,
											CouponCode VARCHAR(24) NOT NULL UNIQUE,
											Description TEXT,
											Amount INT,
											Status VARCHAR(24));";
					command.ExecuteNonQuery();

					command.CommandText = @"INSERT into coupon (CouponCode, Description, Amount, Status) 
											Values('2fd6e50e-358a-4acf', 'Get flat 150 of on cart value', 150, 'A');
											INSERT into coupon (CouponCode, Description, Amount,Status) 
											Values('b5f8-77a39338f967', 'Upto 100 off on cart value', 100, 'A');";
					command.ExecuteNonQuery();

				}
				catch(NpgsqlException ex)
				{
					if(retryForAvailability < 3)
					{
						retryForAvailability++;
						Thread.Sleep(2000);
						MigrateDatabase<TContext>(host, retryForAvailability);
					}
				}
			}
			return host;
		}
	}
}
