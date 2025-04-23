namespace TaskHub.Models
{
    public class TaskModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Використовуємо UTC для створення дати
        public DateTime Deadline { get; set; }
        public List<Subtasks>? Subtasks { get; set; } = new List<Subtasks>();

        // **Правильний зв’язок**
        public string? UserId { get; set; } // Це зовнішній ключ (FK)
        public AppUser? AppUser { get; set; }  // Навігаційна властивість
        public TeamModel? Team { get; set; } // Навігаційна властивість
        public Guid TeamId { get; set; } // Це зовнішній ключ (FK)
    }
}