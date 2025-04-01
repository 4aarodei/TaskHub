using Microsoft.AspNetCore.Identity;

namespace TaskHub.Models
{
    public class AppUser : IdentityUser
    {
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}