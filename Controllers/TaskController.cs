using Microsoft.AspNetCore.Mvc;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    public class TaskController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public TaskController(UserService userService, TaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public class IndexViewModel
        {
            public List<TaskModel>? UserTask { get; set; }
            public string UserId { get; set; }
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();
            var userTask = await _taskService.GetNonFinishedTask(user.Id);

            var model = new IndexViewModel()
            {
                UserId = user.Id,
                UserTask = userTask
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(Guid taskId)
        {
            var user = await _userService.GetCurrentUserAsync();

            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            ViewData ["UserName"] = user.NormalizedUserName;

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(Guid taskId)
        {
            await _taskService.CompleteTaskWithSubtasksAsync(taskId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSubtask(Guid taskId, Guid subtaskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);

            if (task == null)
                return NotFound();

            await _taskService.CompletSubtask(subtaskId, task);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TaskHistory(Guid userId)
        {
            var competedTasks = await _taskService.GetDoneTask(userId);

            return View(competedTasks);
        }

    }
}