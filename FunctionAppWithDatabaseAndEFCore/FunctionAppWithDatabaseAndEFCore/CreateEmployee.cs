using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using ServiceLayer;
using System.Text.Json;

namespace FunctionAppWithDatabaseAndEFCore;

public class CreateEmployee
{
    private readonly EmployeeService _employeeService;
    private readonly ILogger<GetEmployees> _logger;

    public CreateEmployee(ILogger<GetEmployees> logger, EmployeeService serviceLayer)
    {
        _logger = logger;
        _employeeService = serviceLayer;
    }

    [Function("CreateEmployee")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        if (req.ContentLength is null or 0)
        {
            return new BadRequestObjectResult("Please provide employee data in the request body.");
        }

        Employee? employee;
        try
        {
            employee = await JsonSerializer.DeserializeAsync<Employee>(req.Body, cancellationToken: req.HttpContext.RequestAborted);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult("Invalid JSON format. Please provide valid employee data.");
        }

        if (employee is null)
        {
            return new BadRequestObjectResult("Please provide valid employee data.");
        }

        await _employeeService.CreateEmployee(employee);

        return new OkObjectResult(employee);
    }
}