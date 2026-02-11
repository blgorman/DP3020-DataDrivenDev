using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServiceLayer;

namespace FunctionAppWithDatabaseAndEFCore;

public class GetEmployees
{
    private readonly EmployeeService _employeeService;
    private readonly ILogger<GetEmployees> _logger;

    public GetEmployees(ILogger<GetEmployees> logger, EmployeeService serviceLayer)
    {
        _logger = logger;
        _employeeService = serviceLayer;
    }

    [Function("GetEmployees")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        var employees = await _employeeService.GetEmployees();
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(employees);
    }
}