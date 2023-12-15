
namespace ToDoList.Models
{
    public class TaskModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Description { get; set; }
        public DateTime? Timeframe { get; set; }
        public string? Priority { get; set; }
        public bool Done { get; set; }

        public User? User { get; set; }
    }
}
