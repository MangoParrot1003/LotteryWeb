using LotteryBackend.Data;
using LotteryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace LotteryBackend.Repositories;

/// <summary>
/// 会员仓储实现
/// </summary>
public class MembershipRepository : IMembershipRepository
{
    private readonly StudentContext _context;

    public MembershipRepository(StudentContext context)
    {
        _context = context;
    }

    public async Task<MembershipRecord> CreateMembershipAsync(MembershipRecord membership)
    {
        _context.MembershipRecords.Add(membership);
        await _context.SaveChangesAsync();
        return membership;
    }

    public async Task<MembershipRecord?> GetActiveMembershipByUserIdAsync(string userId)
    {
        return await _context.MembershipRecords
            .Where(m => m.UserId == userId && m.Status == "active")
            .Where(m => !m.ExpiryTime.HasValue || m.ExpiryTime.Value > DateTime.Now)
            .Where(m => m.MembershipType == "monthly" || m.RemainingExports > 0)
            .OrderByDescending(m => m.PurchaseTime)
            .FirstOrDefaultAsync();
    }

    public async Task<MembershipRecord?> GetByOrderNoAsync(string orderNo)
    {
        return await _context.MembershipRecords
            .FirstOrDefaultAsync(m => m.OrderNo == orderNo);
    }

    public async Task<bool> DecrementExportCountAsync(int membershipId)
    {
        var membership = await _context.MembershipRecords.FindAsync(membershipId);
        if (membership == null) return false;

        if (membership.MembershipType == "single")
        {
            membership.RemainingExports--;
            if (membership.RemainingExports <= 0)
            {
                membership.Status = "used";
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int membershipId, string status)
    {
        var membership = await _context.MembershipRecords.FindAsync(membershipId);
        if (membership == null) return false;

        membership.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<MembershipRecord>> GetUserMembershipsAsync(string userId)
    {
        return await _context.MembershipRecords
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.PurchaseTime)
            .ToListAsync();
    }
}
