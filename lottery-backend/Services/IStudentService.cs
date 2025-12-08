using LotteryBackend.Models;

namespace LotteryBackend.Services;

/// <summary>
/// 学生服务接口
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// 获取所有学生
    /// </summary>
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    
    /// <summary>
    /// 根据ID获取学生
    /// </summary>
    Task<Student?> GetStudentByIdAsync(int id);
    
    /// <summary>
    /// 随机抽取学生
    /// </summary>
    Task<Student?> DrawStudentAsync(string? gender = null, string? className = null);
    
    /// <summary>
    /// 批量随机抽取学生
    /// </summary>
    Task<IEnumerable<Student>> DrawMultipleStudentsAsync(int count, string? gender = null, string? className = null, bool excludeDuplicates = true);
    
    /// <summary>
    /// 获取统计信息
    /// </summary>
    Task<object> GetStatisticsAsync();
    
    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    Task<IEnumerable<string>> GetClassListAsync();
    
    /// <summary>
    /// 保存抽签历史记录
    /// </summary>
    Task SaveDrawHistoryAsync(Student student, string? sessionId = null, bool isBatch = false, string? batchId = null);
    
    /// <summary>
    /// 获取抽签历史记录
    /// </summary>
    Task<IEnumerable<DrawHistory>> GetDrawHistoryAsync(string? sessionId = null, int limit = 100);
    
    /// <summary>
    /// 清空抽签历史记录
    /// </summary>
    Task ClearDrawHistoryAsync(string? sessionId = null);
    
    /// <summary>
    /// 删除单条历史记录
    /// </summary>
    Task DeleteDrawHistoryAsync(int historyId);
    
    // ========== 分组历史相关方法 ==========
    
    /// <summary>
    /// 保存分组历史记录
    /// </summary>
    Task SaveGroupingHistoryAsync(List<List<Student>> groups, int groupSize, string? sessionId = null);
    
    /// <summary>
    /// 获取分组历史记录
    /// </summary>
    Task<IEnumerable<GroupingHistory>> GetGroupingHistoryAsync(string? sessionId = null, int limit = 10);
    
    /// <summary>
    /// 清空分组历史记录
    /// </summary>
    Task ClearGroupingHistoryAsync(string? sessionId = null);
    
    /// <summary>
    /// 删除指定批次的分组历史
    /// </summary>
    Task DeleteGroupingHistoryByBatchIdAsync(string batchId);
}

