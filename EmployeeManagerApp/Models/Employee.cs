namespace EmployeeManagerApp.Models
{
    public class Employee : Person
    {
        private string _department { get; set; }
        private string _position { get; set; }
        private DateOnly _startDate { get; set; }
        private DateOnly? _finishDate { get; set; }
        private string _workPhoneNumber { get; set; } = string.Empty;
        private string _workEmail { get; set; } = string.Empty;
        private string _workAddress { get; set; }

        public string Department 
        { 
            get => _department; 
            set => _department = value;
        }

        public string Position 
        { 
            get => _position;
            set => _position = value;
        }

        public DateOnly StartDate 
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateOnly? FinishDate 
        {
            get => _finishDate;
            set => _finishDate = value;
        }

        public string WorkPhoneNumber
        {
            get => _workPhoneNumber;
            set => _workPhoneNumber = value;
        }

        public string WorkEmail
        {
            get => _workEmail;
            set => _workEmail = value;
        }

        public string WorkAddress
        {
            get => _workAddress;
            set => _workAddress = value;
        }

        /// <summary>
        /// Опыт работы в годах
        /// </summary>
        public float Experience 
        {
            get
            {
                var endDate = FinishDate ?? DateOnly.FromDateTime(DateTime.Today);

                if (endDate < StartDate)
                    return 0;

                int fullYears = endDate.Year - StartDate.Year;

                if (StartDate > endDate.AddYears(-fullYears))
                    fullYears--;

                // дробная часть
                var lastBirthday = StartDate.AddYears(fullYears);
                var daysInYear = DateTime.IsLeapYear(endDate.Year) ? 366 : 365;
                var extraDays = endDate.DayNumber - lastBirthday.DayNumber;

                return fullYears + (float)extraDays / daysInYear;
            }
        }
    }
}
