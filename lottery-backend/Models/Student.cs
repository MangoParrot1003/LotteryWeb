namespace LotteryBackend.Models;

/// <summary>
/// 学生模型
/// </summary>
public class Student
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 序号
    /// </summary>
    public int? SerialNumber { get; set; }
    
    /// <summary>
    /// 学号
    /// </summary>
    public string StudentId { get; set; } = string.Empty;
    
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; }
    
    /// <summary>
    /// 专业
    /// </summary>
    public string? Major { get; set; }
    
    /// <summary>
    /// 班级
    /// </summary>
    public string? Class { get; set; }
}
