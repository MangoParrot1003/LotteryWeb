using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LotteryBackend.Models;

/// <summary>
/// 抽奖历史记录
/// </summary>
[Table("PrizeDrawHistory")]
public class PrizeDrawHistory
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 奖品名称
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PrizeName { get; set; } = string.Empty;

    /// <summary>
    /// 中奖者JSON列表
    /// </summary>
    [Required]
    public string Winners { get; set; } = string.Empty;

    /// <summary>
    /// 中奖人数
    /// </summary>
    public int WinnerCount { get; set; }

    /// <summary>
    /// 抽奖时间
    /// </summary>
    public DateTime DrawTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 会话ID（可选，用于区分不同抽奖会话）
    /// </summary>
    [MaxLength(100)]
    public string? SessionId { get; set; }

    /// <summary>
    /// 批次ID（用于关联同一次抽奖活动的多个奖项）
    /// </summary>
    [MaxLength(100)]
    public string? BatchId { get; set; }

    /// <summary>
    /// 获取中奖者列表
    /// </summary>
    [NotMapped]
    public List<Student> WinnersList
    {
        get
        {
            if (string.IsNullOrEmpty(Winners))
                return new List<Student>();
            
            try
            {
                return JsonSerializer.Deserialize<List<Student>>(Winners) ?? new List<Student>();
            }
            catch
            {
                return new List<Student>();
            }
        }
        set
        {
            Winners = JsonSerializer.Serialize(value);
            WinnerCount = value.Count;
        }
    }
}
