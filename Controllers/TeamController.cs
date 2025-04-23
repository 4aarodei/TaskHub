using Microsoft.AspNetCore.Mvc;
using TaskHub.Services;
using TaskHub.Models;
using TaskHub.Models.TaskViewModel;
using TaskHub.Models.TeamViewModel;

namespace TaskHub.Controllers;

public class TeamController : Controller
{

    private readonly UserService _userService;
    private readonly TaskService _taskService;
    private readonly TeamService _teamService;
    private readonly InviteService _inviteService;

    public TeamController(/*ApplicationDbContext context,*/ UserService userService, TaskService taskService, InviteService inviteService, TeamService teamService)
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

        var allTeamTasks = await _taskService.GetAllTasksForTeamAsync(teamId);
        var taskWithoutUsers = await _taskService.GetAllTaskWithoutUser(teamId);

        var usersOnTeam = await _teamService.GetUsersForTeamAsync(teamId);
        var team = await _teamService.GetTeamByIdAsync(teamId);

        var model = new IndexModel(taskWithoutUsers, usersOnTeam, allTeamTasks, team)
        {
            TaskWithoutUser = taskWithoutUsers,
            TeamModel = team,
            UsersOnTeam = usersOnTeam,
            AllTeamTasks = allTeamTasks
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid teamId)
    {
        var team = await _teamService.GetTeamByIdAsync(teamId);
        var usersOnTeam = await _teamService.GetUsersForTeamAsync(teamId);

        if (team == null)
        {
            return NotFound("Team not found.");
        }

        var model = new TaskModel
        {
            TeamId = team.ID,
            Team = team,
            CreatedDate = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.AddDays(3) // Дедлайн по умолчанию через 7 дней
        };

        ViewBag.UsersOnTeam = usersOnTeam;
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Create(TaskModel task)
    {
        var team = await _teamService.GetTeamByIdAsync(task.TeamId);

        foreach (var subtask in task.Subtasks)
        {
            subtask.Id = Guid.NewGuid();
            subtask.TaskModel = task; // Заполняем TaskModel
            subtask.TaskId = task.ID;
        }

        task.Team = team;

        // Проверяем ModelState после заполнения TaskModel
        if (!ModelState.IsValid)
        {
            var usersOnTeam = await _teamService.GetUsersForTeamAsync(task.TeamId);
            ViewBag.UsersOnTeam = usersOnTeam;

            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Поле: {state.Key}, Помилка: {error.ErrorMessage}");
                        Console.ResetColor();
                    }
                }
            }

            return View(task);
        }

        var taskTeam = await _teamService.GetTeamByIdAsync(task.TeamId);

        task.Team = taskTeam;

        await _taskService.AddTaskAsync(task);
        return RedirectToAction(nameof(Index), new { teamId = task.TeamId });
    }

    [HttpPost]
    public async Task<IActionResult> AssignToCurrentUser(Guid taskId, Guid teamId)
    {
        var user = await _userService.GetCurrentUserAsync();
        await _taskService.AssignTaskToUserAsync(taskId, user.Id);

        return RedirectToAction("Index", new { teamId = teamId });
    }

}

