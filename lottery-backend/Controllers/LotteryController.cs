using Microsoft.AspNetCore.Mvc;
using LotteryBackend.Models;
using LotteryBackend.Services;

namespace LotteryBackend.Controllers;

/// <summary>
/// 抽签控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LotteryController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<LotteryController> _logger;

    public LotteryController(IStudentService studentService, ILogger<LotteryController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>
    /// 获取所有学生列表
    /// </summary>
    /// <returns>学生列表</returns>
    /// <response code="200">返回学生列表</response>
    [HttpGet("students")]
    [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        _logger.LogInformation("API调用: 获取所有学生");
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    /// <summary>
    /// 根据ID获取学生信息
    /// </summary>
    /// <param name="id">学生ID</param>
    /// <returns>学生信息</returns>
    /// <response code="200">返回学生信息</response>
    /// <response code="404">学生不存在</response>
    [HttpGet("students/{id}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        _logger.LogInformation("API调用: 获取学生ID={Id}", id);
        var student = await _studentService.GetStudentByIdAsync(id);
        
        if (student == null)
        {
            _logger.LogWarning("学生不存在: ID={Id}", id);
            return NotFound(new { message = "学生不存在" });
        }
        
        return Ok(student);
    }

    /// <summary>
    /// 随机抽取学生
    /// </summary>
    /// <param name="gender">性别筛选（可选）</param>
    /// <param name="className">班级筛选（可选）</param>
    /// <param name="sessionId">会话ID（可选）</param>
    /// <returns>抽中的学生</returns>
    /// <response code="200">返回抽中的学生</response>
    /// <response code="404">没有符合条件的学生</response>
    [HttpGet("draw")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> DrawStudent(
        [FromQuery] string? gender = null, 
        [FromQuery] string? className = null,
        [FromQuery] string? sessionId = null)
    {
        _logger.LogInformation("API调用: 抽签 - 性别={Gender}, 班级={Class}", gender ?? "全部", className ?? "全部");
        var student = await _studentService.DrawStudentAsync(gender, className);
        
        if (student == null)
        {
            _logger.LogWarning("没有符合条件的学生 - 性别={Gender}, 班级={Class}", gender, className);
            return NotFound(new { message = "没有符合条件的学生" });
        }
        
        // 保存抽签历史
        await _studentService.SaveDrawHistoryAsync(student, sessionId, false, null);
        
        return Ok(student);
    }

    /// <summary>
    /// 获取统计信息
    /// </summary>
    /// <returns>统计信息（总数、性别分布、班级分布）</returns>
    /// <response code="200">返回统计信息</response>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetStatistics()
    {
        _logger.LogInformation("API调用: 获取统计信息");
        var stats = await _studentService.GetStatisticsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// 获取所有班级列表
    /// </summary>
    /// <returns>班级列表</returns>
    /// <response code="200">返回班级列表</response>
    [HttpGet("classes")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetClasses()
    {
        _logger.LogInformation("API调用: 获取班级列表");
        var classes = await _studentService.GetClassListAsync();
        return Ok(classes);
    }

    /// <summary>
    /// 批量随机抽取学生
    /// </summary>
    /// <param name="count">抽取数量</param>
    /// <param name="gender">性别筛选（可选）</param>
    /// <param name="className">班级筛选（可选）</param>
    /// <param name="sessionId">会话ID（可选）</param>
    /// <returns>抽中的学生列表</returns>
    /// <response code="200">返回抽中的学生列表</response>
    /// <response code="400">请求参数错误</response>
    /// <response code="404">没有足够的符合条件的学生</response>
    [HttpGet("draw-multiple")]
    [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Student>>> DrawMultipleStudents(
        [FromQuery] int count = 1,
        [FromQuery] string? gender = null,
        [FromQuery] string? className = null,
        [FromQuery] string? sessionId = null)
    {
        if (count < 1 || count > 50)
        {
            _logger.LogWarning("批量抽签数量无效: {Count}", count);
            return BadRequest(new { message = "抽取数量必须在 1-50 之间" });
        }

        _logger.LogInformation("API调用: 批量抽签 - 数量={Count}, 性别={Gender}, 班级={Class}", 
            count, gender ?? "全部", className ?? "全部");
        
        var students = await _studentService.DrawMultipleStudentsAsync(count, gender, className);
        var studentList = students.ToList();
        
        if (!studentList.Any())
        {
            _logger.LogWarning("没有符合条件的学生 - 性别={Gender}, 班级={Class}", gender, className);
            return NotFound(new { message = "没有符合条件的学生" });
        }
        
        if (studentList.Count < count)
        {
            _logger.LogWarning("符合条件的学生不足 - 需要={Count}, 实际={Actual}", count, studentList.Count);
            return NotFound(new { message = $"符合条件的学生不足，仅找到 {studentList.Count} 人" });
        }
        
        // 保存批量抽签历史
        var batchId = Guid.NewGuid().ToString();
        foreach (var student in studentList)
        {
            await _studentService.SaveDrawHistoryAsync(student, sessionId, true, batchId);
        }
        
        return Ok(studentList);
    }

    /// <summary>
    /// 获取抽签历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选）</param>
    /// <param name="limit">返回数量限制</param>
    /// <returns>历史记录列表</returns>
    /// <response code="200">返回历史记录列表</response>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<DrawHistory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DrawHistory>>> GetHistory(
        [FromQuery] string? sessionId = null,
        [FromQuery] int limit = 100)
    {
        _logger.LogInformation("API调用: 获取抽签历史 - 会话={SessionId}, 限制={Limit}", 
            sessionId ?? "全部", limit);
        
        var history = await _studentService.GetDrawHistoryAsync(sessionId, limit);
        return Ok(history);
    }

    /// <summary>
    /// 清空抽签历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选，不传则清空所有）</param>
    /// <returns>操作结果</returns>
    /// <response code="200">清空成功</response>
    [HttpDelete("history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ClearHistory([FromQuery] string? sessionId = null)
    {
        _logger.LogInformation("API调用: 清空抽签历史 - 会话={SessionId}", sessionId ?? "全部");
        
        await _studentService.ClearDrawHistoryAsync(sessionId);
        return Ok(new { message = "历史记录已清空" });
    }

    /// <summary>
    /// 删除单条历史记录
    /// </summary>
    /// <param name="id">历史记录ID</param>
    /// <returns>操作结果</returns>
    /// <response code="200">删除成功</response>
    [HttpDelete("history/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteHistory(int id)
    {
        _logger.LogInformation("API调用: 删除历史记录 - ID={Id}", id);
        
        await _studentService.DeleteDrawHistoryAsync(id);
        return Ok(new { message = "历史记录已删除" });
    }
}
