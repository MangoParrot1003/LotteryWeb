using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 学生仓储实现
/// </summary>
public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(StudentContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Student>> GetStudentsByFilterAsync(string? gender = null, string? className = null)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(gender))
        {
            query = query.Where(s => s.Gender == gender);
        }

        if (!string.IsNullOrEmpty(className))
        {
            query = query.Where(s => s.Class == className);
        }

        return await query.ToListAsync();
    }

    public async Task<Student?> GetRandomStudentAsync(string? gender = null, string? className = null)
    {
        var students = await GetStudentsByFilterAsync(gender, className);
        var studentList = students.ToList();
        
        if (!studentList.Any())
        {
            return null;
        }

        var random = new Random();
        var index = random.Next(studentList.Count);
        var selectedStudent = studentList[index];
        
        // 调试日志：确保返回的学生数据正确
        Console.WriteLine($"[DEBUG] 抽中学生: ID={selectedStudent.Id}, Name='{selectedStudent.Name}', StudentId='{selectedStudent.StudentId}'");
        
        return selectedStudent;
    }

    public async Task<IEnumerable<Student>> GetRandomStudentsAsync(int count, string? gender = null, string? className = null)
    {
        var students = await GetStudentsByFilterAsync(gender, className);
        var studentList = students.ToList();
        
        if (!studentList.Any())
        {
            return new List<Student>();
        }

        var random = new Random();
        return studentList.OrderBy(x => random.Next()).Take(count).ToList();
    }

    public async Task<IEnumerable<string>> GetClassListAsync()
    {
        return await _dbSet
            .Where(s => s.Class != null)
            .Select(s => s.Class!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<object> GetStatisticsAsync()
    {
        var total = await _dbSet.CountAsync();
        
        var genderStats = await _dbSet
            .Where(s => s.Gender != null)
            .GroupBy(s => s.Gender)
            .Select(g => new { Gender = g.Key, Count = g.Count() })
            .ToListAsync();
        
        var classStats = await _dbSet
            .Where(s => s.Class != null)
            .GroupBy(s => s.Class)
            .Select(g => new { Class = g.Key, Count = g.Count() })
            .OrderBy(c => c.Class)
            .ToListAsync();
        
        return new
        {
            Total = total,
            GenderStats = genderStats,
            ClassStats = classStats
        };
    }
}
