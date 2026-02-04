namespace EmployeeManagerServer.Models
{
    public class User
    {
        public uint Id { get; set; }
        public uint EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int? AccessRights { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
