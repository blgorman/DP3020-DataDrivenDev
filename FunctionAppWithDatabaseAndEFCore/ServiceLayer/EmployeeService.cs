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
        return await _db.Employees.ToListAsync();
    }

    public async Task<Employee> CreateEmployee(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return employee;
    }
}
