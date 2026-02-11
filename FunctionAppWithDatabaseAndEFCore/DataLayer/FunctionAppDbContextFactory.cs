using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataLayer;

public class FunctionAppDbContextFactory : IDesignTimeDbContextFactory<FunctionAppDbContext>
{
    public FunctionAppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .Build();

        var connectionString = configuration["SqlConnectionString"];

        var optionsBuilder = new DbContextOptionsBuilder<FunctionAppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new FunctionAppDbContext(optionsBuilder.Options);
    }
}
