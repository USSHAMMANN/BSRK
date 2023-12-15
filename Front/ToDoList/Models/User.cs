namespace ToDoList.Models
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<TaskModel> Tasks { get; } = new List<TaskModel>();
    }
}
