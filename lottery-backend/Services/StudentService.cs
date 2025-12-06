using Dapper;
using LotteryBackend.Models;
using Microsoft.Data.Sqlite;

namespace LotteryBackend.Services;

/// <summary>
/// 学生服务实现
/// </summary>
public class StudentService : IStudentService
{
    private readonly string _connectionString;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IConfiguration configuration, ILogger<StudentService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("数据库连接字符串未配置");
        _logger = logger;
    }

    /// <summary>
    /// 获取数据库连接
    /// </summary>
    private SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    /// <summary>
    /// 获取所有学生
    /// </summary>
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        using var connection = GetConnection();
        const string sql = @"
            SELECT id as Id, serial_number as SerialNumber, student_id as StudentId, 
                   name as Name, gender as Gender, major as Major, class as Class
            FROM students
            ORDER BY id";
        
        _logger.LogInformation("查询所有学生");
        return await connection.QueryAsync<Student>(sql);
    }

    /// <summary>
    /// 根据ID获取学生
    /// </summary>
    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        using var connection = GetConnection();
        const string sql = @"
            SELECT id as Id, serial_number as SerialNumber, student_id as StudentId, 
                   name as Name, gender as Gender, major as Major, class as Class
            FROM students
            WHERE id = @Id";
        
        _logger.LogInformation("查询学生ID: {Id}", id);
        return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
    }

    /// <summary>
    /// 随机抽取学生
    /// </summary>
    public async Task<Student?> DrawStudentAsync(string? gender = null, string? className = null)
    {
        using var connection = GetConnection();
        
        // 构建SQL查询
        var sql = @"
            SELECT id as Id, serial_number as SerialNumber, student_id as StudentId, 
                   name as Name, gender as Gender, major as Major, class as Class
            FROM students
            WHERE 1=1";
        
        var parameters = new DynamicParameters();
        
        if (!string.IsNullOrEmpty(gender))
        {
            sql += " AND gender = @Gender";
            parameters.Add("Gender", gender);
        }
        
        if (!string.IsNullOrEmpty(className))
        {
            sql += " AND class = @Class";
            parameters.Add("Class", className);
        }
        
        sql += " ORDER BY RANDOM() LIMIT 1";
        
        _logger.LogInformation("抽签 - 性别: {Gender}, 班级: {Class}", gender ?? "全部", className ?? "全部");
        return await connection.QueryFirstOrDefaultAsync<Student>(sql, parameters);
    }

    /// <summary>
    /// 批量随机抽取学生
    /// </summary>
    public async Task<IEnumerable<Student>> DrawMultipleStudentsAsync(int count, string? gender = null, string? className = null, bool excludeDuplicates = true)
    {
        using var connection = GetConnection();
        
        // 构建SQL查询
        var sql = @"
            SELECT id as Id, serial_number as SerialNumber, student_id as StudentId, 
                   name as Name, gender as Gender, major as Major, class as Class
            FROM students
            WHERE 1=1";
        
        var parameters = new DynamicParameters();
        
        if (!string.IsNullOrEmpty(gender))
        {
            sql += " AND gender = @Gender";
            parameters.Add("Gender", gender);
        }
        
        if (!string.IsNullOrEmpty(className))
        {
            sql += " AND class = @Class";
            parameters.Add("Class", className);
        }
        
        sql += " ORDER BY RANDOM() LIMIT @Count";
        parameters.Add("Count", count);
        
        _logger.LogInformation("批量抽签 - 数量: {Count}, 性别: {Gender}, 班级: {Class}", 
            count, gender ?? "全部", className ?? "全部");
        
        return await connection.QueryAsync<Student>(sql, parameters);
    }

    /// <summary>
    /// 获取统计信息
    /// </summary>
    public async Task<object> GetStatisticsAsync()
    {
        using var connection = GetConnection();
        
        // 获取总数
        const string totalSql = "SELECT COUNT(*) FROM students";
        var total = await connection.ExecuteScalarAsync<int>(totalSql);
        
        // 获取性别统计
        const string genderSql = @"
            SELECT gender as Gender, COUNT(*) as Count
            FROM students
            WHERE gender IS NOT NULL
            GROUP BY gender";
        var genderStats = await connection.QueryAsync<dynamic>(genderSql);
        
        // 获取班级统计
        const string classSql = @"
            SELECT class as Class, COUNT(*) as Count
            FROM students
            WHERE class IS NOT NULL
            GROUP BY class
            ORDER BY class";
        var classStats = await connection.QueryAsync<dynamic>(classSql);
        
        _logger.LogInformation("获取统计信息 - 总数: {Total}", total);
        
        return new
        {
            Total = total,
            GenderStats = genderStats,
            ClassStats = classStats
        };
    }

    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    public async Task<IEnumerable<string>> GetClassListAsync()
    {
        using var connection = GetConnection();
        const string sql = @"
            SELECT DISTINCT class
            FROM students
            WHERE class IS NOT NULL
            ORDER BY class";
        
        _logger.LogInformation("获取班级列表");
        return await connection.QueryAsync<string>(sql);
    }
}
