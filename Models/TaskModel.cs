namespace TaskHub.Models
{
    public class TaskModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow; // Використовуємо UTC для створення дати
        public bool IsComplete { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Використовуємо UTC для створення дати

        // **Правильний зв’язок**
        public string UserId { get; set; } // Це зовнішній ключ (FK)
        public User User { get; set; }  // Навігаційна властивість
        public TeamModel Team { get; set; } // Навігаційна властивість
        public Guid TeamId { get; set; } // Це зовнішній ключ (FK)
    }
}