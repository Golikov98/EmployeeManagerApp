using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagerServer.Data;
using EmployeeManagerServer.Models;
using System;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/employees
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _db.Employees.ToListAsync();
        return Ok(employees);
    }

    // POST: api/employees
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    // GET: api/employees/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            return NotFound(new { message = "Сотрудник не найден" });

        return Ok(employee);
    }

    // PUT: api/employees/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(uint id, Employee updatedEmployee)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            return NotFound(new { message = "Сотрудник не найден" });

        // Обновляем все поля, кроме Id и CreatedAt
        employee.Name = updatedEmployee.Name;
        employee.MaritalStatus = updatedEmployee.MaritalStatus;
        employee.Position = updatedEmployee.Position;
        employee.Department = updatedEmployee.Department;
        employee.Education = updatedEmployee.Education;
        employee.Birthday = updatedEmployee.Birthday;
        employee.StartDate = updatedEmployee.StartDate;
        employee.FinishDate = updatedEmployee.FinishDate;
        employee.PersonalPhoneNumber = updatedEmployee.PersonalPhoneNumber;
        employee.WorkPhoneNumber = updatedEmployee.WorkPhoneNumber;
        employee.PersonalEmail = updatedEmployee.PersonalEmail;
        employee.WorkEmail = updatedEmployee.WorkEmail;
        employee.PersonalAddress = updatedEmployee.PersonalAddress;
        employee.WorkAddress = updatedEmployee.WorkAddress;

        await _db.SaveChangesAsync();
        return Ok(employee);
    }

    // DELETE: api/employees/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(uint id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            return NotFound(new { message = "Сотрудник не найден" });

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}