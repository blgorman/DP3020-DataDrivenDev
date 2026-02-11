using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class MigrateMyDatabase
{
    private readonly ILogger<MigrateMyDatabase> _logger;
    private readonly FunctionAppDbContext _db;

    public MigrateMyDatabase(ILogger<MigrateMyDatabase> logger, FunctionAppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [Function("MigrateMyDatabase")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("Migrating database");

        _db.Database.Migrate();

        return new OkObjectResult("Database migrated successfully!");
    }
}