using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 抽签历史仓储实现
/// </summary>
public class DrawHistoryRepository : Repository<DrawHistory>, IDrawHistoryRepository
{
    public DrawHistoryRepository(StudentContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DrawHistory>> GetBySessionIdAsync(string sessionId, int limit = 100)
    {
        return await _dbSet
            .Where(h => h.SessionId == sessionId)
            .OrderByDescending(h => h.Id)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IEnumerable<DrawHistory>> GetRecentHistoryAsync(int limit = 100)
    {
        return await _dbSet
            .OrderByDescending(h => h.Id)
            .Take(limit)
            .ToListAsync();
    }

    public async Task ClearBySessionIdAsync(string sessionId)
    {
        var records = await _dbSet
            .Where(h => h.SessionId == sessionId)
            .ToListAsync();
        
        _dbSet.RemoveRange(records);
        await _context.SaveChangesAsync();
    }

    public async Task ClearAllAsync()
    {
        var records = await _dbSet.ToListAsync();
        _dbSet.RemoveRange(records);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DrawHistory>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(h => h.StudentId == studentId)
            .OrderByDescending(h => h.Id)
            .ToListAsync();
    }
}
