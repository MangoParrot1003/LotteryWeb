namespace LotteryBackend.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int? SerialNumber { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Major { get; set; }
        public string? Class { get; set; }
    }
}
