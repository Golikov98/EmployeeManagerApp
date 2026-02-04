namespace EmployeeManagerServer.Models
{
    public class Employee
    {
        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? Education { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? PersonalPhoneNumber { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string PersonalEmail { get; set; } = null!;
        public string WorkEmail { get; set; } = null!;
        public string? PersonalAddress { get; set; }
        public string? WorkAddress { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
