namespace LotteryBackend.Models;

/// <summary>
/// 抽签历史记录模型
/// </summary>
public class DrawHistory
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 学生ID
    /// </summary>
    public int StudentId { get; set; }
    
    /// <summary>
    /// 学生姓名（冗余字段，方便查询）
    /// </summary>
    public string StudentName { get; set; } = string.Empty;
    
    /// <summary>
    /// 学号（冗余字段）
    /// </summary>
    public string StudentNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// 班级（冗余字段）
    /// </summary>
    public string? Class { get; set; }
    
    /// <summary>
    /// 性别（冗余字段）
    /// </summary>
    public string? Gender { get; set; }
    
    /// <summary>
    /// 抽签时间
    /// </summary>
    public DateTime DrawTime { get; set; }
    
    /// <summary>
    /// 会话ID（用于区分不同的抽签会话）
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// 是否批量抽签
    /// </summary>
    public bool IsBatch { get; set; }
    
    /// <summary>
    /// 批次号（批量抽签时的批次）
    /// </summary>
    public string? BatchId { get; set; }
}
