using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppWithDatabaseAndEFCore;

public class DeleteEmployee
{
    private readonly ILogger<DeleteEmployee> _logger;

    public DeleteEmployee(ILogger<DeleteEmployee> logger)
    {
        _logger = logger;
    }

    [Function("DeleteEmployee")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}