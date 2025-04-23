using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
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

        public async Task<bool> CompletSubtask(Guid subtaskID, TaskModel task)
        {
            var subtask = _context.Subtasks.FirstOrDefault(s => s.Id == subtaskID);
            if (subtask == null) return false;

            subtask.IsComplete = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TaskModel>> GetDoneTask(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId.ToString() && t.IsComplete)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<bool> CompleteTaskWithSubtasksAsync(Guid taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync(t => t.ID == taskId);

            if (task.Subtasks != null)
            {
                foreach (var subtask in task.Subtasks)
                {
                    subtask.IsComplete = true; // Без перевірки, просто одразу оновлюємо
                }
            }

            task.IsComplete = true;
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<TaskModel?> GetDoneTask(Guid userId, Guid taskId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId.ToString() && t.ID == taskId && t.IsComplete)
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TaskModel>> GetNonFinishedTask(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && !t.IsComplete)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task AssignTaskToUserAsync(Guid taskId, string userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync(t => t.ID == taskId);

            if (task != null && task.UserId == null)
            {
                task.UserId = userId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TaskModel>> GetTasksWithUserAsync()
        {
            return await _context.Tasks
                .Where(t => t.UserId != null)
                .Include(t => t.AppUser)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<List<TaskModel>> GetAllTaskWithoutUser(Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == null && t.TeamId == teamId)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<List<TaskModel>> GetAllTasksForTeamAsync(Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.TeamId == teamId)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<List<TaskModel>> GetAllTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<List<TaskModel>> GetAllTasksByUserId_OnTeamAsync(string userId, Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.TeamId == teamId)
                .Include(t => t.Subtasks)
                .ToListAsync();
        }

        public async Task<TaskModel?> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<bool> AddTaskAsync(TaskModel task)
        {
            _context.Tasks.Add(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}


