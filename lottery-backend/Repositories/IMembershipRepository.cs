using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 会员仓储接口
/// </summary>
public interface IMembershipRepository
{
    /// <summary>
    /// 创建会员记录
    /// </summary>
    Task<MembershipRecord> CreateMembershipAsync(MembershipRecord membership);

    /// <summary>
    /// 根据用户ID获取有效会员记录
    /// </summary>
    Task<MembershipRecord?> GetActiveMembershipByUserIdAsync(string userId);

    /// <summary>
    /// 根据订单号获取会员记录
    /// </summary>
    Task<MembershipRecord?> GetByOrderNoAsync(string orderNo);

    /// <summary>
    /// 减少导出次数
    /// </summary>
    Task<bool> DecrementExportCountAsync(int membershipId);

    /// <summary>
    /// 更新会员状态
    /// </summary>
    Task<bool> UpdateStatusAsync(int membershipId, string status);

    /// <summary>
    /// 获取用户所有会员记录
    /// </summary>
    Task<List<MembershipRecord>> GetUserMembershipsAsync(string userId);
}
