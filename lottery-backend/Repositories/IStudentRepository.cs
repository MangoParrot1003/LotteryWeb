using LotteryBackend.Models;

namespace LotteryBackend.Repositories;

/// <summary>
/// 学生仓储接口
/// </summary>
public interface IStudentRepository : IRepository<Student>
{
    /// <summary>
    /// 根据筛选条件获取学生
    /// </summary>
    Task<IEnumerable<Student>> GetStudentsByFilterAsync(string? gender = null, string? className = null);
    
    /// <summary>
    /// 随机抽取学生
    /// </summary>
    Task<Student?> GetRandomStudentAsync(string? gender = null, string? className = null);
    
    /// <summary>
    /// 批量随机抽取学生
    /// </summary>
    Task<IEnumerable<Student>> GetRandomStudentsAsync(int count, string? gender = null, string? className = null);
    
    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    Task<IEnumerable<string>> GetClassListAsync();
    
    /// <summary>
    /// 获取统计信息
    /// </summary>
    Task<object> GetStatisticsAsync();
}
