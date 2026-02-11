using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using ServiceLayer;
using System.Text.Json;

namespace FunctionAppWithDatabaseAndEFCore;

public class UpdateEmployee
{
    private readonly ILogger<UpdateEmployee> _logger;
    private readonly EmployeeService _employeeService;
    public UpdateEmployee(ILogger<UpdateEmployee> logger, EmployeeService employeeService)
    {
        _logger = logger;
        _employeeService = employeeService;
    }

    [Function("UpdateEmployee")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
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
        catch
        {
            return new BadRequestObjectResult("Invalid JSON format. Please provide valid employee data.");
        }

        if (employee is null)
            return new BadRequestObjectResult("Please provide valid employee data.");

        var result = await _employeeService.UpdateEmployee(employee);

        _logger.LogInformation("Updated employee with id {EmployeeId}", employee.Id);
        return new OkObjectResult(result);
    }
}