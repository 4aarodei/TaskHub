using Microsoft.AspNetCore.Mvc;
using TaskHub.Models;
using TaskHub.Models.TaskViewModel;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    public class TaskController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly TeamService _teamService;
        private readonly InviteService _inviteService;

        public TaskController(/*ApplicationDbContext context,*/ UserService userService, TaskService taskService, InviteService inviteService, TeamService teamService)
        {
            //_context = context;
            _teamService = teamService;
            _inviteService = inviteService;
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<IActionResult> NoTeamsPreIndex()
        {
            return View();
        }

        public async Task<IActionResult> PreIndex(Guid teamId)
        {

            var user = await _userService.GetCurrentUserAsync();
            var userTeams = await _teamService.GetAllTeamsForUserAsync(user.Id);
            if (userTeams.Count == 0)
            {
                return RedirectToAction("NoTeamsPreIndex");

            }
            return View(userTeams);
        }

        // GET: Task/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Index(Guid teamId)
        {
            var user = await _userService.GetCurrentUserAsync();
            var userTask = await _taskService.GetAllTasksByUserId_OnTeamAsync(user.Id, teamId);
            var teamTasks = await _taskService.GetAllTasksForTeamAsync(teamId);
            var team = await _teamService.GetTeamByIdAsync(teamId);
            var userOnTeam = await _teamService.GetUserForTeamAsync(teamId);

            var model = new IndexModel(userTask, team, userOnTeam, teamTasks)
            {
                TaskList = userTask,
                TeamModel = team,
                UsersOnTeam = userOnTeam,
                TeamTasks = teamTasks
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid teamId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);
            var usersOnTeam = await _teamService.GetUserForTeamAsync(teamId);

            var model = new TaskModel
            {
                Team = team,
                TeamId = team.ID,
                AppUser = new AppUser(),
                UserId = string.Empty
            };

            ViewBag.UsersOnTeam = usersOnTeam;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskModel task)
        {


           return RedirectToAction();
        }

        // GET: Task/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(TaskModel task)
        {
            return RedirectToAction(nameof(PreIndex));
        }

        // GET: Task/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(PreIndex));
        }

    }
}