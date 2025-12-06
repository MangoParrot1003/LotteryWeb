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
    /// <returns>抽中的学生</returns>
    /// <response code="200">返回抽中的学生</response>
    /// <response code="404">没有符合条件的学生</response>
    [HttpGet("draw")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> DrawStudent([FromQuery] string? gender = null, [FromQuery] string? className = null)
    {
        _logger.LogInformation("API调用: 抽签 - 性别={Gender}, 班级={Class}", gender ?? "全部", className ?? "全部");
        var student = await _studentService.DrawStudentAsync(gender, className);
        
        if (student == null)
        {
            _logger.LogWarning("没有符合条件的学生 - 性别={Gender}, 班级={Class}", gender, className);
            return NotFound(new { message = "没有符合条件的学生" });
        }
        
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
}
