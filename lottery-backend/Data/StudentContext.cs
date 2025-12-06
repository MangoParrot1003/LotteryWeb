using Microsoft.EntityFrameworkCore;
using LotteryBackend.Models;

namespace LotteryBackend.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Student>().Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Student>().Property(s => s.SerialNumber).HasColumnName("serial_number");
            modelBuilder.Entity<Student>().Property(s => s.StudentId).HasColumnName("student_id");
            modelBuilder.Entity<Student>().Property(s => s.Name).HasColumnName("name");
            modelBuilder.Entity<Student>().Property(s => s.Gender).HasColumnName("gender");
            modelBuilder.Entity<Student>().Property(s => s.Major).HasColumnName("major");
            modelBuilder.Entity<Student>().Property(s => s.Class).HasColumnName("class");
        }
    }
}
