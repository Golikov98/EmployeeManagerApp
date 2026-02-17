using EmployeeManagerApp.Api;
using EmployeeManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Tasks
{
    public class EmployeesTask
    {

        private readonly IApiClient _apiClient;

        public EmployeesTask(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Запрашивает список сотрудников с сервера
        /// </summary>
        public async Task<List<Employee>> getAllEmployeesAsync()
        {
            return await _apiClient.GetAsync<List<Employee>>("employees");
        }
    }
}
