using Microsoft.EntityFrameworkCore;
using LotteryBackend.Models;

namespace LotteryBackend.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<DrawHistory> DrawHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 学生表配置
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Student>().Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Student>().Property(s => s.SerialNumber).HasColumnName("serial_number");
            modelBuilder.Entity<Student>().Property(s => s.StudentId).HasColumnName("student_id");
            modelBuilder.Entity<Student>().Property(s => s.Name).HasColumnName("name");
            modelBuilder.Entity<Student>().Property(s => s.Gender).HasColumnName("gender");
            modelBuilder.Entity<Student>().Property(s => s.Major).HasColumnName("major");
            modelBuilder.Entity<Student>().Property(s => s.Class).HasColumnName("class");

            // 抽签历史表配置
            modelBuilder.Entity<DrawHistory>().ToTable("draw_history");
            modelBuilder.Entity<DrawHistory>().Property(h => h.Id).HasColumnName("id");
            modelBuilder.Entity<DrawHistory>().Property(h => h.StudentId).HasColumnName("student_id");
            modelBuilder.Entity<DrawHistory>().Property(h => h.StudentName).HasColumnName("student_name");
            modelBuilder.Entity<DrawHistory>().Property(h => h.StudentNumber).HasColumnName("student_number");
            modelBuilder.Entity<DrawHistory>().Property(h => h.Class).HasColumnName("class");
            modelBuilder.Entity<DrawHistory>().Property(h => h.Gender).HasColumnName("gender");
            modelBuilder.Entity<DrawHistory>().Property(h => h.DrawTime).HasColumnName("draw_time");
            modelBuilder.Entity<DrawHistory>().Property(h => h.SessionId).HasColumnName("session_id");
            modelBuilder.Entity<DrawHistory>().Property(h => h.IsBatch).HasColumnName("is_batch");
            modelBuilder.Entity<DrawHistory>().Property(h => h.BatchId).HasColumnName("batch_id");
        }
    }
}
