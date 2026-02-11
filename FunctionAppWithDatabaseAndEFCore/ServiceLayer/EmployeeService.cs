using DataLayer;
using Microsoft.EntityFrameworkCore;
using Models;

namespace ServiceLayer;

public class EmployeeService
{
    private readonly FunctionAppDbContext _db;
    public EmployeeService(FunctionAppDbContext context)
    { 
        _db = context;
    }

    public async Task<List<Employee>> GetEmployees()
    {
        // In a real application, this would likely involve database access.
        //TODO: Call data layer
        return await _db.Employees.ToListAsync();
    }
}
