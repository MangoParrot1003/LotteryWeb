using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryBackend.Models;

/// <summary>
/// 会员记录
/// </summary>
[Table("MembershipRecords")]
public class MembershipRecord
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID（学号或唯一标识）
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 会员类型（single:单次导出, monthly:包月会员）
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string MembershipType { get; set; } = string.Empty;

    /// <summary>
    /// 订单号
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 支付金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 剩余导出次数（单次会员有限制，包月会员为-1表示无限）
    /// </summary>
    public int RemainingExports { get; set; }

    /// <summary>
    /// 购买时间
    /// </summary>
    public DateTime PurchaseTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? ExpiryTime { get; set; }

    /// <summary>
    /// 状态（active:有效, expired:已过期, used:已用完）
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "active";

    /// <summary>
    /// 检查会员是否有效
    /// </summary>
    [NotMapped]
    public bool IsValid
    {
        get
        {
            if (Status != "active") return false;
            if (ExpiryTime.HasValue && ExpiryTime.Value < DateTime.Now) return false;
            if (MembershipType == "single" && RemainingExports <= 0) return false;
            return true;
        }
    }
}
