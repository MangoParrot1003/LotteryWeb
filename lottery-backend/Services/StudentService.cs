using LotteryBackend.Models;
using LotteryBackend.Repositories;
using System.Text.Json;

namespace LotteryBackend.Services;

/// <summary>
/// 学生服务实现
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDrawHistoryRepository _historyRepository;
    private readonly IGroupingHistoryRepository _groupingHistoryRepository;
    private readonly IPrizeDrawHistoryRepository _prizeHistoryRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository studentRepository,
        IDrawHistoryRepository historyRepository,
        IGroupingHistoryRepository groupingHistoryRepository,
        IPrizeDrawHistoryRepository prizeHistoryRepository,
        ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _historyRepository = historyRepository;
        _groupingHistoryRepository = groupingHistoryRepository;
        _prizeHistoryRepository = prizeHistoryRepository;
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
        var student = await _studentRepository.GetRandomStudentAsync(gender, className);
        
        if (student != null)
        {
            _logger.LogInformation("抽中学生: ID={Id}, Name='{Name}', StudentId='{StudentId}'", 
                student.Id, student.Name, student.StudentId);
        }
        
        return student;
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

    // ========== 分组历史相关方法 ==========

    /// <summary>
    /// 保存分组历史记录
    /// </summary>
    public async Task SaveGroupingHistoryAsync(List<List<Student>> groups, int groupSize, string? sessionId = null)
    {
        var batchId = Guid.NewGuid().ToString();
        var groupTime = DateTime.Now;
        var totalGroups = groups.Count;

        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var membersJson = JsonSerializer.Serialize(group.Select(s => new
            {
                s.Id,
                s.StudentId,
                s.Name,
                s.Gender,
                s.Class,
                s.Major
            }));

            var history = new GroupingHistory
            {
                BatchId = batchId,
                GroupNumber = i + 1,
                GroupSize = groupSize,
                TotalGroups = totalGroups,
                Members = membersJson,
                GroupTime = groupTime,
                SessionId = sessionId
            };

            await _groupingHistoryRepository.AddAsync(history);
        }

        await _groupingHistoryRepository.SaveChangesAsync();
        _logger.LogInformation("保存分组历史 - 批次: {BatchId}, 总组数: {TotalGroups}", batchId, totalGroups);
    }

    /// <summary>
    /// 获取分组历史记录
    /// </summary>
    public async Task<IEnumerable<GroupingHistory>> GetGroupingHistoryAsync(string? sessionId = null, int limit = 10)
    {
        _logger.LogInformation("获取分组历史 - 会话: {SessionId}, 限制: {Limit}", sessionId ?? "全部", limit);

        if (!string.IsNullOrEmpty(sessionId))
        {
            return await _groupingHistoryRepository.GetBySessionIdAsync(sessionId, limit);
        }

        return await _groupingHistoryRepository.GetRecentHistoryAsync(limit);
    }

    /// <summary>
    /// 清空分组历史记录
    /// </summary>
    public async Task ClearGroupingHistoryAsync(string? sessionId = null)
    {
        if (!string.IsNullOrEmpty(sessionId))
        {
            await _groupingHistoryRepository.ClearBySessionIdAsync(sessionId);
            _logger.LogInformation("清空分组历史 - 会话: {SessionId}", sessionId);
        }
        else
        {
            await _groupingHistoryRepository.ClearAllAsync();
            _logger.LogInformation("清空所有分组历史");
        }
    }

    /// <summary>
    /// 删除指定批次的分组历史
    /// </summary>
    public async Task DeleteGroupingHistoryByBatchIdAsync(string batchId)
    {
        await _groupingHistoryRepository.DeleteByBatchIdAsync(batchId);
        _logger.LogInformation("删除分组历史 - 批次: {BatchId}", batchId);
    }

    // ========== 抽奖历史相关方法 ==========

    /// <summary>
    /// 保存抽奖历史记录
    /// </summary>
    public async Task SavePrizeDrawHistoryAsync(string prizeName, IEnumerable<Student> winners, string? sessionId = null, string? batchId = null)
    {
        var winnersList = winners.ToList();
        var history = new PrizeDrawHistory
        {
            PrizeName = prizeName,
            WinnersList = winnersList,
            DrawTime = DateTime.Now,
            SessionId = sessionId,
            BatchId = batchId
        };

        await _prizeHistoryRepository.AddAsync(history);
        _logger.LogInformation("保存抽奖历史 - 奖品: {PrizeName}, 中奖人数: {Count}, 批次: {BatchId}", 
            prizeName, winnersList.Count, batchId ?? "单独");
    }

    /// <summary>
    /// 获取抽奖历史记录
    /// </summary>
    public async Task<IEnumerable<PrizeDrawHistory>> GetPrizeHistoryAsync(string? sessionId = null, int limit = 10)
    {
        _logger.LogInformation("获取抽奖历史 - 会话: {SessionId}, 限制: {Limit}", sessionId ?? "全部", limit);
        return await _prizeHistoryRepository.GetAllAsync(sessionId, limit);
    }

    /// <summary>
    /// 清空抽奖历史记录
    /// </summary>
    public async Task ClearPrizeHistoryAsync(string? sessionId = null)
    {
        await _prizeHistoryRepository.ClearAsync(sessionId);
        
        if (!string.IsNullOrEmpty(sessionId))
        {
            _logger.LogInformation("清空抽奖历史 - 会话: {SessionId}", sessionId);
        }
        else
        {
            _logger.LogInformation("清空所有抽奖历史");
        }
    }

    /// <summary>
    /// 删除单条抽奖历史记录
    /// </summary>
    public async Task DeletePrizeHistoryAsync(int historyId)
    {
        await _prizeHistoryRepository.DeleteAsync(historyId);
        _logger.LogInformation("删除抽奖历史记录 - ID: {Id}", historyId);
    }
}

