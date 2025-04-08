using global::TaskHub.Data;
using Microsoft.EntityFrameworkCore;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class TaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskModel>> GetAllTasksForTeamAsync(Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.TeamId == teamId)
                .ToListAsync();
        }

        // Отримати всі задачі користувача
        public async Task<List<TaskModel>> GetAllTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<TaskModel>> GetAllTasksByUserId_OnTeamAsync(string userId, Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.TeamId == teamId)
                .ToListAsync();
        }

        // Отримати задачу за ID
        public async Task<TaskModel?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        // Додати нову задачу
        public async Task<bool> AddTaskAsync(TaskModel task)
        {
            _context.Tasks.Add(task);
            return await _context.SaveChangesAsync() > 0;
        }

        // Оновити задачу
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }

        // Видалити задачу
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}


