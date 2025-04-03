using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
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
        private readonly InviteService _inviteService;

        public TaskController(/*ApplicationDbContext context,*/ UserService userService, TaskService taskService, InviteService inviteService)
        {
            //_context = context;
            _inviteService = inviteService;
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                var user = await _userService.GetCurrentUserAsync();
                var userTeams = await _teamService.GetAllTeamsForUserAsync(user.Id);
                var stopVar = new string("  ");
            }
            return View();
        }

        // GET: Task/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            return View();
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
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

    }
}