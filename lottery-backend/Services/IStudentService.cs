using LotteryBackend.Models;

namespace LotteryBackend.Services;

/// <summary>
/// 学生服务接口
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// 获取所有学生
    /// </summary>
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    
    /// <summary>
    /// 根据ID获取学生
    /// </summary>
    Task<Student?> GetStudentByIdAsync(int id);
    
    /// <summary>
    /// 随机抽取学生
    /// </summary>
    Task<Student?> DrawStudentAsync(string? gender = null, string? className = null);
    
    /// <summary>
    /// 获取统计信息
    /// </summary>
    Task<object> GetStatisticsAsync();
    
    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    Task<IEnumerable<string>> GetClassListAsync();
}
