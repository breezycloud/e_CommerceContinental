using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IEmployeeService
{
    ValueTask<bool> CheckUsernameExist(string username);
    ValueTask<Employee[]> GetEmployees();    
    ValueTask<Employee[]> GetTailors();    
    ValueTask<Employee[]> GetEmployeeWithNoSalary();
    ValueTask<Employee[]> GetEmployeesExcludingMaster();
    ValueTask<Employee> GetEmployee(Guid id);
    ValueTask<int> GetTailorCurrentTask(Guid id);
    ValueTask<bool> AddEmployee(Employee employee);
    ValueTask<bool> CreateEmployeeAccount(EmployeeAccount account);
    ValueTask<bool> EditEmployee(Guid id, Employee employee);
    ValueTask<bool> DeleteEmployee(Guid id);
}