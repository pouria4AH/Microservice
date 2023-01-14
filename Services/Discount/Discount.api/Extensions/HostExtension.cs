using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("migration posgtresql database");

                    using var connection = new NpgsqlConnection(
                        configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    // seed data
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon(Id  SERIAL PRIMARY KEY,
                                                                            ProductName VARCHAR(200) NOT NULL,
                                                                            Description TEXT,
                                                                            Amount INT);";
                    command.ExecuteNonQuery();
                    command.CommandText =
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('IPhone x', 'iphone discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung 10', 'samsung discount', 150);";
                    command.ExecuteNonQuery();

                    logger.LogError("migration has been completed ! ");
                }

                catch (NpgsqlException ex)
                {
                    logger.LogError("an error has been occured ");
                    if (retryForAvailability < 50)
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
