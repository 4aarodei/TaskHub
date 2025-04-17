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
        private readonly TeamService _teamService;

        public TaskController(UserService userService, TaskService taskService, TeamService teamService)
        {
            _teamService = teamService;
            _userService = userService;
            _taskService = taskService;
        }

       

        //public async Task<IActionResult> PreIndex(Guid teamId)
        //{
        //    var user = await _userService.GetCurrentUserAsync();
        //    var userTeams = await _teamService.GetAllTeamsForUserAsync(user.Id);
        //    if (userTeams.Count == 0)
        //    {
        //        return RedirectToAction("NoTeamsPreIndex");

        //    }
        //    return View(userTeams);
        //}

        //// GET: Task/Details/{id}
        //[HttpGet]
        //public async Task<IActionResult> Index(Guid teamId)
        //{
        //    var user = await _userService.GetCurrentUserAsync();

        //    var userTask = await _taskService.GetAllTasksByUserId_OnTeamAsync(user.Id, teamId);
        //    var allTeamTasks = await _taskService.GetAllTasksForTeamAsync(teamId);
        //    var taskWithoutUsers = await _taskService.GetAllTaskWithoutUser(teamId);

        //    var usersOnTeam = await _teamService.GetUsersForTeamAsync(teamId);
        //    var team = await _teamService.GetTeamByIdAsync(teamId);

        //    var model = new IndexModel(taskWithoutUsers, usersOnTeam, userTask, allTeamTasks, team)
        //    {
        //        TaskWithoutUser = taskWithoutUsers,
        //        UserTaskList = userTask,
        //        TeamModel = team,
        //        UsersOnTeam = usersOnTeam,
        //        AllTeamTasks = allTeamTasks
        //    };

        //    return View(model);
        //}

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();
            var userTask = await _taskService.GetAllTasksByUserIdAsync(user.Id);

            ViewData["user"] = user;

            return View(userTask);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid teamId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);
            var usersOnTeam = await _teamService.GetUsersForTeamAsync(teamId);

            var model = new TaskModel
            {
                Team = team,
                TeamId = team.ID,
                AppUser = new AppUser(),
                UserId = string.Empty,
                CreatedDate = DateTime.UtcNow.AddSeconds(-DateTime.UtcNow.Second).AddMilliseconds(-DateTime.UtcNow.Millisecond),
                Deadline = DateTime.UtcNow.AddSeconds(-DateTime.UtcNow.Second).AddMilliseconds(-DateTime.UtcNow.Millisecond)
            };

            ViewBag.UsersOnTeam = usersOnTeam;

            return View(model);
        }

       

        // GET: Task/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        //// POST: Task/Edit/{id}
        //[HttpPost]
        //public async Task<IActionResult> Edit(TaskModel task)
        //{
        //    return RedirectToAction(nameof(PreIndex));
        //}

        // GET: Task/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        //// POST: Task/Delete/{id}
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    return RedirectToAction(nameof(PreIndex));
        //}

        [HttpGet]
        public async Task<IActionResult> Details(Guid taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            return View(task); // має бути створена вʼюха Views/Task/Details.cshtml
        }

        

    }
}