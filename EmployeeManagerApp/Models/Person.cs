namespace EmployeeManagerApp.Models
{
    public class Person
    {
        private string _name { get; set; }
        private DateOnly _birthday { get; set; }
        private string _maritalStatus { get; set; }
        private string _personalPhoneNumber { get; set; } = string.Empty;
        private string _personalEmail { get; set; } = string.Empty;
        private string _personalAddress { get; set; }
        private string _education { get; set; }

        public string Name 
        { 
            get => _name; 
            set => _name = value;
        }

        public DateOnly Birthday 
        {
            get => _birthday; 
            set => _birthday = value; 
        }

        public string MaritalStatus 
        {
            get => _maritalStatus; 
            set => _maritalStatus = value; 
        }
        public string PersonalPhoneNumber
        {
            get => _personalPhoneNumber;
            set => _personalPhoneNumber = value;
        }

        public string PersonalEmail
        {
            get => _personalEmail;
            set => _personalEmail = value;
        }

        public string PersonalAdress
        {
            get => _personalAddress;
            set => _personalAddress = value;
        }

        public string Education
        {
            get => _education;
            set => _education = value;
        }

        public int Age 
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                int age = today.Year - _birthday.Year;

                if (_birthday > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}
