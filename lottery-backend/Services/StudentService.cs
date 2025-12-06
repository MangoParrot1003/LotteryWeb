using LotteryBackend.Models;
using LotteryBackend.Repositories;

namespace LotteryBackend.Services;

/// <summary>
/// 学生服务实现
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDrawHistoryRepository _historyRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository studentRepository,
        IDrawHistoryRepository historyRepository,
        ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _historyRepository = historyRepository;
        _logger = logger;
    }

    /// <summary>
    /// 获取所有学生
    /// </summary>
    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        _logger.LogInformation("查询所有学生");
        return await _studentRepository.GetAllAsync();
    }

    /// <summary>
    /// 根据ID获取学生
    /// </summary>
    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        _logger.LogInformation("查询学生ID: {Id}", id);
        return await _studentRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// 随机抽取学生
    /// </summary>
    public async Task<Student?> DrawStudentAsync(string? gender = null, string? className = null)
    {
        _logger.LogInformation("抽签 - 性别: {Gender}, 班级: {Class}", gender ?? "全部", className ?? "全部");
        return await _studentRepository.GetRandomStudentAsync(gender, className);
    }

    /// <summary>
    /// 批量随机抽取学生
    /// </summary>
    public async Task<IEnumerable<Student>> DrawMultipleStudentsAsync(int count, string? gender = null, string? className = null, bool excludeDuplicates = true)
    {
        _logger.LogInformation("批量抽签 - 数量: {Count}, 性别: {Gender}, 班级: {Class}", 
            count, gender ?? "全部", className ?? "全部");
        
        return await _studentRepository.GetRandomStudentsAsync(count, gender, className);
    }

    /// <summary>
    /// 获取统计信息
    /// </summary>
    public async Task<object> GetStatisticsAsync()
    {
        _logger.LogInformation("获取统计信息");
        return await _studentRepository.GetStatisticsAsync();
    }

    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    public async Task<IEnumerable<string>> GetClassListAsync()
    {
        _logger.LogInformation("获取班级列表");
        return await _studentRepository.GetClassListAsync();
    }

    /// <summary>
    /// 保存抽签历史记录
    /// </summary>
    public async Task SaveDrawHistoryAsync(Student student, string? sessionId = null, bool isBatch = false, string? batchId = null)
    {
        var history = new DrawHistory
        {
            StudentId = student.Id,
            StudentName = student.Name,
            StudentNumber = student.StudentId,
            Class = student.Class,
            Gender = student.Gender,
            DrawTime = DateTime.Now,
            SessionId = sessionId,
            IsBatch = isBatch,
            BatchId = batchId
        };
        
        await _historyRepository.AddAsync(history);
        await _historyRepository.SaveChangesAsync();
        
        _logger.LogInformation("保存抽签历史 - 学生: {Name}, 会话: {SessionId}", student.Name, sessionId ?? "默认");
    }

    /// <summary>
    /// 获取抽签历史记录
    /// </summary>
    public async Task<IEnumerable<DrawHistory>> GetDrawHistoryAsync(string? sessionId = null, int limit = 100)
    {
        _logger.LogInformation("获取抽签历史 - 会话: {SessionId}, 限制: {Limit}", sessionId ?? "全部", limit);
        
        if (!string.IsNullOrEmpty(sessionId))
        {
            return await _historyRepository.GetBySessionIdAsync(sessionId, limit);
        }
        
        return await _historyRepository.GetRecentHistoryAsync(limit);
    }

    /// <summary>
    /// 清空抽签历史记录
    /// </summary>
    public async Task ClearDrawHistoryAsync(string? sessionId = null)
    {
        if (!string.IsNullOrEmpty(sessionId))
        {
            await _historyRepository.ClearBySessionIdAsync(sessionId);
            _logger.LogInformation("清空抽签历史 - 会话: {SessionId}", sessionId);
        }
        else
        {
            await _historyRepository.ClearAllAsync();
            _logger.LogInformation("清空所有抽签历史");
        }
    }

    /// <summary>
    /// 删除单条历史记录
    /// </summary>
    public async Task DeleteDrawHistoryAsync(int historyId)
    {
        await _historyRepository.DeleteAsync(historyId);
        await _historyRepository.SaveChangesAsync();
        
        _logger.LogInformation("删除抽签历史记录 - ID: {Id}", historyId);
    }
}
