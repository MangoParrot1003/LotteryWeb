using Microsoft.AspNetCore.Mvc;
using LotteryBackend.Models;
using LotteryBackend.Services;
using LotteryBackend.Repositories;

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
    private readonly IMembershipRepository _membershipRepository;

    public LotteryController(
        IStudentService studentService, 
        ILogger<LotteryController> logger,
        IMembershipRepository membershipRepository)
    {
        _studentService = studentService;
        _logger = logger;
        _membershipRepository = membershipRepository;
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

    // ========== 分组历史相关 API ==========

    /// <summary>
    /// 保存分组结果
    /// </summary>
    /// <param name="request">分组请求</param>
    /// <returns>操作结果</returns>
    /// <response code="200">保存成功</response>
    [HttpPost("grouping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SaveGrouping([FromBody] SaveGroupingRequest request)
    {
        _logger.LogInformation("API调用: 保存分组 - 组数={GroupCount}, 每组人数={GroupSize}", 
            request.Groups.Count, request.GroupSize);
        
        // 转换请求数据为 List<List<Student>>
        var groups = request.Groups.Select(g => g.Select(m => new Models.Student
        {
            Id = m.Id,
            StudentId = m.StudentId,
            Name = m.Name,
            Gender = m.Gender,
            Class = m.Class,
            Major = m.Major
        }).ToList()).ToList();

        await _studentService.SaveGroupingHistoryAsync(groups, request.GroupSize, request.SessionId);
        return Ok(new { message = "分组结果已保存" });
    }

    /// <summary>
    /// 获取分组历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选）</param>
    /// <param name="limit">返回数量限制</param>
    /// <returns>分组历史记录列表</returns>
    /// <response code="200">返回分组历史记录列表</response>
    [HttpGet("grouping-history")]
    [ProducesResponseType(typeof(IEnumerable<Models.GroupingHistory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.GroupingHistory>>> GetGroupingHistory(
        [FromQuery] string? sessionId = null,
        [FromQuery] int limit = 10)
    {
        _logger.LogInformation("API调用: 获取分组历史 - 会话={SessionId}, 限制={Limit}", 
            sessionId ?? "全部", limit);
        
        var history = await _studentService.GetGroupingHistoryAsync(sessionId, limit);
        return Ok(history);
    }

    /// <summary>
    /// 清空分组历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选，不传则清空所有）</param>
    /// <returns>操作结果</returns>
    /// <response code="200">清空成功</response>
    [HttpDelete("grouping-history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ClearGroupingHistory([FromQuery] string? sessionId = null)
    {
        _logger.LogInformation("API调用: 清空分组历史 - 会话={SessionId}", sessionId ?? "全部");
        
        await _studentService.ClearGroupingHistoryAsync(sessionId);
        return Ok(new { message = "分组历史记录已清空" });
    }

    /// <summary>
    /// 删除指定批次的分组历史
    /// </summary>
    /// <param name="batchId">批次ID</param>
    /// <returns>操作结果</returns>
    /// <response code="200">删除成功</response>
    [HttpDelete("grouping-history/{batchId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteGroupingHistoryBatch(string batchId)
    {
        _logger.LogInformation("API调用: 删除分组历史批次 - BatchId={BatchId}", batchId);
        
        await _studentService.DeleteGroupingHistoryByBatchIdAsync(batchId);
        return Ok(new { message = "分组历史批次已删除" });
    }

    // ========== 抽奖历史相关 API ==========

    /// <summary>
    /// 保存抽奖结果
    /// </summary>
    /// <param name="request">抽奖请求</param>
    /// <returns>操作结果</returns>
    /// <response code="200">保存成功</response>
    [HttpPost("prize-draw")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SavePrizeDraw([FromBody] SavePrizeDrawRequest request)
    {
        _logger.LogInformation("API调用: 保存抽奖 - 奖品={PrizeName}, 中奖人数={Count}", 
            request.PrizeName, request.Winners.Count);

        // 转换为 Student 对象
        var winners = request.Winners.Select(w => new Models.Student
        {
            Id = w.Id,
            StudentId = w.StudentId,
            Name = w.Name,
            Gender = w.Gender,
            Class = w.Class,
            Major = w.Major
        }).ToList();

        await _studentService.SavePrizeDrawHistoryAsync(request.PrizeName, winners, request.SessionId);
        return Ok(new { message = "抽奖结果已保存" });
    }

    /// <summary>
    /// 获取抽奖历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选）</param>
    /// <param name="limit">返回数量限制</param>
    /// <returns>抽奖历史记录列表</returns>
    /// <response code="200">返回抽奖历史记录列表</response>
    [HttpGet("prize-history")]
    [ProducesResponseType(typeof(IEnumerable<Models.PrizeDrawHistory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.PrizeDrawHistory>>> GetPrizeHistory(
        [FromQuery] string? sessionId = null,
        [FromQuery] int limit = 10)
    {
        _logger.LogInformation("API调用: 获取抽奖历史 - 会话={SessionId}, 限制={Limit}", 
            sessionId ?? "全部", limit);

        var history = await _studentService.GetPrizeHistoryAsync(sessionId, limit);
        return Ok(history);
    }

    /// <summary>
    /// 清空抽奖历史记录
    /// </summary>
    /// <param name="sessionId">会话ID（可选，不传则清空所有）</param>
    /// <returns>操作结果</returns>
    /// <response code="200">清空成功</response>
    [HttpDelete("prize-history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ClearPrizeHistory([FromQuery] string? sessionId = null)
    {
        _logger.LogInformation("API调用: 清空抽奖历史 - 会话={SessionId}", sessionId ?? "全部");

        await _studentService.ClearPrizeHistoryAsync(sessionId);
        return Ok(new { message = "抽奖历史记录已清空" });
    }

    /// <summary>
    /// 删除单条抽奖历史记录
    /// </summary>
    /// <param name="id">历史记录ID</param>
    /// <returns>操作结果</returns>
    /// <response code="200">删除成功</response>
    [HttpDelete("prize-history/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePrizeHistory(int id)
    {
        _logger.LogInformation("API调用: 删除抽奖历史记录 - ID={Id}", id);

        await _studentService.DeletePrizeHistoryAsync(id);
        return Ok(new { message = "抽奖历史记录已删除" });
    }

    /// <summary>
    /// 批量抽奖（一次抽取多个奖项）
    /// </summary>
    /// <param name="request">批量抽奖请求</param>
    /// <returns>操作结果</returns>
    /// <response code="200">抽奖成功</response>
    [HttpPost("prize-draw-batch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SaveBatchPrizeDraw([FromBody] SaveBatchPrizeDrawRequest request)
    {
        _logger.LogInformation("API调用: 批量抽奖 - 奖项数={Count}", request.Prizes.Count);

        // 生成批次ID
        var batchId = Guid.NewGuid().ToString();
        var allWinnersIds = new HashSet<int>(); // 用于防止重复中奖

        foreach (var prize in request.Prizes)
        {
            // 抽取中奖者，排除已中奖的学生
            var availableStudents = await _studentService.DrawMultipleStudentsAsync(
                prize.WinnerCount,
                request.Gender,
                request.ClassName);

            var winners = availableStudents
                .Where(s => !allWinnersIds.Contains(s.Id))
                .Take(prize.WinnerCount)
                .ToList();

            // 如果不够人数，继续抽取
            while (winners.Count < prize.WinnerCount)
            {
                var additionalStudent = await _studentService.DrawStudentAsync(
                    request.Gender,
                    request.ClassName);

                if (additionalStudent != null && !allWinnersIds.Contains(additionalStudent.Id))
                {
                    winners.Add(additionalStudent);
                    allWinnersIds.Add(additionalStudent.Id);
                }
                else
                {
                    break; // 没有更多符合条件的学生
                }
            }

            // 记录已中奖学生
            foreach (var winner in winners)
            {
                allWinnersIds.Add(winner.Id);
            }

            // 保存本奖项的抽奖结果
            await _studentService.SavePrizeDrawHistoryAsync(
                prize.PrizeName,
                winners,
                request.SessionId,
                batchId);
        }

        return Ok(new { message = "批量抽奖完成", batchId });
    }

    // ========== 会员管理相关 API ==========

    /// <summary>
    /// 创建会员记录（模拟支付成功）
    /// </summary>
    [HttpPost("membership")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateMembership([FromBody] CreateMembershipRequest request)
    {
        _logger.LogInformation("API调用: 创建会员 - 用户={UserId}, 类型={Type}", 
            request.UserId, request.MembershipType);

        var membership = new MembershipRecord
        {
            UserId = request.UserId,
            MembershipType = request.MembershipType,
            OrderNo = request.OrderNo,
            Amount = request.Amount,
            RemainingExports = request.MembershipType == "single" ? 1 : -1,
            PurchaseTime = DateTime.Now,
            ExpiryTime = request.MembershipType == "monthly" ? DateTime.Now.AddDays(30) : null,
            Status = "active"
        };

        await _membershipRepository.CreateMembershipAsync(membership);
        return Ok(new { message = "会员创建成功", membership });
    }

    /// <summary>
    /// 查询用户会员状态
    /// </summary>
    [HttpGet("membership/check/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CheckMembership(string userId)
    {
        _logger.LogInformation("API调用: 查询会员状态 - 用户={UserId}", userId);

        var membership = await _membershipRepository.GetActiveMembershipByUserIdAsync(userId);
        
        if (membership == null || !membership.IsValid)
        {
            return Ok(new { 
                hasPermission = false, 
                message = "未找到有效会员" 
            });
        }

        return Ok(new { 
            hasPermission = true, 
            membership = new
            {
                type = membership.MembershipType,
                remainingExports = membership.RemainingExports,
                expiryTime = membership.ExpiryTime
            }
        });
    }

    /// <summary>
    /// 导出Excel数据
    /// </summary>
    [HttpPost("export/excel")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> ExportToExcel([FromBody] ExportRequest request)
    {
        _logger.LogInformation("API调用: 导出Excel - 用户={UserId}, 类型={Type}", 
            request.UserId, request.ExportType);

        // 检查会员权限
        var membership = await _membershipRepository.GetActiveMembershipByUserIdAsync(request.UserId);
        if (membership == null || !membership.IsValid)
        {
            return Unauthorized(new { message = "需要购买会员才能导出数据" });
        }

        // 扣除导出次数
        if (membership.MembershipType == "single")
        {
            await _membershipRepository.DecrementExportCountAsync(membership.Id);
        }

        // 根据导出类型获取数据
        var exportData = await GetExportDataAsync(request.ExportType);
        
        // 生成Excel文件（简化实现，实际应使用EPPlus或NPOI库）
        var csvContent = GenerateCsvContent(exportData, request.ExportType);
        var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);

        return File(bytes, "text/csv", $"{request.ExportType}_{DateTime.Now:yyyyMMddHHmmss}.csv");
    }

    private async Task<object> GetExportDataAsync(string exportType)
    {
        return exportType switch
        {
            "draw_history" => await _studentService.GetDrawHistoryAsync(null, 1000),
            "prize_history" => await _studentService.GetPrizeHistoryAsync(null, 1000),
            "students" => await _studentService.GetAllStudentsAsync(),
            _ => await _studentService.GetDrawHistoryAsync(null, 1000)
        };
    }

    private string GenerateCsvContent(object data, string exportType)
    {
        var sb = new System.Text.StringBuilder();
        
        switch (exportType)
        {
            case "draw_history":
                var drawHistory = (IEnumerable<DrawHistory>)data;
                sb.AppendLine("序号,学号,姓名,班级,性别,抽取时间");
                int index = 1;
                foreach (var item in drawHistory)
                {
                    sb.AppendLine($"{index++},{item.StudentNumber},{item.StudentName},{item.Class},{item.Gender},{item.DrawTime:yyyy-MM-dd HH:mm:ss}");
                }
                break;
                
            case "prize_history":
                var prizeHistory = (IEnumerable<PrizeDrawHistory>)data;
                sb.AppendLine("序号,奖品名称,中奖人数,抽奖时间,中奖者");
                index = 1;
                foreach (var item in prizeHistory)
                {
                    var winners = string.Join(";", item.WinnersList.Select(w => w.Name));
                    sb.AppendLine($"{index++},{item.PrizeName},{item.WinnerCount},{item.DrawTime:yyyy-MM-dd HH:mm:ss},{winners}");
                }
                break;
                
            case "students":
                var students = (IEnumerable<Student>)data;
                sb.AppendLine("学号,姓名,性别,班级,专业");
                foreach (var s in students)
                {
                    sb.AppendLine($"{s.StudentId},{s.Name},{s.Gender},{s.Class},{s.Major}");
                }
                break;
        }
        
        return sb.ToString();
    }
}

/// <summary>
/// 保存抽奖请求模型
/// </summary>
public class SavePrizeDrawRequest
{
    /// <summary>
    /// 奖品名称
    /// </summary>
    public string PrizeName { get; set; } = string.Empty;

    /// <summary>
    /// 中奖者列表
    /// </summary>
    public List<GroupMember> Winners { get; set; } = new();

    /// <summary>
    /// 会话ID（可选）
    /// </summary>
    public string? SessionId { get; set; }
}

/// <summary>
/// 保存分组请求模型
/// </summary>
public class SaveGroupingRequest
{
    /// <summary>
    /// 分组结果
    /// </summary>
    public List<List<GroupMember>> Groups { get; set; } = new();
    
    /// <summary>
    /// 每组人数设置
    /// </summary>
    public int GroupSize { get; set; }
    
    /// <summary>
    /// 会话ID（可选）
    /// </summary>
    public string? SessionId { get; set; }
}

/// <summary>
/// 分组成员信息
/// </summary>
public class GroupMember
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? Class { get; set; }
    public string? Major { get; set; }
}

/// <summary>
/// 批量抽奖请求模型
/// </summary>
public class SaveBatchPrizeDrawRequest
{
    /// <summary>
    /// 奖项列表
    /// </summary>
    public List<PrizeItem> Prizes { get; set; } = new();

    /// <summary>
    /// 性别筛选（可选）
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// 班级筛选（可选）
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// 会话ID（可选）
    /// </summary>
    public string? SessionId { get; set; }
}

/// <summary>
/// 奖项信息
/// </summary>
public class PrizeItem
{
    /// <summary>
    /// 奖品名称
    /// </summary>
    public string PrizeName { get; set; } = string.Empty;

    /// <summary>
    /// 中奖人数
    /// </summary>
    public int WinnerCount { get; set; }
}

/// <summary>
/// 创建会员请求
/// </summary>
public class CreateMembershipRequest
{
    public string UserId { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public string OrderNo { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// 导出请求
/// </summary>
public class ExportRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ExportType { get; set; } = "draw_history";
}

