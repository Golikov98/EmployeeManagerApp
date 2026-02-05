namespace EmployeeManagerApp.Models
{
    internal class User: Employee
    {
        private string _password;
        private int _accessRights;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public int AccessRights
        {
            get { return _accessRights; }
            set { _accessRights = value; }
        }
    }
}
