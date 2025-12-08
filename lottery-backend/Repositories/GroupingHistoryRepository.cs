using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 分组历史仓储实现
/// </summary>
public class GroupingHistoryRepository : Repository<GroupingHistory>, IGroupingHistoryRepository
{
    public GroupingHistoryRepository(StudentContext context) : base(context)
    {
    }

    public async Task<IEnumerable<GroupingHistory>> GetByBatchIdAsync(string batchId)
    {
        return await _dbSet
            .Where(h => h.BatchId == batchId)
            .OrderBy(h => h.GroupNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupingHistory>> GetRecentHistoryAsync(int limit = 10)
    {
        // 获取最近的不同批次
        var recentBatches = await _dbSet
            .OrderByDescending(h => h.GroupTime)
            .Select(h => h.BatchId)
            .Distinct()
            .Take(limit)
            .ToListAsync();
        
        // 获取这些批次的所有记录
        return await _dbSet
            .Where(h => recentBatches.Contains(h.BatchId))
            .OrderByDescending(h => h.GroupTime)
            .ThenBy(h => h.GroupNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupingHistory>> GetBySessionIdAsync(string sessionId, int limit = 10)
    {
        var recentBatches = await _dbSet
            .Where(h => h.SessionId == sessionId)
            .OrderByDescending(h => h.GroupTime)
            .Select(h => h.BatchId)
            .Distinct()
            .Take(limit)
            .ToListAsync();
        
        return await _dbSet
            .Where(h => recentBatches.Contains(h.BatchId))
            .OrderByDescending(h => h.GroupTime)
            .ThenBy(h => h.GroupNumber)
            .ToListAsync();
    }

    public async Task ClearAllAsync()
    {
        var records = await _dbSet.ToListAsync();
        _dbSet.RemoveRange(records);
        await _context.SaveChangesAsync();
    }

    public async Task ClearBySessionIdAsync(string sessionId)
    {
        var records = await _dbSet
            .Where(h => h.SessionId == sessionId)
            .ToListAsync();
        
        _dbSet.RemoveRange(records);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByBatchIdAsync(string batchId)
    {
        var records = await _dbSet
            .Where(h => h.BatchId == batchId)
            .ToListAsync();
        
        _dbSet.RemoveRange(records);
        await _context.SaveChangesAsync();
    }
}
