namespace TaskHub.Models
{
    public class TeamModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Використовуємо UTC для створення дати
    }
}