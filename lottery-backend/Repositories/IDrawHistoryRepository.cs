using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 抽签历史仓储接口
/// </summary>
public interface IDrawHistoryRepository : IRepository<DrawHistory>
{
    /// <summary>
    /// 根据会话ID获取历史记录
    /// </summary>
    Task<IEnumerable<DrawHistory>> GetBySessionIdAsync(string sessionId, int limit = 100);
    
    /// <summary>
    /// 获取最近的历史记录
    /// </summary>
    Task<IEnumerable<DrawHistory>> GetRecentHistoryAsync(int limit = 100);
    
    /// <summary>
    /// 清空指定会话的历史记录
    /// </summary>
    Task ClearBySessionIdAsync(string sessionId);
    
    /// <summary>
    /// 清空所有历史记录
    /// </summary>
    Task ClearAllAsync();
    
    /// <summary>
    /// 根据学生ID获取历史记录
    /// </summary>
    Task<IEnumerable<DrawHistory>> GetByStudentIdAsync(int studentId);
}
