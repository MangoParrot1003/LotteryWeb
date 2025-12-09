using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 抽奖历史仓储接口
/// </summary>
public interface IPrizeDrawHistoryRepository
{
    Task<IEnumerable<PrizeDrawHistory>> GetAllAsync(string? sessionId = null, int limit = 10);
    Task<PrizeDrawHistory?> GetByIdAsync(int id);
    Task AddAsync(PrizeDrawHistory history);
    Task DeleteAsync(int id);
    Task ClearAsync(string? sessionId = null);
}
