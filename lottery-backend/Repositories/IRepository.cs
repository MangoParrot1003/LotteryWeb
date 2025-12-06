namespace LotteryBackend.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// 获取所有实体
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// 添加实体
    /// </summary>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// 更新实体
    /// </summary>
    Task UpdateAsync(T entity);
    
    /// <summary>
    /// 删除实体
    /// </summary>
    Task DeleteAsync(int id);
    
    /// <summary>
    /// 保存更改
    /// </summary>
    Task<int> SaveChangesAsync();
}
