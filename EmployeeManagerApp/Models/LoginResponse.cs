using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagerApp.Models
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public int AccessRights { get; set; }
        public Employee? Employee { get; set; }
    }
}
