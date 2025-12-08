namespace LotteryBackend.Models;

/// <summary>
/// 分组历史记录模型
/// </summary>
public class GroupingHistory
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 分组批次ID（同一次分组操作的所有组共享相同的批次ID）
    /// </summary>
    public string BatchId { get; set; } = string.Empty;
    
    /// <summary>
    /// 组号（从1开始）
    /// </summary>
    public int GroupNumber { get; set; }
    
    /// <summary>
    /// 每组人数设置
    /// </summary>
    public int GroupSize { get; set; }
    
    /// <summary>
    /// 总组数
    /// </summary>
    public int TotalGroups { get; set; }
    
    /// <summary>
    /// 组内成员（JSON格式存储学生信息）
    /// </summary>
    public string Members { get; set; } = string.Empty;
    
    /// <summary>
    /// 分组时间
    /// </summary>
    public DateTime GroupTime { get; set; }
    
    /// <summary>
    /// 会话ID（可选，用于区分不同使用者）
    /// </summary>
    public string? SessionId { get; set; }
}
