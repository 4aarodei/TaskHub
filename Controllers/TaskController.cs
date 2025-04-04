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
            var team = await _teamService.GetTeamByIdAsync(teamId);

            var model = new IndexModel(userTask, team)
            {
                TaskList = userTask,
                TeamModel = team
            };
            return View(model);
        }

        public class GetCreateModel
        {
            public GetCreateModel(List<AppUser> usersOnTeam, List<TeamModel> teamModels)
            {
                UsersOnTeam = usersOnTeam;
                TeamModels = teamModels;
            }

            public List<AppUser> UsersOnTeam { get; set; }
            public List<TeamModel> TeamModels { get; set; }
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