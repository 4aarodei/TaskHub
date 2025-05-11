namespace TaskHub.Models
{
    public class TeamModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid CreatorId { get; set; } // Це зовнішній ключ (FK)
        public AppUser Creator { get; set; } // Навігаційна властивість
        public Guid AdminId { get; set; }
        public AppUser Admin { get; set; } // Навігаційна властивістьf
        public string Name { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Використовуємо UTC для створення дати
    }
}