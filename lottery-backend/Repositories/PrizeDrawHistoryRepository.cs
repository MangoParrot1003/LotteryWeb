using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 抽奖历史仓储实现
/// </summary>
public class PrizeDrawHistoryRepository : IPrizeDrawHistoryRepository
{
    private readonly StudentContext _context;

    public PrizeDrawHistoryRepository(StudentContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PrizeDrawHistory>> GetAllAsync(string? sessionId = null, int limit = 10)
    {
        var query = _context.Set<PrizeDrawHistory>().AsQueryable();

        if (!string.IsNullOrEmpty(sessionId))
        {
            query = query.Where(h => h.SessionId == sessionId);
        }

        return await query
            .OrderByDescending(h => h.DrawTime)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<PrizeDrawHistory?> GetByIdAsync(int id)
    {
        return await _context.Set<PrizeDrawHistory>().FindAsync(id);
    }

    public async Task AddAsync(PrizeDrawHistory history)
    {
        await _context.Set<PrizeDrawHistory>().AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var history = await GetByIdAsync(id);
        if (history != null)
        {
            _context.Set<PrizeDrawHistory>().Remove(history);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearAsync(string? sessionId = null)
    {
        var query = _context.Set<PrizeDrawHistory>().AsQueryable();

        if (!string.IsNullOrEmpty(sessionId))
        {
            query = query.Where(h => h.SessionId == sessionId);
        }

        var records = await query.ToListAsync();
        _context.Set<PrizeDrawHistory>().RemoveRange(records);
        await _context.SaveChangesAsync();
    }
}
