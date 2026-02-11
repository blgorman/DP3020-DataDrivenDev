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
    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        var result = await _db.Employees.SingleOrDefaultAsync(x => x.Id == employee.Id);
        
        if (result is null)
            return await CreateEmployee(employee);
        
        result.FirstName = employee.FirstName;
        result.LastName = employee.LastName;

        await _db.SaveChangesAsync();

        return result;
    }
    public async Task<bool> DeleteEmployee(Employee employee)
    {
        var result = await _db.Employees.SingleOrDefaultAsync(x => x.Id == employee.Id);
        
        if (result is null)
            return false;

        _db.Remove(result);

        await _db.SaveChangesAsync();

        return true;
    }
}