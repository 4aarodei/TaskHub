namespace TaskHub.Models
{
    public class TeamInvite
    {
        public Guid Id { get; set; } // Унікальний токен запрошення
        public Guid TeamId { get; set; } // ID команди
        public DateTime CreatedAt { get; set; } // Час створення запрошення
    }

}
