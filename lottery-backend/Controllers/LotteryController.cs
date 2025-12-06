using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Models;

namespace LotteryBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotteryController : ControllerBase
    {
        private readonly StudentContext _context;

        public LotteryController(StudentContext context)
        {
            _context = context;
        }

        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("students/{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpGet("draw")]
        public async Task<ActionResult<Student>> DrawStudent([FromQuery] string? gender = null, [FromQuery] string? className = null)
        {
            var query = _context.Students.AsQueryable();
            
            if (!string.IsNullOrEmpty(gender))
                query = query.Where(s => s.Gender == gender);
            
            if (!string.IsNullOrEmpty(className))
                query = query.Where(s => s.Class == className);

            var count = await query.CountAsync();
            if (count == 0) return NotFound("没有符合条件的学生");

            var random = new Random();
            var skip = random.Next(count);
            var student = await query.Skip(skip).FirstOrDefaultAsync();

            return student!;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            var total = await _context.Students.CountAsync();
            var genderStats = await _context.Students
                .GroupBy(s => s.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToListAsync();
            var classStats = await _context.Students
                .GroupBy(s => s.Class)
                .Select(g => new { Class = g.Key, Count = g.Count() })
                .ToListAsync();

            return new { Total = total, GenderStats = genderStats, ClassStats = classStats };
        }
    }
}
