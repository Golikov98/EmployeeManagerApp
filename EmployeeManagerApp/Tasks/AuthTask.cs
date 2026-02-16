using EmployeeManagerApp.Api;
using EmployeeManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Tasks
{
    public class AuthTask
    {
        private readonly  IApiClient _apiClient;

        public AuthTask(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = await _apiClient.PostAsync<LoginRequest, LoginResponse>("auth/login", request);

            if (response is null)
            {
                throw new InvalidOperationException("Login response in empty");
            }

            return response;
        }
    }
}
