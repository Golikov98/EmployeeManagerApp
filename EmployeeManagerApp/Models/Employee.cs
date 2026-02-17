namespace EmployeeManagerApp.Models
{
    public class Employee : Person
    {
        private int _id {  get; set; }
        private string _department { get; set; }
        private string _position { get; set; }
        private DateTime _startDate { get; set; }
        private DateTime? _finishDate { get; set; }
        private string _workPhoneNumber { get; set; } = string.Empty;
        private string _workEmail { get; set; } = string.Empty;
        private string _workAddress { get; set; }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

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

        public DateTime StartDate 
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateTime? FinishDate 
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
                if (StartDate == DateTime.MinValue)
                    return 0;

                var endDate = FinishDate ?? DateTime.Today;

                if (endDate < StartDate)
                    return 0;

                int fullYears = endDate.Year - StartDate.Year;

                if (endDate.Month < StartDate.Month ||
                    (endDate.Month == StartDate.Month && endDate.Day < StartDate.Day))
                {
                    fullYears--;
                }

                var lastBirthday = StartDate.AddYears(fullYears);
                double extraDays = (endDate - lastBirthday).TotalDays;
                int daysInYear = DateTime.IsLeapYear(lastBirthday.Year) ? 366 : 365;

                return fullYears + (float)(extraDays / daysInYear);
            }
        }
    }
}
