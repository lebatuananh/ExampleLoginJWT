using System;
using System.IO;
using ExampleLoginJWT.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExampleLoginJWT.WebAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ExampleLoginJwtDbContext>
    {
        public ExampleLoginJwtDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ExampleLoginJwtDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ExampleLoginJwtDbContext(builder.Options);
        }
    }
}