using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 分组历史仓储接口
/// </summary>
public interface IGroupingHistoryRepository : IRepository<GroupingHistory>
{
    /// <summary>
    /// 根据批次ID获取分组历史记录
    /// </summary>
    Task<IEnumerable<GroupingHistory>> GetByBatchIdAsync(string batchId);
    
    /// <summary>
    /// 获取最近的分组历史记录（按批次分组）
    /// </summary>
    Task<IEnumerable<GroupingHistory>> GetRecentHistoryAsync(int limit = 10);
    
    /// <summary>
    /// 根据会话ID获取历史记录
    /// </summary>
    Task<IEnumerable<GroupingHistory>> GetBySessionIdAsync(string sessionId, int limit = 10);
    
    /// <summary>
    /// 清空所有分组历史记录
    /// </summary>
    Task ClearAllAsync();
    
    /// <summary>
    /// 清空指定会话的分组历史记录
    /// </summary>
    Task ClearBySessionIdAsync(string sessionId);
    
    /// <summary>
    /// 删除指定批次的分组历史
    /// </summary>
    Task DeleteByBatchIdAsync(string batchId);
}
