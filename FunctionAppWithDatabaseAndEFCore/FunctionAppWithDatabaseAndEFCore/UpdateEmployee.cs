using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class UpdateEmployee
{
    private readonly ILogger<UpdateEmployee> _logger;

    public UpdateEmployee(ILogger<UpdateEmployee> logger)
    {
        _logger = logger;
    }

    [Function("UpdateEmployee")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}