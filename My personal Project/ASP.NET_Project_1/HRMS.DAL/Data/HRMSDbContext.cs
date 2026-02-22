using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DAL.Data
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options)
            : base(options) { }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<SalaryStructure> SalaryStructure { get; set; }
        public DbSet<SalaryRevisions>SalaryRevisions { get; set; }
        public DbSet<Deductions> Deductions { get; set; }
        public DbSet<Bonuses> Bonuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Roles → Users (1-M)
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users → UserProfile (1-1)
            modelBuilder.Entity<Users>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraints
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<UserProfile>()
                .HasIndex(up => up.Email)
                .IsUnique();

            modelBuilder.Entity<Roles>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Users → Attendance (1-M)
            modelBuilder.Entity<Attendance>()
                 .HasOne(a => a.Users)           
                 .WithMany(u => u.Attendance)  
                 .HasForeignKey(a => a.UserId)  
                 .OnDelete(DeleteBehavior.Restrict);

            // Users → SalaryStructure (1-M)
            modelBuilder.Entity<SalaryStructure>()
                 .HasOne(a => a.Users)
                 .WithMany(u => u.SalaryStructure)
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            // Users → SalaryRevisions (1-M) on UserId-UserId
            modelBuilder.Entity<SalaryRevisions>()
                 .HasOne(a => a.Users)
                 .WithMany(u => u.SalaryRevisions)
                 .HasForeignKey(a => a.UserId) 
                 .OnDelete(DeleteBehavior.Restrict);

            // Users → SalaryRevisions (1-M) on ApprovedBy-UserId
            modelBuilder.Entity<SalaryRevisions>()
                    // Approver relationship
                    .HasOne(sr => sr.ApprovedByUser)
                    .WithMany(u => u.ApprovedSalaryRevisions)
                    .HasForeignKey(sr => sr.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);

            // Users → Deductions
            modelBuilder.Entity<Deductions>()
                 .HasOne(a => a.Users)
                 .WithMany(u => u.Deductions)
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            // Users → Bonuses
            modelBuilder.Entity<Bonuses>()
                 .HasOne(a => a.Users)
                 .WithMany(u => u.Bonuses)
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

        }
    }
}       