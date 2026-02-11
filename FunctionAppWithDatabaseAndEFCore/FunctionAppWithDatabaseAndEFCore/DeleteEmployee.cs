using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using ServiceLayer;
using System.Text.Json;

namespace FunctionAppWithDatabaseAndEFCore;

public class DeleteEmployee
{
    private readonly ILogger<DeleteEmployee> _logger;
    private readonly EmployeeService _employeeService;
    public DeleteEmployee(ILogger<DeleteEmployee> logger, EmployeeService employeeService)
    {
        _logger = logger;
        _employeeService = employeeService;
    }

    [Function("DeleteEmployee")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
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

        bool result = await _employeeService.DeleteEmployee(employee);

        _logger.LogInformation("Deleted employee with id {EmployeeId}", employee.Id);
        return new OkObjectResult(result);
    }
}