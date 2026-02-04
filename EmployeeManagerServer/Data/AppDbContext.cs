using EmployeeManagerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerServer.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .HasMaxLength(255);

                entity.Property(e => e.MaritalStatus)
                      .HasColumnName("marital_status")
                      .HasMaxLength(100);

                entity.Property(e => e.Position)
                      .HasColumnName("position")
                      .HasMaxLength(255);

                entity.Property(e => e.Department)
                      .HasColumnName("department")
                      .HasMaxLength(255);

                entity.Property(e => e.Education)
                      .HasColumnName("education")
                      .HasMaxLength(255);

                entity.Property(e => e.Birthday)
                      .HasColumnName("birthday");

                entity.Property(e => e.StartDate)
                      .HasColumnName("start_date");

                entity.Property(e => e.FinishDate)
                      .HasColumnName("finish_date");

                entity.Property(e => e.PersonalPhoneNumber)
                      .HasColumnName("personal_phone_number")
                      .HasMaxLength(25);

                entity.Property(e => e.WorkPhoneNumber)
                      .HasColumnName("work_phone_number")
                      .HasMaxLength(25);

                entity.Property(e => e.PersonalEmail)
                      .HasColumnName("personal_email")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.HasIndex(e => e.PersonalEmail)
                      .IsUnique();

                entity.Property(e => e.WorkEmail)
                      .HasColumnName("work_email")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.HasIndex(e => e.WorkEmail)
                      .IsUnique();

                entity.Property(e => e.PersonalAddress)
                      .HasColumnName("personal_address")
                      .HasMaxLength(255);

                entity.Property(e => e.WorkAddress)
                      .HasColumnName("work_address")
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.EmployeeId)
                      .HasColumnName("employee_id")
                      .IsRequired();

                entity.HasOne(u => u.Employee)
                      .WithMany()
                      .HasForeignKey(u => u.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(u => u.Email)
                      .HasColumnName("email")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.PasswordHash)
                      .HasColumnName("password_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(u => u.AccessRights)
                      .HasColumnName("access_rights");

                entity.Property(u => u.CreatedAt)
                      .HasColumnName("created_at");
            });
        }
    }
}
